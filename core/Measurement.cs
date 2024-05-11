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

        public List<float> GetVelocityValues(int ColumnIndex)
        {
            List<float> Values = new();
            foreach (DataRow Row in this.Table.Rows)
            {
                int index = Table.Rows.IndexOf(Row);
                Values.Add(CalculateVelocity(index));
            }
            return Values;
        }

        public List<float> GetAccelerationValues(int ColumnIndex)
        {
            List<float> Values = new();
            foreach (DataRow Row in this.Table.Rows)
            {
                int index = Table.Rows.IndexOf(Row);
                Values.Add(CalculateAcceleration(index));
            }
            return Values;
        }

        public float CalculateVelocity(int index)
        {
            double[] xValues = new double[10];
            double[] yValues = new double[10];

            int offset = -4;

            for (int i = 0; i < 10; i++)
            {
                if (index + offset < 4 || index + offset >= Table.Rows.Count)
                {
                    yValues[i] = 0;
                    xValues[i] = 0;
                }
                else
                {
                    yValues[i] = Table.Rows[index + offset].Field<Single>(0);
                    xValues[i] = Table.Rows[index + offset].Field<Single>(1);
                }

                offset++;
            }

            return Reglinp.FitLine(yValues, xValues);
        }

        public float CalculateAcceleration(int index)
        {
            double[] xValues = new double[10];
            double[] yValues = new double[10];

            int offset = -4;

            for (int i = 0; i < 10; i++)
            {
                if (index + offset < 4 || index + offset >= Table.Rows.Count)
                {
                    yValues[i] = 0;
                    xValues[i] = 0;
                }
                else
                {
                    yValues[i] = Table.Rows[index + offset].Field<Single>(0);
                    xValues[i] = CalculateVelocity(index + offset);
                }

                offset++;
            }

            return Reglinp.FitLine(yValues, xValues);
        }
    }
}
