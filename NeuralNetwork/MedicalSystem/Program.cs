using NeuralNetwork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalSystem
{
    internal static class Program
    {
        public static SystemController Controller { get; private set; }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Controller = new SystemController(ImportHelper.ImportNetwork("network.txt"));
      
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MedicalSystem());
        }
    }
}
