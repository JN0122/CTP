using CTP.core;
using ViewModelsSamples.General.RealTime;
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

        private void FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = FilePicker.GetFilePath();
            string FileContentRaw = FilePicker.GetFileContent(FilePath);
            Parser parser = new(FileContentRaw);
            Trace.WriteLine(String.Join(", ", parser.GetColumnValues(1)));
        }

    }
}