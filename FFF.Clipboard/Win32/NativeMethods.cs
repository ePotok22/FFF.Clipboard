using System;
using System.Runtime.InteropServices;

namespace FFF.Win32
{
    internal static class NativeMethods
    {
        // Defines the name of the kernel32.dll, a core Windows library.
        public const string KERNEL32 = "kernel32.dll";

        /// <summary>
        /// Checks if a specific clipboard format is available.
        /// </summary>
        /// <param name="format">The clipboard format to check.</param>
        /// <returns>True if the format is available, otherwise false.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsClipboardFormatAvailable(uint format);

        /// <summary>
        /// Retrieves data from the clipboard in a specified format.
        /// </summary>
        /// <param name="uFormat">The clipboard format to retrieve.</param>
        /// <returns>A pointer to the clipboard data.</returns>
        [DllImport("User32.dll", SetLastError = true)]
        internal static extern IntPtr GetClipboardData(uint uFormat);

        /// <summary>
        /// Locks a global memory object and returns a pointer to the memory block.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object.</param>
        /// <returns>A pointer to the locked memory block.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        /// <summary>
        /// Decrements the lock count on a memory object.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object.</param>
        /// <returns>True if the memory is still locked; otherwise, false.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// Opens the clipboard for examination and prevents other applications from modifying the clipboard content.
        /// </summary>
        /// <param name="hWndNewOwner">A handle to the window to be associated with the open clipboard.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// Closes the clipboard.
        /// </summary>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseClipboard();

        /// <summary>
        /// Sets data in the clipboard in a specified format.
        /// </summary>
        /// <param name="uFormat">The format to set in the clipboard.</param>
        /// <param name="data">A pointer to the data to be set.</param>
        /// <returns>A handle to the data in the specified format.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        /// <summary>
        /// Empties the clipboard and frees handles to data in the clipboard.
        /// </summary>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll")]
        internal static extern bool EmptyClipboard();

        /// <summary>
        /// Returns the size of a specified global memory object, in bytes.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object.</param>
        /// <returns>The size of the specified object, in bytes.</returns>
        [DllImport(KERNEL32, SetLastError = true)]
        internal static extern int GlobalSize(IntPtr hMem);
    }
}
