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
    public sealed class Measurement
    {
        public DataTable Table = new();

        public float MinTime = 0;
        public float MaxTime = 0;

        private Measurement() { }

        private static Measurement? _instance;

        public static Measurement GetInstance()
        {
            _instance ??= new Measurement();
            return _instance;
        }

        public void SetDataTable(DataTable Table)
        {
            this.Table = Table;

            MinTime = Table.Rows[1].Field<float>(0);
            MaxTime = Table.Rows[Table.Rows.Count - 1].Field<float>(0);
        }

        public List<float> GetValues(int ColumnIndex)
        {
            List<float> Values = new();
            foreach (DataRow Row in this.Table.Rows)
            {
                Values.Add(Row.Field<float>(ColumnIndex));
            }
            return Values;
        }
    }
}
