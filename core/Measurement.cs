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

            foreach (DataRow Row in _table.Rows)
            {
                double value = Row.Field<double>(1);
                if (value > MaxValue) MaxValue = value;
                else if(value < MinValue) MinValue = value;
            }
        }

        public double? ReadValue(int index, int col = 1)
        {
            if (_table == null || index >= ValueCount()) return null;

            return _table.Rows[index].Field<double>(col);
        }

        public int ValueCount()
        {
            return _table.Rows.Count;
        }
    }
}
