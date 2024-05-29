using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Markup;

namespace CTP.core
{
    public class Column : INotifyPropertyChanged
    {

        private string name;
        private string rodzaj;
        private double vmin;
        private double vmax;
        private double mmin;
        private double mmax;

        public Column(string name, string rodzaj, double vmin, double vmax, double mmin, double mmax)
        {
            this.name = name;
            this.rodzaj = rodzaj;
            this.vmin = vmin;
            this.vmax = vmax;
            this.mmin = mmin;
            this.mmax = mmax;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Rodzaj
        {
            get
            {
                return rodzaj;
            }

            set
            {
                rodzaj = value;
                OnPropertyChanged("Rodzaj");
            }
        }

        public double Vmin
        {
            get
            {
                return vmin;
            }

            set
            {
                vmin = value;
                OnPropertyChanged("Vmin");
            }
        }

        public double Vmax
        {
            get
            {
                return vmax;
            }

            set
            {
                vmax = value;
                OnPropertyChanged("Vmax");
            }
        }

        public double Mmin
        {
            get
            {
                return mmin;
            }

            set
            {
                mmin = value;
                OnPropertyChanged("Mmin");
            }
        }

        public double Mmax
        {
            get
            {
                return mmax;
            }

            set
            {
                mmax = value;
                OnPropertyChanged("Mmax");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
