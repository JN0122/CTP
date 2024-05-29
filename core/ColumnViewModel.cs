using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CTP.core
{
    internal class ColumnViewModel: INotifyCollectionChanged
    {
        //private IList<Column> _measurement._ColumnsList { get; set; } = new List<Column>();

        public ColumnViewModel()
        {

        }

        private readonly Measurement _measurement = Measurement.GetInstance();

        public void AddColumns(Measurement dt) 
        {
            for (int i = 0; i < dt.Table.Columns.Count; i++)
            {
                Column col = new Column(dt.Table.Columns[i].ColumnName, "Napięciowy" , 0, 0, 0, 0);

                _measurement._ColumnsList.Add(col);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, col));
            }
        }
        public void ClearColumns()
        {
            foreach(Column element in _measurement._ColumnsList.ToList())
            {
                _measurement._ColumnsList.Remove(element);
                //CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, element));
            }
        }

        public int GetItemListName_Debug() 
        {
            return _measurement._ColumnsList.Count;
        }

        public List<Column> GetColumns()
        {
            return _measurement._ColumnsList.ToList();
        }

        public void SwapData(Measurement NewData)
        {
            ClearColumns();
            //CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            AddColumns(NewData);
        }


        //public IList<Column> Columns
        //{
        //    get { return _measurement._ColumnsList; }
        //    set { _measurement._ColumnsList = value; }
        //}

        public ObservableCollection<Column> Columns
        {
            get { return _measurement._ColumnsList; }
            set { _measurement._ColumnsList = value; }
        }

        private ICommand mUpdater;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

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
