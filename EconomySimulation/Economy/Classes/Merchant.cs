using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Timers;
using Economy.BusinessObjects;

namespace Economy.Classes
{
    public class Merchant
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Economy Economy { get; set; }

        public Merchant()
        {
            Logger.Debug("Initializing new merchant");
        }

        public void DayOver(object sender, ElapsedEventArgs e)
        {
            
        }

        private Good DetermineGoodWithBestPriceDifference()
        {
            Good retval = new Good();
            List<KeyValuePair<Good,float>> highestpricelist = (from l in Economy.Locations select l.Market.GetGoodWithHighestPrice()).ToList();
            highestpricelist.Sort(new Comparison<KeyValuePair<Good, float>>(
                (KeyValuePair<Good, float> a, KeyValuePair<Good, float> b) =>
                {
                    return a.Key.Value.CompareTo(b.Key.Value);
                }));
            return retval;
        }
    }
}
