using System;
using System.Linq;
using System.Reflection; //DLL ler için (Assembly)
using System.IO;


namespace DLL
{
    /// <summary>
    /// Kullanımı:
    /// *.Dll dosyalarının Propertieslerindeki
    /// > BuildAction = Embedded Resource
    /// > CopyToOutput = Always Copy
    /// Reference Propertiesleride
    /// > CopyLocal = False
    /// Olacak
    /// </summary>
    class DllYukleyici
    {
        public DllYukleyici()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(">>>"+args.Name + new AssemblyName(args.Name).Name);
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
                Console.WriteLine(new AssemblyName(args.Name).Name + " Dll yuklendi");
                return Assembly.Load(assemblyData);
            }
        }
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        /*System.Reflection.Assembly CurrentDomain_AssemblyResolve2(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            return System.Reflection.Assembly.Load(bytes);
        }*/
    }
}
