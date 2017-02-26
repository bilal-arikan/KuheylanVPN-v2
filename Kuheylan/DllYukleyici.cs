using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Reflection; //DLL ler için (Assembly)
using System.IO;


namespace DLL
{
    class DllYukleyici
    {
        public DllYukleyici()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = string.Format("{0}.dll", new AssemblyName(args.Name).Name);
            Assembly assem = Assembly.GetExecutingAssembly();
            string resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));

            if (resourceName == null)
            {
                return null;
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                Console.Write(new AssemblyName(args.Name).Name + " Dll yuklendi\n");
                return Assembly.Load(assemblyData);
            }
        }
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
    }
}
