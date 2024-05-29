using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public List<float> GetDistanceValues(int ColumnIndex)
        {
            List<float> Values = new();
            ColumnViewModel allColumns = new();

            allColumns.AddColumns(_instance);

            List<Column> providedColumns = allColumns.GetColumns();
            DataTable newTable = VtoMm2(this.Table, providedColumns, true);

            foreach (DataRow Row in newTable.Rows)
            {
                Values.Add(Row.Field<float>(ColumnIndex));
            }
            return Values;
        }

        public List<float> GetVelocityValues(int ColumnIndex)
        {
            List<float> Values = new();
            ColumnViewModel allColumns = new();

            allColumns.AddColumns(_instance);

            List<Column> providedColumns = allColumns.GetColumns();
            DataTable newTable = VtoMm2(this.Table, providedColumns, true);

            foreach (DataRow Row in newTable.Rows)
            {
                int index = newTable.Rows.IndexOf(Row);
                Values.Add(CalculateVelocity(index, ColumnIndex));
            }
            return Values;
        }

        public List<float> GetAccelerationValues(int ColumnIndex)
        {
            List<float> Values = new();
            ColumnViewModel allColumns = new();

            allColumns.AddColumns(_instance);

            List<Column> providedColumns = allColumns.GetColumns();
            DataTable newTable = VtoMm2(this.Table, providedColumns, true);

            foreach (DataRow Row in newTable.Rows)
            {
                int index = newTable.Rows.IndexOf(Row);
                Values.Add(CalculateAcceleration(index, ColumnIndex));
            }
            return Values;
        }

        public float CalculateVelocity(int index , int ColumnIndex)
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
                    xValues[i] = Table.Rows[index + offset].Field<Single>(ColumnIndex);
                }

                offset++;
            }

            return Reglinp.FitLine(yValues, xValues);
        }

        public float CalculateAcceleration(int index, int ColumnIndex)
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
                    xValues[i] = CalculateVelocity(index + offset, ColumnIndex);
                }

                offset++;
            }

            return Reglinp.FitLine(yValues, xValues);
        }

        public int ColumnsCount()
        {
            return Table.Columns.Count;
        }

        [Obsolete("Use VToMM2 instead")]
        public static DataTable VtoMm(double VLow, double VHigh, double MmLow, double MmHigh, DataTable originalData, bool truncateToMmHigh = false)
        {
            /// <summary>
            /// truncateToMmHigh - wartości które przekraczałyby górny zakres czujnika
            /// zostaną "przycięte" do MmHigh
            /// </summary>
            //double offset1 = -VLow;
            //double offset2 = -MmLow;
            if (VLow == VHigh || MmLow == MmHigh) throw new ArgumentException("Division by 0 in VtoMm conversion has ocurred\n");

            DataTable returnTable = new DataTable();
            returnTable.Columns.Add("Mm", typeof(double));

            double scale = (MmHigh - MmLow) / (VHigh - VLow);
            double newval = 0;

            foreach (DataRow row in originalData.Rows)
            {

                newval = (row.Field<double>(0) - VLow) * scale;
                if (truncateToMmHigh == true && newval > MmHigh) newval = MmHigh;
                returnTable.Rows.Add(newval);

            }

            return returnTable;
        }

        public static DataTable VtoMm2(DataTable originalData, List<Column> ColumnsProvided, bool truncateToMmHigh = true)
        {

            if (originalData.Columns.Count != ColumnsProvided.Count) 
            {
                throw new ArgumentException("Column count mismatch in VToMM conversion");
            }


            //create and initialize a returnTable
            DataTable returnTable  = new DataTable();

            for (int i = 0; i < originalData.Columns.Count; i++)
            {
                returnTable.Columns.Add(ColumnsProvided[i].Name, typeof(string));
            }

            DataRow[] AuxilliaryRowArray;

            for (int i = 0; i < originalData.Columns.Count; i++)
            {
                //nie chcemy edytowac kolumn ktore nie sa typu double
                if (originalData.Columns[i].DataType == typeof(string)) break;


                switch (ColumnsProvided[i].Rodzaj)
                {
                    case "Napięciowy":

                        //float scale = (ColumnsProvided[i].Mmax - ColumnsProvided[i].Mmin) / (ColumnsProvided[i].Vmax - ColumnsProvided[i].Vmin);
                        float scale = (6 - ColumnsProvided[i].Mmin) / (2 - ColumnsProvided[i].Vmin);
                        float newval = 0;

                        foreach (DataRow row in originalData.Rows)
                        {
                            //Jezus maria
                            newval = (row.Field<float>(i) - ColumnsProvided[i].Vmin) * scale;
                            if (truncateToMmHigh == true && newval > ColumnsProvided[i].Mmax) newval = ColumnsProvided[i].Mmax;
                            row.SetField<float>(originalData.Columns[i], newval);
                            //returnTable.Rows.Add(newval);

                        }

                        break;
                    case "Impulsowy":
                        //AuxilliaryRowArray = ImpulseToMm(originalData.Select(originalData.Columns[i].ColumnName), ColumnsProvided[i]);
                        break;
                    default:
                        //TODO
                        break;
                }


            }

            return originalData;
        }

        static void ImpulseToMm(DataRow[] Rows, Column Col, int IndexLast, int IndexFirst, int VConst, bool truncateOutOfRangeValues = true)
        {
            //średnica elementu obrotowego d


            //return new DataRow[Rows.Length];
        }

        //static DataRow[] SignalToMm(DataRow[] Rows, Column Col, bool truncateOutOfRangeValues = true)
        //{
        //    return new DataRow[Rows.Length];
        //}

        public DataTable GetDataTable()
        {
            return this.Table;
        }
    }
}
