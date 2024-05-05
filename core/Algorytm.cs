using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    internal class Algorytm
    {
        public static DataTable VtoMm(double VLow, double VHigh, double MmLow, double MmHigh, DataTable originalData)
        {
            //double offset1 = -VLow;
            //double offset2 = -MmLow;
            if (VLow ==  VHigh || MmLow == MmHigh) throw new ArgumentException("Division by 0 in VtoMm conversion has ocurred\n");

            DataTable returnTable = new DataTable();
            returnTable.Columns.Add("Mm", typeof(double));

            double scale = (MmHigh-MmLow)/(VHigh-VLow);

            foreach (DataRow row in originalData.Rows) 
            {

                double newval = (row.Field<double>(0) - VLow) * scale + MmHigh;
                returnTable.Rows.Add(newval);
                
            }

            return returnTable;
        }
    }
}
