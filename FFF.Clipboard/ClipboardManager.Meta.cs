using FFF.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace FFF.Clipboard
{
    /// <summary>
    /// Provides functionalities to manage the system clipboard, including setting and retrieving text in various formats.
    /// </summary>
    public partial class ClipboardManager
    {
        // Constants for clipboard format types
        private const uint cfUnicodeText = 13;
        private const uint cfText = 1;

        /// <summary>
        /// Sets the clipboard data in a specific format.
        /// </summary>
        /// <param name="dataFormat">The format of the data to be set.</param>
        /// <param name="content">The content to be set in the clipboard.</param>
        private static void SetClipboardData(TextDataFormat dataFormat, string content)
        {
            switch (dataFormat)
            {
                // Set clipboard data based on the data format
                case TextDataFormat.Text:
                case TextDataFormat.UnicodeText:
                    InnerSet(content, dataFormat == TextDataFormat.Text ? cfText : cfUnicodeText);
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
            IDataObject clipboardBefore = System.Windows.Clipboard.GetDataObject();
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
