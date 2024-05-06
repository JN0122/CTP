using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    internal class DataScaler
    {
        public DataScaler() { }

        // default argument jest tymczasowo, do poprawienia
        public static DataTable ScaleData(DataTable OriginalData, int TargetDatapoints = 250)
        {

            decimal OriginalDataPointsCount = OriginalData.Rows.Count;

            decimal OriginalDtPtsPerNewDataPt = Math.Floor(OriginalDataPointsCount/(TargetDatapoints));

            decimal LeftoverDataPoints = 0;

            // dzielenie przez TargetDatapoints jest bez reszty
            if (OriginalDtPtsPerNewDataPt != (OriginalDataPointsCount/TargetDatapoints))
            {
                LeftoverDataPoints = OriginalDataPointsCount - (TargetDatapoints * OriginalDtPtsPerNewDataPt);
            }

            DataTable newDataTable = new DataTable();

            for (int i = 0; i < OriginalData.Columns.Count; i++)
            {
                newDataTable.Columns.Add(OriginalData.Columns[i].ColumnName, typeof(float));
            }

            //robimy tablice do liczenia srednich i czyscimy ja
            float[] newValues = new float[newDataTable.Columns.Count];
            for (int i = 0; i < newDataTable.Columns.Count; i++) newValues[i] = 0;


            for (int j = 0; j < TargetDatapoints; j++)
            {
                DataRow datarow = newDataTable.NewRow();

                for (int k = 0; k < OriginalData.Columns.Count; k++)
                {
                    for (int  l = 0; l < OriginalDtPtsPerNewDataPt; l++)
                    {
                        // ta super matma w indeksie sprawi ze przelecimy przez prawie wszystkie elementy DataTable oryginalnego
                        // nie liczac reszty z dzielenia przez te 250 na poczatku
                        newValues[k] += OriginalData.Rows[l + j * (int)OriginalDtPtsPerNewDataPt].Field<float>(k);
                    }

                    newValues[k] = newValues[k] / (float)OriginalDtPtsPerNewDataPt;

                }

                // nie wiem czemu ItemArray nie akceptuje float[] tylko object[]
                // wiec zamieniam jedno w drugie xd

                object[] rowarr = new object[newValues.Length];

                for (int i = 0; i < newValues.Length; i++)
                {
                    rowarr[i] = newValues[i];
                }

                datarow.ItemArray = rowarr;

                newDataTable.Rows.Add(datarow);
            }

            return newDataTable;
        }
    }
}
