using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Economy.Classes;

namespace Economy.BusinessObjects
{
    public class Market
    {
        public Dictionary<Good, float> Pricelist { get; private set; }
        private Dictionary<Good, float> Countlist { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event Func<Good, float, float, float, float> OnGoodBuyedEvent;
        public event Func<Good, float, float, float, float> OnGoodSoldEvent;
        public Economy Economy;

        public Market(List<Good> InitialGoodList)
        {
            Logger.Debug("Initializing new market");
            this.Pricelist = new Dictionary<Good, float>();
            this.Countlist = new Dictionary<Good, float>();

            InitialGoodList.ForEach((good) => {
                Logger.DebugFormat("Adding good '{0}' to stocklist", good.InternalName);
                this.Pricelist.Add(good, good.Value);
                Logger.DebugFormat("Adding good '{0}' to countlist", good.InternalName);
                this.Countlist.Add(good, 0.1f);
            });
        }

        public bool SellGood(Good good)
        {
            Logger.DebugFormat("Selling good '{0}'", good.InternalName);
            Countlist[good]--;
            InvokeOnGoodSoldEvent(good, Pricelist[good]);
            Logger.DebugFormat("Selling good '{0}', current quantity is '{1}' and current price is '{2}'", good.InternalName, Countlist[good], Pricelist[good]);
            return true;
        }

        public bool BuyGood(Good good, float price)
        {
            bool buyed = false;
            if (Pricelist[good] <= price)
            {
                Countlist[good]++;
                Logger.DebugFormat("Buying good '{0}' for '{1}', current quantity is '{2}'", good.InternalName, price, Countlist[good]);
                buyed = true;
                InvokeOnGoodBuyedEvent(good, price);
            }
            return buyed;
        }

        public KeyValuePair<Good, float> GetGoodWithHighestPrice()
        {
            return (from entry in Pricelist orderby entry.Value descending select entry).First();
        }

        internal void AddGood(Good good)
        {
            Countlist[good] = Countlist[good] + 1;
            InvokeOnGoodBuyedEvent(good, Pricelist[good]);
        }

        private void InvokeOnGoodSoldEvent(Good good, float price)
        {
            if (OnGoodSoldEvent != null)
            {
                Pricelist[good] = OnGoodSoldEvent(good, price, Pricelist[good], Countlist[good]);
            }
            else
            {
                Logger.DebugFormat("No OnGoodSoldEvent defined");
            }
        }

        private void InvokeOnGoodBuyedEvent(Good good, float price)
        {
            if (OnGoodBuyedEvent != null)
            {
                Pricelist[good] = OnGoodBuyedEvent(good, price, Pricelist[good], Countlist[good]);
            }
            else
            {
                Logger.DebugFormat("No OnGoodBuyedEvent defined");
            }     
        }

        public KeyValuePair<float, float> GetGoodInfo(Good good)
        {
            return new KeyValuePair<float, float>(Countlist[good], Pricelist[good]);
        }
    }
}
