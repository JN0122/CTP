using System.Data;
using System.Diagnostics;

namespace CTP.core
{
    class Parser
    {
        private readonly DataTable _datatable;

        public Parser(string fileContent)
        {
            List<string> lines = new(fileContent.Replace(',', '.').Split("\r\n"));
            char[] delimiter = { '\t', ';' };
            _datatable = new DataTable();
            List<string> columnHeaders = new(lines[0].Split(delimiter));
            bool firtLineHeader = !IsDouble(columnHeaders[0]);

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                if (firtLineHeader) _datatable.Columns.Add(columnHeaders[i], typeof(double));
                else _datatable.Columns.Add($"Column{i}", typeof(double));
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0 && firtLineHeader) continue;
                DataRow datarow = _datatable.NewRow();
                datarow.ItemArray = lines[i].Split(delimiter);
                _datatable.Rows.Add(datarow);
            }
        }

        private static bool IsDouble(string field)
        {
            try
            {
                double.Parse(field);
                return true;

            }
            catch (FormatException)
            {
                return false;
            }
        }

        public List<double> GetColumnValues(int columnIndex)
        {
            List<double> values = new();
            foreach (DataRow row in _datatable.Rows)
            {
                values.Add((double)row[columnIndex]);
            }
            return values;
        }

        public DataTable GetDataTable()
        {
            return _datatable;
        }
    }
}
