using FFF.Helpers;
using FFF.Win32;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FFF.Clipboard
{
    /// <summary>
    /// Provides functionalities to manage the system clipboard, including setting and retrieving text in various formats.
    /// </summary>
    public sealed class ClipboardManager
    {
        // Constants for clipboard format types
        private const uint cfUnicodeText = 13;
        private const uint cfText = 1;

        /// <summary>
        /// Asynchronously sets the specified text content to the system clipboard in a given format.
        /// </summary>
        /// <param name="content">The text content to set in the clipboard.</param>
        /// <param name="type">The type of data being set (e.g., file path or plain text).</param>
        /// <param name="dataFormat">The format in which the text should be set in the clipboard.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="NotSupportedException">Thrown when there is an error setting the clipboard content.</exception>
        public static async Task SetTextAsync(string content, TypeDataFormat type, System.Windows.TextDataFormat dataFormat)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            try
            {
                await Task.Run(() =>
                {
                    switch (type)
                    {
                        // Handles clipboard operation for file paths
                        case TypeDataFormat.Path:
                            HandlePathContent(ref content);
                            if (!FileHelper.IsExists(content) && !DirectoryHelper.IsExists(content))
                                throw new FileNotFoundException("File path does not exist.");

                            StringCollection strcoll = new StringCollection { content };
                            System.Windows.Clipboard.SetFileDropList(strcoll);
                            break;
                        // Handles other types of clipboard content
                        default:
                            SetClipboardData(dataFormat, content);
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"Error setting clipboard content: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves text content from the system clipboard in a specified format.
        /// </summary>
        /// <param name="dataFormat">The format of the text to be retrieved from the clipboard.</param>
        /// <returns>A Task representing the asynchronous operation, containing the retrieved text.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is an error retrieving text from the clipboard.</exception>
        public static async Task<string> GetTextAsync(System.Windows.TextDataFormat dataFormat)
        {
            try
            {
                return await Task.Run(() =>
                {
                    System.Windows.IDataObject clipBoardBefore = System.Windows.Clipboard.GetDataObject();

                    switch (dataFormat)
                    {
                        case System.Windows.TextDataFormat.Text:
                            return GetClipboardData(System.Windows.DataFormats.Text);
                        case System.Windows.TextDataFormat.UnicodeText:
                            return GetClipboardData(System.Windows.DataFormats.UnicodeText);
                        case System.Windows.TextDataFormat.Rtf:
                            return GetClipboardData(System.Windows.DataFormats.Rtf);
                        case System.Windows.TextDataFormat.Html:
                            return GetClipboardData(System.Windows.DataFormats.Html);
                        case System.Windows.TextDataFormat.CommaSeparatedValue:
                            return GetClipboardData(System.Windows.DataFormats.CommaSeparatedValue);
                        case System.Windows.TextDataFormat.Xaml:
                            return GetClipboardData(System.Windows.DataFormats.Xaml);
                        default:
                            return null;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving text from clipboard: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Asynchronously clears the content of the system clipboard.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task ClearAsync()
        {
            await Task.Run(() =>
            {
                TryOpenClipboard();
                NativeMethods.EmptyClipboard();
                NativeMethods.CloseClipboard();
            });
        }

        /// <summary>
        /// Sets the clipboard data in a specific format.
        /// </summary>
        /// <param name="dataFormat">The format of the data to be set.</param>
        /// <param name="content">The content to be set in the clipboard.</param>
        private static void SetClipboardData(System.Windows.TextDataFormat dataFormat, string content)
        {
            switch (dataFormat)
            {
                // Set clipboard data based on the data format
                case System.Windows.TextDataFormat.Text:
                case System.Windows.TextDataFormat.UnicodeText:
                    InnerSet(content, dataFormat == System.Windows.TextDataFormat.Text ? cfText : cfUnicodeText);
                    break;
                default:
                    System.Windows.Clipboard.SetData(dataFormat.ToString(), content);
                    break;
            }
        }

        /// <summary>
        /// Gets the clipboard data in a specified format.
        /// </summary>
        /// <param name="format">The format of the data to retrieve from the clipboard.</param>
        /// <returns>The clipboard data in the specified format.</returns>
        private static string GetClipboardData(string format)
        {
            System.Windows.IDataObject clipboardBefore = System.Windows.Clipboard.GetDataObject();
            return clipboardBefore.GetDataPresent(format) ? clipboardBefore.GetData(format) as string : null;
        }

        /// <summary>
        /// Sets the specified content in the clipboard using native methods.
        /// </summary>
        /// <param name="text">The text to set in the clipboard.</param>
        /// <param name="cf">The clipboard format type.</param>
        private static void InnerSet(string text, uint cf)
        {
            TryOpenClipboard();
            NativeMethods.EmptyClipboard();
            IntPtr hGlobal = default;
            try
            {
                hGlobal = cf == cfText ? Marshal.StringToHGlobalAnsi(text) : Marshal.StringToHGlobalUni(text);
                if (NativeMethods.SetClipboardData(cf, hGlobal) == IntPtr.Zero) ThrowWin32();
                hGlobal = IntPtr.Zero;
            }
            finally
            {
                if (hGlobal != IntPtr.Zero) Marshal.FreeHGlobal(hGlobal);
                NativeMethods.CloseClipboard();
            }
        }

        /// <summary>
        /// Tries to open the clipboard with retries.
        /// </summary>
        /// <exception cref="Win32Exception">Thrown when the clipboard cannot be opened after several attempts.</exception>
        private static void TryOpenClipboard()
        {
            const int retryCount = 10;
            for (int i = 0; i < retryCount; i++)
            {
                if (NativeMethods.OpenClipboard(IntPtr.Zero)) return;
                Thread.Sleep(100);
            }
            ThrowWin32();
        }

        /// <summary>
        /// Throws a Win32Exception with the last error.
        /// </summary>
        private static void ThrowWin32() =>
            throw new Win32Exception(Marshal.GetLastWin32Error());

        /// <summary>
        /// Processes and sanitizes the path content.
        /// </summary>
        /// <param name="content">The file path content to be processed.</param>
        private static void HandlePathContent(ref string content)
        {
            content = string.Join(string.Empty, content.ToCharArray().Where(item => item != (char)8234));
            content = Path.GetFullPath(content);
        }
    }
}
