using System.ComponentModel;

namespace FFF.Clipboard
{
    /// <summary>
    /// Represents the data format type.
    /// </summary>
    public enum TypeDataFormat : int
    {
        /// <summary>
        /// The data is in text format.
        /// </summary>
        [Description("Text")]
        Text = 0,

        /// <summary>
        /// The data is in path format.
        /// </summary>
        [Description("Path")]
        Path = 1,
    }

}
