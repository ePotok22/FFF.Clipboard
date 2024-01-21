using FFF.Helpers;
using FFF.Win32;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Threading.Tasks;

namespace FFF.Clipboard
{
    /// <summary>
    /// Provides functionalities to manage the system clipboard, including setting and retrieving text in various formats.
    /// </summary>
    public partial class ClipboardManager
    {
        /// <summary>
        /// Synchronously sets the specified text content to the system clipboard in a given format. This method wraps
        /// the asynchronous operation in a Task.Run to avoid deadlocks and waits for its completion.
        /// It's designed for scenarios where asynchronous execution is not possible or desired, but should be used
        /// with caution to avoid blocking the main thread in UI applications.
        /// </summary>
        /// <param name="content">The text content to set in the clipboard.</param>
        /// <param name="type">The type of data being set (e.g., file path or plain text).</param>
        /// <param name="dataFormat">The format in which the text should be set in the clipboard.</param>
        public static void SetText(string content, TypeDataFormat type, TextDataFormat dataFormat)
        {
            // Use ConfigureAwait(false) to avoid capturing the synchronization context and reduce the risk of deadlocks.
            Task.Run(async () => await SetTextAsync(content, type, dataFormat).ConfigureAwait(false)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously sets the specified text content to the system clipboard in a given format.
        /// </summary>
        /// <param name="content">The text content to set in the clipboard.</param>
        /// <param name="type">The type of data being set (e.g., file path or plain text).</param>
        /// <param name="dataFormat">The format in which the text should be set in the clipboard.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="NotSupportedException">Thrown when there is an error setting the clipboard content.</exception>
        public static async Task SetTextAsync(string content, TypeDataFormat type, TextDataFormat dataFormat)
        {
            if (string.IsNullOrWhiteSpace(content))  return;

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
        /// Synchronously retrieves text content from the system clipboard in a specified format. This method wraps
        /// the asynchronous operation in a Task.Run to avoid deadlocks and waits for its completion.
        /// It's designed for scenarios where asynchronous execution is not possible or desired, but should be used
        /// with caution to avoid blocking the main thread in UI applications.
        /// </summary>
        /// <param name="dataFormat">The format of the text to be retrieved from the clipboard.</param>
        /// <returns>The text content retrieved from the clipboard in the specified format.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is an error retrieving text from the clipboard.</exception>
        public static string GetText(TextDataFormat dataFormat)
        {
            // Use ConfigureAwait(false) to avoid capturing the synchronization context and reduce the risk of deadlocks.
            string result = Task.Run(async () => await GetTextAsync(dataFormat).ConfigureAwait(false)).GetAwaiter().GetResult();
            return result;
        }

        /// <summary>
        /// Asynchronously retrieves text content from the system clipboard in a specified format.
        /// </summary>
        /// <param name="dataFormat">The format of the text to be retrieved from the clipboard.</param>
        /// <returns>A Task representing the asynchronous operation, containing the retrieved text.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is an error retrieving text from the clipboard.</exception>
        public static async Task<string> GetTextAsync(TextDataFormat dataFormat)
        {
            try
            {
                return await Task.Run(() =>
                {
                   IDataObject clipBoardBefore = System.Windows.Clipboard.GetDataObject();

                    switch (dataFormat)
                    {
                        case TextDataFormat.Text:
                            return GetClipboardData(DataFormats.Text);
                        case TextDataFormat.UnicodeText:
                            return GetClipboardData(DataFormats.UnicodeText);
                        case TextDataFormat.Rtf:
                            return GetClipboardData(DataFormats.Rtf);
                        case TextDataFormat.Html:
                            return GetClipboardData(DataFormats.Html);
                        case TextDataFormat.CommaSeparatedValue:
                            return GetClipboardData(DataFormats.CommaSeparatedValue);
                        case TextDataFormat.Xaml:
                            return GetClipboardData(DataFormats.Xaml);
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
        /// Clears the clipboard content synchronously. This method wraps the asynchronous clear operation in a Task.Run
        /// to avoid deadlocks and waits for its completion. It's designed to be used in scenarios where asynchronous
        /// execution is not possible or desired, but care should be taken to avoid blocking the main thread in UI applications.
        /// </summary>
        public static void Clear()
        {
            // Task.Run is used to offload the asynchronous operation to a thread pool thread.
            // This avoids deadlock issues that might occur with .Wait() or .Result in a context with a synchronization context.
            // GetAwaiter().GetResult() is used for waiting on the task's completion without throwing an AggregateException.
            Task.Run(async () => await ClearAsync()).GetAwaiter().GetResult();
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
    }
}
