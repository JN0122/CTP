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
        private float vmin;
        private float vmax;
        private float mmin;
        private float mmax;

        public Column(string name, string rodzaj, float vmin, float vmax, float mmin, float mmax)
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

        public float Vmin
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

        public float Vmax
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

        public float Mmin
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

        public float Mmax
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
