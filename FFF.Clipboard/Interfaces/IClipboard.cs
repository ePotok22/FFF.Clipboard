using FFF.Clipboard;
using System.Threading.Tasks;
using System.Windows;

namespace FFF.Interfaces
{
    public interface IClipboard
    {
        /// <summary>
        /// Synchronously sets the specified text content onto the clipboard.
        /// </summary>
        /// <param name="content">The text to be set in the clipboard.</param>
        /// <param name="type">The custom type specifying the format of the content.</param>
        /// <param name="dataFormat">Specifies the format of the text (e.g., plain text, RTF) as defined in System.Windows.TextDataFormat.</param>
        void SetText(string content, TypeDataFormat type, TextDataFormat dataFormat);

        /// <summary>
        /// Asynchronously sets the specified text content onto the clipboard.
        /// </summary>
        /// <param name="content">The text to be set in the clipboard.</param>
        /// <param name="type">The custom type specifying the format of the content.</param>
        /// <param name="dataFormat">Specifies the format of the text (e.g., plain text, RTF) as defined in System.Windows.TextDataFormat.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetTextAsync(string content, TypeDataFormat type, TextDataFormat dataFormat);

        /// <summary>
        /// Synchronously retrieves text from the clipboard in the specified format.
        /// </summary>
        /// <param name="dataFormat">The format in which the text should be retrieved.</param>
        /// <returns>The text content from the clipboard.</returns>
        string GetText(TextDataFormat dataFormat);

        /// <summary>
        /// Asynchronously retrieves text from the clipboard in the specified format.
        /// </summary>
        /// <param name="dataFormat">The format in which the text should be retrieved.</param>
        /// <returns>A task that represents the asynchronous read operation. The task result contains the text content from the clipboard.</returns>
        Task<string> GetTextAsync(TextDataFormat dataFormat);

        /// <summary>
        /// Synchronously clears the content of the clipboard.
        /// </summary>
        void Clear();

        /// <summary>
        /// Asynchronously clears the content of the clipboard.
        /// </summary>
        /// <returns>A task representing the asynchronous clear operation.</returns>
        Task ClearAsync();
    }
}
