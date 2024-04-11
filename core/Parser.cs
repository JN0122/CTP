using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace CTP.core
{
    class Parser
    {
        private readonly DataTable datatable;
        private static readonly CultureInfo cultureInfo = new("pl-PL");

        ///<summary>
        /// Automatycznie wykrywa czy plik posiada nagłówek, tworzy obiekt DataTable z wartościami typu double.
        ///</summary>
        public Parser(string fileContent)
        {
            List<string> lines = new(fileContent.Split("\r\n"));
            char[] delimiter = { '\t', ';' };
            datatable = new DataTable();
            datatable.Locale = Parser.cultureInfo;
            List<string> columnHeaders = new(lines[0].Split(delimiter));
            bool firtLineHeader = !IsDouble(columnHeaders[0]);

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                if (firtLineHeader) datatable.Columns.Add(columnHeaders[i], typeof(double));
                else datatable.Columns.Add($"Column{i}", typeof(double));
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if ((i == 0 && firtLineHeader) || String.IsNullOrEmpty(lines[i])) continue;
                DataRow datarow = datatable.NewRow();
                datarow.ItemArray = lines[i].Split(delimiter);
                datatable.Rows.Add(datarow);
            }
        }

        ///<summary>
        /// Sprawdza czy string można przekonwertować do typu double.
        ///</summary>
        private static bool IsDouble(string field)
        {
            try
            {
                double.Parse(field, Parser.cultureInfo);
                return true;

            }
            catch (FormatException)
            {
                return false;
            }
        }

        ///<summary>
        /// Zwraca wartości w podanej kolumnie.
        ///</summary>
        public List<double> GetColumnValues(int columnIndex)
        {
            List<double> values = new();
            foreach (DataRow row in datatable.Rows)
            {
                values.Add((double)row[columnIndex]);
            }
            return values;
        }

        public DataTable GetDataTable()
        {
            return datatable;
        }
    }
}