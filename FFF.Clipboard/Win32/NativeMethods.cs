using System;
using System.Runtime.InteropServices;

namespace FFF.Win32
{
    internal static class NativeMethods
    {
        public const string KERNEL32 = "kernel32.dll";

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        internal static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport(KERNEL32, SetLastError = true)]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport(KERNEL32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        internal static extern bool EmptyClipboard();

        [DllImport(KERNEL32, SetLastError = true)]
        internal static extern int GlobalSize(IntPtr hMem);

    }
}
