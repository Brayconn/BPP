using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace BPP
{
    static class ShellOpener
    {
        public const string WINDOWS_EXPLORER = "explorer.exe";
        public const string MAC_OPEN = "open";
        public const string LINUX_OPEN = "xdg-open";
        public static void OpenPath(string path)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    Process.Start(WINDOWS_EXPLORER, $"/select,\"{path}\"");
                    break;
                case PlatformID.MacOSX:
                    Process.Start(MAC_OPEN, $"-R \"{path}\"");
                    break;
                case PlatformID.Unix:
                    Process.Start(LINUX_OPEN, $"\"{Path.GetDirectoryName(path)}\""); //TODO figure out how to select the file
                    break;
            }
        }
        public static void Open(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        Process.Start(WINDOWS_EXPLORER, $"\"{path}\"");
                        break;
                    case PlatformID.MacOSX:
                        Process.Start(MAC_OPEN, $"\"{path}\"");
                        break;
                    case PlatformID.Unix:
                        Process.Start(LINUX_OPEN, $"\"{path}\"");
                        break;
                }
            }
        }
        public static void OpenFileWith(string path, IntPtr handle)
        {
            if (File.Exists(path))
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        OpenAs(handle, path);
                        break;
                    //TODO implement mac open with
                    case PlatformID.MacOSX:
                        break;
                    //TODO implement linux open with
                    case PlatformID.Unix:
                        break;
                }
            }
        }

        #region some slightly modified stuff from stack overflow
        //https://stackoverflow.com/questions/23566667/launch-associated-program-or-show-open-with-dialog-from-another-program/32153874#32153874
        //https://stackoverflow.com/questions/9224693/how-can-i-display-the-open-with-dialog-for-an-unregistered-file-extension/21182262#21182262

        #region http://www.pinvoke.net/default.aspx/shell32/SHOpenWithDialog.html

        [DllImport("shell32.dll", EntryPoint = "SHOpenWithDialog", CharSet = CharSet.Unicode)]
        private static extern int SHOpenWithDialog(IntPtr hWndParent, ref tagOPENASINFO oOAI);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/bb773363(v=vs.85).aspx 
        private struct tagOPENASINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszClass;

            [MarshalAs(UnmanagedType.I4)]
            public tagOPEN_AS_INFO_FLAGS oaifInFlags;
        }

        [Flags]
        private enum tagOPEN_AS_INFO_FLAGS
        {
            OAIF_ALLOW_REGISTRATION = 0x00000001,   // Show "Always" checkbox
            OAIF_REGISTER_EXT = 0x00000002,   // Perform registration when user hits OK
            OAIF_EXEC = 0x00000004,   // Exec file after registering
            OAIF_FORCE_REGISTRATION = 0x00000008,   // Force the checkbox to be registration
            OAIF_HIDE_REGISTRATION = 0x00000020,   // Vista+: Hide the "always use this file" checkbox
            OAIF_URL_PROTOCOL = 0x00000040,   // Vista+: cszFile is actually a URI scheme; show handlers for that scheme
            OAIF_FILE_IS_URI = 0x00000080    // Win8+: The location pointed to by the pcszFile parameter is given as a URI
        }

        private static void DoOpenFileWith(IntPtr hwndParent, string sFilename)
        {
            tagOPENASINFO oOAI = new tagOPENASINFO();
            oOAI.cszFile = sFilename;
            oOAI.cszClass = string.Empty;
            oOAI.oaifInFlags = tagOPEN_AS_INFO_FLAGS.OAIF_ALLOW_REGISTRATION | tagOPEN_AS_INFO_FLAGS.OAIF_EXEC;
            SHOpenWithDialog(hwndParent, ref oOAI);
        }

        #endregion

        #region http://www.codeproject.com/Articles/13103/Calling-the-Open-With-dialog-box-from-your-applica

        [Serializable]
        private struct ShellExecuteInfo
        {
            public int Size;
            public uint Mask;
            public IntPtr hwnd;
            public string Verb;
            public string File;
            public string Parameters;
            public string Directory;
            public uint Show;
            public IntPtr InstApp;
            public IntPtr IDList;
            public string Class;
            public IntPtr hkeyClass;
            public uint HotKey;
            public IntPtr IconMonitorUnion;
            public IntPtr Process;
        }

        // Code For OpenWithDialog Box

        [DllImport("shell32.dll", SetLastError = true)]
        extern private static bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

        private const uint SW_NORMAL = 1;

        private static void OpenAsOld(IntPtr hwndParent, string file)
        {
            ShellExecuteInfo sei = new ShellExecuteInfo();
            sei.Size = Marshal.SizeOf(sei);
            sei.Verb = "openas";
            sei.File = file;
            sei.Show = SW_NORMAL;
            sei.hwnd = hwndParent;
            sei.Mask = 12; //(fmask = SEE_MASK_INVOKEIDLIST) This might fix windows 10 support in some edge case, idk
            if (!ShellExecuteEx(ref sei))
                throw new System.ComponentModel.Win32Exception();
        }

        #endregion

        public static void OpenAs(IntPtr hWndParent, string file)
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                DoOpenFileWith(hWndParent, file);
            }
            else
            {
                OpenAsOld(hWndParent, file);
            }
        }

        #endregion
    }
}
