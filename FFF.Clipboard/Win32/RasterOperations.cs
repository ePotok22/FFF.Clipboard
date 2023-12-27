using System;

namespace FFF.Win32
{
    /// <summary>
    /// Represents a set of raster operations used in graphics and image processing.
    /// These operations define how pixel data is combined when drawing or manipulating images.
    /// </summary>
    [Flags]
    internal enum RasterOperations
    {
        /// <summary>
        /// Copies the source directly to the destination.
        /// </summary>
        SRCCOPY = 0x00CC0020,

        /// <summary>
        /// Combines the source and destination using the logical OR operation.
        /// </summary>
        SRCPAINT = 0x00EE0086,

        /// <summary>
        /// Combines the source and destination using the logical AND operation.
        /// </summary>
        SRCAND = 0x008800C6,

        /// <summary>
        /// Inverts the source.
        /// </summary>
        SRCINVERT = 0x00660046,

        /// <summary>
        /// Erases the destination with the source.
        /// </summary>
        SRCERASE = 0x00440328,

        /// <summary>
        /// Copies the inverted source to the destination.
        /// </summary>
        NOTSRCCOPY = 0x00330008,

        /// <summary>
        /// Combines the inverted source and destination using the logical AND operation.
        /// </summary>
        NOTSRCERASE = 0x001100A6,

        /// <summary>
        /// Merges the source and destination using a simple copy operation.
        /// </summary>
        MERGECOPY = 0x00C000CA,

        /// <summary>
        /// Combines the source and destination using a logical OR operation.
        /// </summary>
        MERGEPAINT = 0x00BB0226,

        /// <summary>
        /// Copies the pattern to the destination.
        /// </summary>
        PATCOPY = 0x00F00021,

        /// <summary>
        /// Combines the pattern and destination using the logical OR operation.
        /// </summary>
        PATPAINT = 0x00FB0A09,

        /// <summary>
        /// Inverts the pattern.
        /// </summary>
        PATINVERT = 0x005A0049,

        /// <summary>
        /// Inverts the destination.
        /// </summary>
        DSTINVERT = 0x00550009,

        /// <summary>
        /// Sets all destination pixels to black.
        /// </summary>
        BLACKNESS = 0x00000042,

        /// <summary>
        /// Sets all destination pixels to white.
        /// </summary>
        WHITENESS = 0x00FF0062,

        /// <summary>
        /// Captures the screen image in a window (Windows version 5.0.0 or later).
        /// </summary>
        CAPTUREBLT = 0x40000000 // Only if Windows version is 5.0.0 or later (see wingdi.h)
    }
}
