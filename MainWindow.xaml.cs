using CTP.core;
using LineChart;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Parser parser = new(FileContentRaw);
            List<double> _timeValues = parser.GetColumnValues(0);
            List<double> _voltageValues = parser.GetColumnValues(1);

            var vm = (ViewModel)DataContext;
            vm._timeValues = _timeValues;
            vm._voltageValues = _voltageValues;

            Trace.WriteLine(String.Join(", ", _timeValues));
        }

    }
}