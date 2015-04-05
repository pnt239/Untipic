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
            Assembly assembly = Assembly.LoadFrom("Untipic.UI.dll");
#if MONO
            Type type = assembly.GetType("Untipic.UI.Mono.MainForm");
#else
            Type type = assembly.GetType("Untipic.UI.Net.MainForm");
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form instanceOfMyType = (Form)Activator.CreateInstance(type);
            Application.Run(instanceOfMyType);
        }
    }
}
