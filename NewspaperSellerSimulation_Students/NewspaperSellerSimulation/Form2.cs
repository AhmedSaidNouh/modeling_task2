using NewspaperSellerModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewspaperSellerSimulation
{
    public partial class Form2 : Form
    {
        SimulationSystem sim;
        public Form2(SimulationSystem simulation)
        {
            InitializeComponent();
            sim = simulation;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var binding = new BindingList<SimulationCase>(sim.SimulationTable);
            var src = new BindingSource(binding, null);
            dataGridView1.DataSource = src;
            TotalSalesProfit.Text = sim.PerformanceMeasures.TotalSalesProfit.ToString();
            TotalCost.Text= sim.PerformanceMeasures.TotalCost.ToString();
            TotalLostProfit.Text= sim.PerformanceMeasures.TotalLostProfit.ToString();
            TotalScrapProfit.Text = sim.PerformanceMeasures.TotalScrapProfit.ToString();
            TotalNetProfit.Text= sim.PerformanceMeasures.TotalNetProfit.ToString();
            DaysWithMoreDemand.Text= sim.PerformanceMeasures.DaysWithMoreDemand.ToString();
            DaysWithUnsoldPapers.Text= sim.PerformanceMeasures.DaysWithUnsoldPapers.ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
