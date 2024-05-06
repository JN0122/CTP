using System.Data;
using System.Globalization;

namespace CTP.core
{
    static class Parser
    {
        private static DataTable? _datatable;
        private readonly static CultureInfo cultureInfo = new("pl-PL");

        ///<summary>
        /// Tworzy obiekt DataTable z wartościami typu float, automatycznie wykrywa czy plik posiada nagłówek.
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
            bool firtLineHeader = !float.TryParse(columnHeaders[0], out _);

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                _datatable.Columns.Add(firtLineHeader ? columnHeaders[i] : $"Column{i}", typeof(float));
            }

            try
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if ((i == 0 && firtLineHeader) || String.IsNullOrEmpty(lines[i])) continue;
                    DataRow datarow = _datatable.NewRow();
                    datarow.ItemArray = lines[i].Split(delimiter);
                    _datatable.Rows.Add(datarow);
                }
            }
            catch (ArgumentException) 
            {
                throw new Exception("File is not correct format! Please check the values in columns.");
            }

            return _datatable;
        }
    }
}