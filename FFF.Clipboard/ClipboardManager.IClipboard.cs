using FFF.Interfaces;
using System.Threading.Tasks;
using System.Windows;

namespace FFF.Clipboard
{
    /// <summary>
    /// Provides functionalities to manage the system clipboard, including setting and retrieving text in various formats.
    /// </summary>
    public partial class ClipboardManager : IClipboard
    {
        // <summary>
        /// Synchronously sets the specified text content onto the clipboard.
        /// This method is an explicit interface member implementation.
        /// </summary>
        /// <param name="content">The text to be set in the clipboard.</param>
        /// <param name="type">The custom type specifying the format of the content.</param>
        /// <param name="dataFormat">Specifies the format of the text (e.g., plain text, RTF) as defined in System.Windows.TextDataFormat.</param>
        void IClipboard.SetText(string content, TypeDataFormat type, TextDataFormat dataFormat) =>
            SetText(content, type, dataFormat);

        /// <summary>
        /// Asynchronously sets the specified text content onto the clipboard.
        /// This method is an explicit interface member implementation.
        /// </summary>
        /// <param name="content">The text to be set in the clipboard.</param>
        /// <param name="type">The custom type specifying the format of the content.</param>
        /// <param name="dataFormat">Specifies the format of the text (e.g., plain text, RTF) as defined in System.Windows.TextDataFormat.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        async Task IClipboard.SetTextAsync(string content, TypeDataFormat type, TextDataFormat dataFormat) =>
            await SetTextAsync(content, type, dataFormat);

        /// <summary>
        /// Synchronously retrieves text from the clipboard in the specified format.
        /// This method is an explicit interface member implementation.
        /// </summary>
        /// <param name="dataFormat">The format in which the text should be retrieved.</param>
        /// <returns>The text content from the clipboard.</returns>
        string IClipboard.GetText(TextDataFormat dataFormat) =>
            GetText(dataFormat);

        /// <summary>
        /// Asynchronously retrieves text from the clipboard in the specified format.
        /// This method is an explicit interface member implementation.
        /// </summary>
        /// <param name="dataFormat">The format in which the text should be retrieved.</param>
        /// <returns>A task that represents the asynchronous read operation. The task result contains the text content from the clipboard.</returns>
        async Task<string> IClipboard.GetTextAsync(TextDataFormat dataFormat) =>
            await GetTextAsync(dataFormat);

        /// <summary>
        /// Synchronously clears the content of the clipboard.
        /// This method is an explicit interface member implementation.
        /// </summary>
        void IClipboard.Clear() =>
            Clear();

        /// <summary>
        /// Asynchronously clears the content of the clipboard.
        /// This method is an explicit interface member implementation.
        /// </summary>
        /// <returns>A task representing the asynchronous clear operation.</returns>
        async Task IClipboard.ClearAsync() =>
            await ClearAsync();
    }
}
