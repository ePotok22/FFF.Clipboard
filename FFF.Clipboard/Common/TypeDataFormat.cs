using System.ComponentModel;

namespace FFF.Clipboard
{
    public enum TypeDataFormat : uint
    {
        [Description("Text")]
        Text = 1,
        [Description("Path")]
        Path = 2,
    }
}
