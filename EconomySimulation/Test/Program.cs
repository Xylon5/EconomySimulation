using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Economy;
using Economy.BusinessObjects;
using log4net;
using Economy.Classes;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //log4net.Util.LogLog.InternalDebugging = true;

            ILog Logger = LogManager.GetLogger(typeof(Program));
            Logger.Debug("Starting");

            Good fur = new Good() { InternalName = "Fur", Name = "Felle", Value = 100, DaysToProduce = 2 };
            Good wood = new Good() { InternalName = "Wood", Name = "Holz", Value = 90, DaysToProduce = 1 };
            Good steel = new Good() { InternalName = "Steel", Name = "Eisen", Value = 120, DaysToProduce = 3 };
            Good honey = new Good() { InternalName = "Honey", Name = "Honig", Value = 70, DaysToProduce = 1 };
           
            List<Good> _goods = new List<Good>() { fur, wood, steel, honey };

            Location danzig = new Location(_goods) { Name = "Danzig", ProductionGoods = new List<Good>() { honey } };
            Location luebeck = new Location(_goods) { Name = "Lübeck", ProductionGoods = new List<Good>() { wood } };
            Location reval = new Location(_goods) { Name = "Reval", ProductionGoods = new List<Good>() { fur } };
            Location stockholm = new Location(_goods) { Name = "Stockholm", ProductionGoods = new List<Good>() { steel } };

            List<Location> _towns = new List<Location>() { luebeck, reval, stockholm, danzig };

            Economy.Economy _eco = new Economy.Economy(_towns, _goods);
            _eco.ChangeTimer(2);

            Console.ReadLine();
        }
    }
}
