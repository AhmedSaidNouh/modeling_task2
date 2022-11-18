using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string path = "../../TestCases/";
            //string test = "TestCase2.txt";
            //string final_path = path + test;
            //NewspaperSellerModels.SimulationSystem simulation = new NewspaperSellerModels.SimulationSystem();
            //simulation.readFromFile(final_path, simulation);
            //simulation.calculateSimulationTable();
            //simulation.calulate_proformance();
            //string x = TestingManager.Test(simulation, Constants.FileNames.TestCase2);
            //MessageBox.Show(x);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
