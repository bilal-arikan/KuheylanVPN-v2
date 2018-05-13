using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Kuheylan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            if (args.Length > 0 && args[0] == "console")
                AllocConsole();
            
            DLL.DllYukleyici dll = new DLL.DllYukleyici();
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormLock("Programın Kayıtlı Olup Olmadığı Kontrol ediliyor...", Color.DarkCyan));
            Application.Run(new FormMain());
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
