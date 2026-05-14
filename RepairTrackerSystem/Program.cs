using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepairTrackerSystem.Core;
using System.Windows.Forms;

namespace RepairTrackerSystem
{
    internal static class Program
    {
    
        [STAThread]
        static void Main()
        {
            DatabaseInitializer.Initialize(); // 🔥 ADD THIS

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
