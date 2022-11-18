using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        // Read From Test Case File
        public void readFromFile(string filePath, SimulationSystem sumlation)
        {
            
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            while (reader.Peek() != -1)
            {
                string lineRead = reader.ReadLine();
                if (lineRead == "NumOfNewspapers")
                {
                    NumOfNewspapers= int.Parse(reader.ReadLine());
                }
                else if(lineRead == "NumOfRecords")
                {
                    NumOfRecords= int.Parse(reader.ReadLine());
                }
                else if(lineRead == "PurchasePrice")
                {
                    PurchasePrice=decimal.Parse(reader.ReadLine());
                }
                else if (lineRead == "SellingPrice")
                {
                    SellingPrice = decimal.Parse(reader.ReadLine());
                }
                else if (lineRead == "ScrapPrice")
                {
                    ScrapPrice = decimal.Parse(reader.ReadLine());
                }
                else if (lineRead == "DayTypeDistributions")
                {
                    string intervalLine = reader.ReadLine();
                    string[] interval_Prob = intervalLine.Split(',');
                    for (int i = 0; i < interval_Prob.Length; i++)
                    {
                        DayTypeDistribution row = new DayTypeDistribution();
                        switch(i)
                        {
                            case 0:
                                row.DayType = Enums.DayType.Good;
                                break;
                            case 1:
                                row.DayType = Enums.DayType.Fair;
                                break;
                            case 2:
                                row.DayType = Enums.DayType.Poor;
                                break;


                        }
                            
        
                        row.Probability = decimal.Parse(interval_Prob[i]);
                        DayTypeDistributions.Add(row);
                    }
                    calculateCummProb_DayTypeDistributions();
                }
                else if(lineRead == "DemandDistributions")
                {
                    while (true)
                    {
                        string intervalLine = reader.ReadLine();
                        if (intervalLine == null)
                        {
                            break;
                        }
                        string[] interval = intervalLine.Split(',');          
                        DemandDistribution Raw = new DemandDistribution();
                        Raw.Demand=int.Parse(interval[0]);
                        for(int i=1;i<interval.Length;i++)
                        {
                            DayTypeDistribution row = new DayTypeDistribution();
                            switch (i-1)
                            {
                                case 0:
                                    row.DayType = Enums.DayType.Good;
                                    break;
                                case 1:
                                    row.DayType = Enums.DayType.Fair;
                                    break;
                                case 2:
                                    row.DayType = Enums.DayType.Poor;
                                    break;


                            }
                            row.Probability= decimal.Parse(interval[i]);
                            Raw.DayTypeDistributions.Add(row);
                        }

                        DemandDistributions.Add(Raw);
                    }
                    calculateCummProb_DemandDistributions();
                }
                //test randoms

            }
            fileStream.Close();
        }
        //Calculate Cumulative Probability DayTypeDistributions
        public void calculateCummProb_DayTypeDistributions()
        {
            decimal sumCProb = 0;
            foreach (DayTypeDistribution Row in DayTypeDistributions)
            {
                Row.MinRange = (int)(sumCProb * 100) + 1;
                sumCProb += Row.Probability;
                Row.CummProbability = sumCProb;
                Row.MaxRange = (int)(sumCProb * 100);
            }
        }
        //Calculate Cumulative Probability DayTypeDistributions
        public void calculateCummProb_DemandDistributions()
        {
            for (int i = 0; i < 3; i++)
            {
                decimal sumCProb = 0;
                foreach (DemandDistribution Row in DemandDistributions)
                {
              
                    Row.DayTypeDistributions[i].MinRange = (int)(sumCProb * 100) + 1;
                    sumCProb += Row.DayTypeDistributions[i].Probability;
                    Row.DayTypeDistributions[i].CummProbability = sumCProb;
                    Row.DayTypeDistributions[i].MaxRange = (int)(sumCProb * 100);
                    if (Row.DayTypeDistributions[i].MinRange > 100)
                    {
                        Row.DayTypeDistributions[i].MaxRange = 0;
                        Row.DayTypeDistributions[i].MinRange = 0;
                    }
                        
                }
            }
        }
        public void calculateSimulationTable()
        {
            Random rd = new Random();
            var randForDemands = new List<int>();
            var randForDays = new List<int>();
            for (int i = 0; i < NumOfRecords; i++) {
                randForDemands.Add(rd.Next(1, 100));
                randForDays.Add(rd.Next(1, 100));
            }
            SimulationCase raw = new SimulationCase();
            for (int i = 0; i < NumOfRecords; i++) {
                raw = new SimulationCase();
                raw.DayNo = i + 1;
                raw.RandomNewsDayType = randForDays[i];
                var day = DayTypeDistributions.Find(x => (randForDays[i] <= x.MaxRange) && (randForDays[i] >= x.MinRange));
                raw.NewsDayType = day.DayType;
                raw.RandomDemand = randForDemands[i];
                var Demand = 0;
                var index = 0;
                if (day.DayType == Enums.DayType.Good) index = 0;
                if (day.DayType == Enums.DayType.Fair) index = 1;
                if (day.DayType == Enums.DayType.Poor) index = 2;
                foreach (DemandDistribution Row in DemandDistributions)
                {
                    if((randForDemands[i] >= Row.DayTypeDistributions[index].MinRange)&&(randForDemands[i] <= Row.DayTypeDistributions[index].MaxRange))
                    {
                        Demand = Row.Demand;
                        break;
                    }
                }
                raw.Demand = Demand;
                raw.DailyCost = NumOfNewspapers * PurchasePrice;
                raw.SalesProfit = Demand * SellingPrice;
                raw.LostProfit = (raw.Demand - NumOfNewspapers) * (SellingPrice-PurchasePrice);
                if (raw.LostProfit <= 0) raw.LostProfit = 0;
                raw.ScrapProfit = (NumOfNewspapers - raw.Demand) * ScrapPrice;
                if (raw.ScrapProfit <= 0) raw.ScrapProfit = 0;
                raw.DailyNetProfit = raw.SalesProfit - raw.DailyCost - raw.LostProfit + raw.ScrapProfit;
                SimulationTable.Add(raw);
                //DemandDistribution demand = DemandDistributions.Find(x => (x.DayTypeDistributions.Find(z => (z.DayType.Equals( raw.NewsDayType)) &&(randForDemands[i]>=z.MinRange) && (randForDemands[i]<=z.MaxRange))));
                //raw.Demand;
            }

            foreach(SimulationCase Scase in SimulationTable)
            {
                Console.WriteLine(Scase.DayNo.ToString() + ' ' + Scase.RandomNewsDayType.ToString() + ' ' + Scase.NewsDayType.ToString() + ' ' + Scase.RandomDemand.ToString() + ' ' + Scase.Demand.ToString() + ' ' +Scase.DailyCost.ToString() + ' '+Scase.SalesProfit.ToString() + ' '+Scase.LostProfit.ToString() + ' '+ Scase.ScrapProfit.ToString() + ' '+Scase.DailyNetProfit.ToString());
            }
        }
        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
    }
}
