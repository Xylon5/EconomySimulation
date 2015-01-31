using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Economy.BusinessObjects;
using System.Timers;
using log4net;
using Economy.Classes;

namespace Economy
{
    public class Economy
    {
        public event Action OnDayElapsed;

        public List<Location> Locations { get; private set; }
        public List<Good> GoodList { get; private set; }
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Timer _timer { get; set; }
        public int DaysElapsed { get; set; }

        public Economy(
            List<Location> Locations,
            List<Good> GoodList)
        {
            Logger.DebugFormat("Initializing economy");
            Logger.DebugFormat("Initializing locations");
            this.Locations = Locations;
            Logger.DebugFormat("Initializing good list");
            this.GoodList = GoodList;

            Logger.DebugFormat("Setting up timer");
            this._timer = new Timer(5000);
            this._timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            Locations.ForEach((location) =>
            {
                Logger.DebugFormat("Wiring up locations time elapsed event for location '{0}'", location.Name);
                this._timer.Elapsed += new ElapsedEventHandler(location.DayOver);
                location.Market.OnGoodBuyedEvent += new Func<Good, float, float, float, float>(Market_OnGoodBuyedEvent);
                location.Market.OnGoodSoldEvent += new Func<Good, float, float, float, float>(Market_OnGoodSoldEvent);
                location.Economy = this;
                location.Market.Economy = this;
            });            
            this._timer.Enabled = true;
        }

        float Market_OnGoodBuyedEvent(Good good, float price, float currentvalue, float currentcount)
        {
            Logger.DebugFormat("Calculating new good value for good '{0}'", good.InternalName);
            return good.Value / price;
        }

        float Market_OnGoodSoldEvent(Good good, float price, float currentvalue, float currentcount)
        {
            Logger.DebugFormat("Calculating new good value for good '{0}'", good.InternalName);
            return good.Value * price;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DaysElapsed++;
            Logger.DebugFormat(string.Format("Day '{0}' elapsed!", DaysElapsed));
            Locations.ForEach((location) => { location.DaysElapsed = DaysElapsed; });
            if (OnDayElapsed != null) OnDayElapsed.Invoke();
        }

        public void ChangeTimer(int secondsForOneDay)
        {
            Logger.DebugFormat("Timer change requested, setting new game time intervall '{0}'", secondsForOneDay);
            this._timer.Interval = secondsForOneDay * 1000;
        }

        public List<info> DumpMarkets()
        {
            List<info> marketdump = new List<info>();
            foreach (Good good in GoodList)
            {
                foreach (Location loc in Locations)
                {
                    marketdump.Add(new info()
                    {
                        good = good.Name,
                        market = loc.Name,
                        count = loc.Market.GetGoodInfo(good).Key,
                        price = loc.Market.GetGoodInfo(good).Value
                    });
                }
            }
            return marketdump;
        }
    }

    public class info
    {
        public string market { get; set; }
        public string good { get; set; }
        public float count { get; set; }
        public float price { get; set; }
    }
}
