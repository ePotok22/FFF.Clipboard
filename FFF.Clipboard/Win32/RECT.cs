using System.Runtime.InteropServices;

namespace FFF.Win32
{
    /// <summary>
    /// Represents a rectangular region with four integer coordinates: left, top, right, and bottom.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        /// <summary>
        /// The x-coordinate of the left edge of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// The y-coordinate of the top edge of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// The x-coordinate of the right edge of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// The y-coordinate of the bottom edge of the rectangle.
        /// </summary>
        public int bottom;
    }
}
