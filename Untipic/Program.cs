using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Untipic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if MONO
			Assembly assembly = Assembly.LoadFrom("Untipic.UI.Mono.dll");
            Type type = assembly.GetType("Untipic.UI.Mono.MainForm");
#else
			Assembly assembly = Assembly.LoadFrom("Untipic.UI.dll");
            Type type = assembly.GetType("Untipic.UI.MainForm");
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainForm = (Form)Activator.CreateInstance(type);
            Application.Run(mainForm);
        }
    }
}
