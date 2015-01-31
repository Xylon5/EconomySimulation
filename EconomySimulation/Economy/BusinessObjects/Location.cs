using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Economy.Classes;
using System.Timers;

namespace Economy.BusinessObjects
{
    public class Location
    {
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public string Name { get; set; }
        public List<Good> ProductionGoods { get; set; }
        public int DaysElapsed { get; set; }
        public Economy Economy { get; set; }

        public Market Market { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public Location(List<Good> AllGoods)
        {
            Logger.DebugFormat("Initializing new location");
            Market = new Market(AllGoods);
        }

        public void DayOver(object sender, ElapsedEventArgs e)
        {
            Logger.DebugFormat("Game day elapsed! Invoking functions at location '{0}'", Name);
            ProduceGoods();
        }

        private void ProduceGoods()
        {
            ProductionGoods.ForEach((good) =>
            {
                if (good.DaysToProduce % DaysElapsed == 0)
                {
                    Logger.DebugFormat("Producing good '{0}' at location '{1}'", good.InternalName, Name);
                    Market.AddGood(good);
                }
            });
        }
    }
}
