using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;

namespace NewspaperSellerSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (test.SelectedItem != null)
            {
                string path = "../../TestCases/";
                string Test = test.SelectedItem.ToString();
                string final_path = path + Test;
                SimulationSystem simulation = new SimulationSystem();
                simulation.readFromFile(final_path, simulation);
                simulation.calculateSimulationTable();
                simulation.calulate_proformance();
                string x=null;
                switch (Test)
                {
                    case "TestCase1.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase1);
                        break;
                    case "TestCase2.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase2);
                        break;
                    case "TestCase3.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase3);
                        break;

                }
                
                MessageBox.Show(x);

            }
            else
                MessageBox.Show("chooce test");

        }
    }
}
