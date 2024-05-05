using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CTP.core
{
    internal class ColumnViewModel
    {
        private IList<Column> _ColumnsList;

        public ColumnViewModel(List<Column> CList)
        {
            _ColumnsList = CList;

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
            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public event EventHandler? CanExecuteChanged;

            public void Execute(object? parameter)
            {
                // Code implementation for execution
            }
        }

    }
}
