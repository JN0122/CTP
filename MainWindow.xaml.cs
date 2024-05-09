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
        private readonly Measurement data = Measurement.GetInstance();
        public MainWindow()
        {
            InitializeComponent();
        }

        public void FilePickerButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string FilePath = FilePicker.GetFilePath();

                if (FilePath == "NullPath") return;

                string FileContentRaw = FilePicker.GetFileContent(FilePath);

                data.SetDataTable(Parser.Parse(FileContentRaw));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RawChart.AllValues = data.GetValues(1).Item1;
        }

        public void GraphsDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            XChart.AllValues = data.GetValues(1).Item1;

            VelocityChart.AllValues = data.GetValues(1).Item2;

            AccelerationChart.AllValues = data.GetValues(1).Item3;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}