using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Economy.BusinessObjects;

namespace EconomyTest
{
    public partial class Form1 : Form
    {
        List<Good> _goods = new List<Good>();
        List<Location> _towns = new List<Location>();
        Economy.Economy _eco;
        object mobj = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Good fur = new Good() { InternalName = "Fur", Name = "Felle", Value = 100, DaysToProduce = 1 };
            Good wood = new Good() { InternalName = "Wood", Name = "Holz", Value = 90, DaysToProduce = 1 };
            Good steel = new Good() { InternalName = "Steel", Name = "Eisen", Value = 120, DaysToProduce = 1 };
            Good honey = new Good() { InternalName = "Honey", Name = "Honig", Value = 70, DaysToProduce = 1 };
            _goods.Add(fur);
            _goods.Add(wood);
            _goods.Add(steel);
            _goods.Add(honey);

            Location danzig = new Location(_goods) { Name = "Danzig", ProductionGoods = new List<Good>() { honey } };
            Location luebeck = new Location(_goods) { Name = "Lübeck", ProductionGoods = new List<Good>() { wood } };
            Location reval = new Location(_goods) { Name = "Reval", ProductionGoods = new List<Good>() { fur } };
            Location stockholm = new Location(_goods) { Name = "Stockholm", ProductionGoods = new List<Good>() { steel } };
            _towns.Add(danzig);
            _towns.Add(luebeck);
            _towns.Add(reval);
            _towns.Add(stockholm);

            _eco = new Economy.Economy(_towns, _goods);

            infoBindingSource.DataSource = _eco.DumpMarkets();
            _eco.ChangeTimer(1);
            timer1.Interval = 1000;
            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            infoBindingSource.ResetBindings(false);
            dataGridView1.Update();
            Form1.ActiveForm.Text = Convert.ToString(_eco.DaysElapsed);
        }
    }
}
