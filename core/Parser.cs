using System.Data;
using System.Globalization;

namespace CTP.core
{
    static class Parser
    {
        private static DataTable? _datatable;
        private static CultureInfo cultureInfo = new("pl-PL");

        ///<summary>
        /// Tworzy obiekt DataTable z wartościami typu double, automatycznie wykrywa czy plik posiada nagłówek.
        ///</summary>
        public static DataTable Parse(string fileContent)
        {
            List<string> lines = new(fileContent.Replace(".", ",").Split("\r\n"));
            char[] delimiter = { '\t', ';' };

            _datatable = new()
            {
                Locale = cultureInfo
            };

            List<string> columnHeaders = new(lines[0].Split(delimiter));
            bool firtLineHeader = !Double.TryParse(columnHeaders[0], out _);

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                if (firtLineHeader) _datatable.Columns.Add(columnHeaders[i], typeof(double));
                else _datatable.Columns.Add($"Column{i}", typeof(double));
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if ((i == 0 && firtLineHeader) || String.IsNullOrEmpty(lines[i])) continue;
                DataRow datarow = _datatable.NewRow();
                datarow.ItemArray = lines[i].Split(delimiter);
                _datatable.Rows.Add(datarow);
            }
            return _datatable;
        }
    }
}