using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CTP.core
{
    internal class ColumnViewModel
    {
        private IList<Column> _ColumnsList { get; set; } = new List<Column>();

        //public ColumnViewModel()
        //{

        //    _ColumnsList = new List<Column>();
        //    //List<Column> CList
        //    //_ColumnsList = CList;
        //    Column c1 = new Column("C1", "Type", 0, 1, 2, 3);
        //    Column c2 = new Column("C2", "Type", 0, 1, 2, 3);
        //    Column c3 = new Column("C3", "Type", 0, 1, 2, 3);
        //    Column c4 = new Column("C4", "Type", 0, 1, 2, 3);

        //    _ColumnsList.Add(c1);
        //    _ColumnsList.Add(c2);
        //    _ColumnsList.Add(c3);
        //    _ColumnsList.Add(c4);

        //}

        public ColumnViewModel()
        {

        }

        public void addColumns(Measurement dt) 
        {
            foreach (DataColumn column in dt.Table.Columns)
            {
                Column col = new Column(column.ColumnName, "init" , 0, 0, 0, 0);

                _ColumnsList.Add(col);
            }
        }

        public IList<Column> Columns
        {
            get { return _ColumnsList; }
            set { _ColumnsList = value; }
        }

        private ICommand mUpdater;

        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                // Code implementation for execution
            }
        }

    }
}
