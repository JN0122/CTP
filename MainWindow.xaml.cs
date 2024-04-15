using CTP.core;
using System.Collections.ObjectModel;
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

        public void FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = FilePicker.GetFilePath();
            string FileContentRaw = FilePicker.GetFileContent(FilePath);

            Measurement data = Measurement.GetInstance();
            data.SetDataTable(Parser.Parse(FileContentRaw));

            /*Trace.WriteLine(String.Join(", ", _timeValues));*/
        }

    }
}