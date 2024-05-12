using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    public sealed class Measurements
    {
        private DataTable _table = new();
        public double MaxValue = 10;
        public double MinValue = 0;

        public double MinTime = 0;
        public double MaxTime = 0;

        public int AmtOfRows = 0;

        public Measurements() { }

        private static Measurements? _instance;

        public static Measurements GetInstance()
        {
            _instance ??= new Measurements();
            return _instance;
        }

        public void SetDataTable(DataTable Table)
        {
            _table = Table;

            AmtOfRows = ValueCount();

            MinTime = _table.Rows[1].Field<double>(0);
            MaxTime = _table.Rows[AmtOfRows-1].Field<double>(0);

            foreach (DataRow Row in _table.Rows)
            {
                double value = Row.Field<double>(1);

                if (value > MaxValue) MaxValue = value;
                else if(value < MinValue) MinValue = value;
            }
        }

        public double? ReadValue(int index, int col = 1)
        {
            if (_table == null || index >= AmtOfRows) return null;

            return _table.Rows[index].Field<double>(col);
        }

        public int ValueCount()
        {
            return _table.Rows.Count;
        }
        internal class Algorytm
        {
            public static DataTable VtoMm(double VLow, double VHigh, double MmLow, double MmHigh, DataTable originalData)
            {
                //double offset1 = -VLow;
                //double offset2 = -MmLow;
                if (VLow == VHigh || MmLow == MmHigh) throw new ArgumentException("Division by 0 in VtoMm conversion has ocurred\n");

                DataTable returnTable = new DataTable();
                returnTable.Columns.Add("Mm", typeof(double));

                double scale = (MmHigh - MmLow) / (VHigh - VLow);

                foreach (DataRow row in originalData.Rows)
                {

                    double newval = (row.Field<double>(0) - VLow) * scale + MmHigh;
                    returnTable.Rows.Add(newval);

                }

                return returnTable;
            }
        }
    }
}
