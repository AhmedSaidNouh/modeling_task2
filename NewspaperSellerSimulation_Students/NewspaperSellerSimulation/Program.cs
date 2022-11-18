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
            NewspaperSellerModels.SimulationSystem simulation = new NewspaperSellerModels.SimulationSystem();
            simulation.readFromFile("D:/TestCase1.txt", simulation);
            //foreach (NewspaperSellerModels.DemandDistribution s in simulation.DemandDistributions)
            //{
                
            //    Console.Write(s.Demand);
            //    Console.Write("_");
            //    Console.Write(s.Demand);
            //    Console.Write("_");
            //    Console.Write(s.Demand);
            //    Console.WriteLine();
            //}
            simulation.calculateSimulationTable();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
