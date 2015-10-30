﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SvgConverter;
using SvgToXaml.Infrastructure;

namespace SvgToXaml
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            int exitCode = 0;
            if (args.Length > 0)
            {
                RunConsole(args);
            }
            else
            {   //normale WPF-Applikationslogik
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            return exitCode;
        }

        private static void RunConsole(string[] args)
        {
            HConsoleHelper.InitConsoleHandles();

            CmdLineHandler.HandleCommandLine(args);

            HConsoleHelper.ReleaseConsoleHandles();
        }

        private static Dictionary<string, Assembly> loadedAsmsCache = new Dictionary<string, Assembly>(StringComparer.InvariantCultureIgnoreCase);
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly cachedAsm;
            if (loadedAsmsCache.TryGetValue(args.Name, out cachedAsm))
                return cachedAsm;

            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo != null && assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    return null;

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                var loadedAsm = Assembly.Load(assemblyRawBytes);
                loadedAsmsCache.Add(args.Name, loadedAsm);
                return loadedAsm;
            }
        }
    }
}
