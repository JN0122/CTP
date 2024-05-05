using CTP.core;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace CTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    WpfMVVMSample.MainWindow window = new MainWindow();
        //    UserViewModel VM = new UserViewModel();
        //    window.DataContext = VM;
        //    window.Show();
        //}

        public int NumberOfColumns;

        public void FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = FilePicker.GetFilePath();
            string FileContentRaw = FilePicker.GetFileContent(FilePath);
            
            Measurements data = Measurements.GetInstance();
            data.SetDataTable(DataScaler.ScaleData(Parser.Parse(FileContentRaw)));

            NumberOfColumns = data.ColumnsCount();

            /*Trace.WriteLine(String.Join(", ", _timeValues));*/
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}