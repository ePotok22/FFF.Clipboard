using FFF.Helpers;
using FFF.Win32;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FFF.Clipboard
{
    public sealed class ClipboardManager
    {
        private const uint cfUnicodeText = 13;
        private const uint cfText = 1;

        public static void SetText(string content, TypeDataFormat type, System.Windows.TextDataFormat dataFormat)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                Exception exception = null;
                Thread tempThread = null;
                switch (type)
                {
                    case TypeDataFormat.Path:
                        tempThread = new Thread(() =>
                        {
                            try
                            {
                                // Remove (char)8234 ""
                                content = string.Join(string.Empty, content.ToCharArray().Where(item => !item.Equals((char)8234)));
                                content = Path.GetFullPath(content);
                                // Check Path Exists
                                if (FileHelper.IsExists(content) || DirectoryHelper.IsExists(content))
                                {
                                    // Add to Collection
                                    StringCollection strcoll = new StringCollection();
                                    strcoll.Add(content);
                                    // Set File Drop List
                                    System.Windows.Clipboard.SetFileDropList(strcoll);

                                    Thread.Sleep(500);
                                }
                                else exception = new FileNotFoundException("File path does not exist.");
                            }
                            catch (Exception ex)
                            {
                                exception = ex;
                            }
                        });
                        break;
                    default:
                        tempThread = new Thread(() =>
                        {
                            try
                            {
                                Action<uint, string> setDataFormObjectNative = (cf, t) =>
                                {
                                    TryOpenClipboard();
                                    InnerSet(t, cf);
                                };
                                switch (dataFormat)
                                {
                                    case System.Windows.TextDataFormat.Text:
                                        setDataFormObjectNative(cfText, content);
                                        // Clipboard.SetData(DataFormats.Text, text);
                                        break;
                                    case System.Windows.TextDataFormat.UnicodeText:
                                        setDataFormObjectNative(cfUnicodeText, content);
                                        //Clipboard.SetData(DataFormats.UnicodeText, content);
                                        break;
                                    case System.Windows.TextDataFormat.Rtf:
                                        System.Windows.Clipboard.SetData(System.Windows.DataFormats.Rtf, content);
                                        break;
                                    case System.Windows.TextDataFormat.Html:
                                        System.Windows.Clipboard.SetData(System.Windows.DataFormats.Html, content);
                                        break;
                                    case System.Windows.TextDataFormat.CommaSeparatedValue:
                                        System.Windows.Clipboard.SetData(System.Windows.DataFormats.CommaSeparatedValue, content);
                                        break;
                                }
                                Thread.Sleep(500);
                            }
                            catch (Exception ex)
                            {
                                exception = ex;
                            }
                        });
                        break;
                }
                tempThread.SetApartmentState(ApartmentState.STA);
                tempThread.IsBackground = true;
                tempThread.Start();
                tempThread.Join();
                tempThread = null;
                if (exception != null) throw new NotSupportedException($"\"{content}\" " + exception.Message);
            }
        }

        public static string GetText(System.Windows.TextDataFormat dataFormat)
        {
            string text = null;
            ExceptionDispatchInfo tempExceptionInfo = null;
            Thread tempThread = new Thread(() =>
            {
                try
                {
                    System.Windows.IDataObject clipBoardBefore = System.Windows.Clipboard.GetDataObject();
                    Func<string, string> getDataFormObject = (textformat) =>
                    {
                        if (clipBoardBefore.GetDataPresent(textformat)) return clipBoardBefore.GetData(textformat) as string;
                        return null;
                    };
                    Func<uint, string> getDataFormObjectNative = (codeformat) =>
                    {
                        if (!NativeMethods.IsClipboardFormatAvailable(codeformat)) return null;
                        TryOpenClipboard();
                        return InnerGet(codeformat);
                    };
                    switch (dataFormat)
                    {
                        case System.Windows.TextDataFormat.Text:
                            //text = getDataFormObjectNative(cfText);
                            text = getDataFormObject(System.Windows.DataFormats.Text);
                            break;
                        case System.Windows.TextDataFormat.UnicodeText:
                            //text = getDataFormObjectNative(cfUnicodeText);
                            text = getDataFormObject(System.Windows.DataFormats.UnicodeText);
                            break;
                        case System.Windows.TextDataFormat.Rtf:
                            text = getDataFormObject(System.Windows.DataFormats.Rtf);
                            break;
                        case System.Windows.TextDataFormat.Html:
                            text = getDataFormObject(System.Windows.DataFormats.Html);
                            break;
                        case System.Windows.TextDataFormat.CommaSeparatedValue:
                            text = getDataFormObject(System.Windows.DataFormats.CommaSeparatedValue);
                            break;
                        case System.Windows.TextDataFormat.Xaml:
                            text = getDataFormObject(System.Windows.DataFormats.Xaml);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    tempExceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
            });
            tempThread.TrySetApartmentState(ApartmentState.STA);
            tempThread.IsBackground = true;
            tempThread.Start();
            tempThread.Join();
            tempThread = null;
            if (tempExceptionInfo != null) tempExceptionInfo.Throw();
            return text;
        }

        public static void Clear()
        {
            ExceptionDispatchInfo tempExceptionInfo = null;
            Thread tempThread = new Thread(() =>
            {
                try
                {
                    TryOpenClipboard();
                    NativeMethods.EmptyClipboard();
                    NativeMethods.CloseClipboard();
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    tempExceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
            });
            tempThread.SetApartmentState(ApartmentState.STA);
            tempThread.IsBackground = true;
            tempThread.Start();
            tempThread.Join();
            tempThread = null;
            if (tempExceptionInfo != null) tempExceptionInfo.Throw();
        }

        private static void InnerSet(string text, uint cf)
        {
            NativeMethods.EmptyClipboard();
            IntPtr hGlobal = default;
            try
            {
                int bytes = (text.Length + 1) * 2;
                hGlobal = Marshal.AllocHGlobal(bytes);
                if (hGlobal == default) ThrowWin32();
                IntPtr target = NativeMethods.GlobalLock(hGlobal);
                if (target == default) ThrowWin32();
                try
                {
                    if (cf == cfText) hGlobal = Marshal.StringToHGlobalAnsi(text);
                    else hGlobal = Marshal.StringToHGlobalUni(text);
                    Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                }
                finally { NativeMethods.GlobalUnlock(target); }
                if (NativeMethods.SetClipboardData(cf, hGlobal) == default) ThrowWin32();
                hGlobal = default;
            }
            finally
            {
                if (hGlobal != default) Marshal.FreeHGlobal(hGlobal);
                NativeMethods.CloseClipboard();
            }
        }

        private static string InnerGet(uint cf)
        {
            IntPtr handle = default;
            IntPtr pointer = default;
            try
            {
                handle = NativeMethods.GetClipboardData(cf);
                if (handle == default) return null;
                pointer = NativeMethods.GlobalLock(handle);
                if (pointer == default) return null;
                int size = NativeMethods.GlobalSize(handle);
                byte[] buff = new byte[size];
                Marshal.Copy(pointer, buff, 0, size);
                if (cf == cfUnicodeText) return Encoding.Unicode.GetString(buff).TrimEnd('\0');
                else return Encoding.ASCII.GetString(buff).TrimEnd('\0');
            }
            finally
            {
                if (pointer != default) NativeMethods.GlobalUnlock(handle);
                NativeMethods.CloseClipboard();
            }
        }

        private static void TryOpenClipboard()
        {
            int num = 10;
            while (true)
            {
                if (NativeMethods.OpenClipboard(default)) break;
                if (--num == 0) ThrowWin32();
                Thread.Sleep(100);
            }
        }

        private static void ThrowWin32() =>
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }
}
