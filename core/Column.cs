using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    internal class Column
    {

        private string Name;
        private string Rodzaj;
        private double Vmin, Vmax;
        private double Mmmin, Mmax;

        public Column(string Name, string Rodzaj)
        {
            this.Name = Name;
            this.Rodzaj = Rodzaj;

        }
    }
}
