using CTP.core;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
                //MessageBox.Show(ColViewModelInstance.GetItemListName_Debug().ToString(), "Liczba", MessageBoxButton.OK, MessageBoxImage.Error);

                string FilePath = FilePicker.GetFilePath();

                if (FilePath == "NullPath") return;

                string FileContentRaw = FilePicker.GetFileContent(FilePath);

                data.SetDataTable(DataScaler.ScaleData(Parser.Parse(FileContentRaw)));

                try
                {
                    ColViewModelInstance.SwapData(data);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ItemLoadError", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                RawChart.AllValues = data.GetValues(1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            }

        public void GraphsDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 1;

            XChart.AllValues = data.GetDistanceValues(1);

            VelocityChart.AllValues = data.GetVelocityValues(1);

            AccelerationChart.AllValues = data.GetAccelerationValues(1);
            //NumberOfColumns = data.ColumnsCount();

            /*Trace.WriteLine(String.Join(", ", _timeValues));*/
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void FinishSensorConfiguration_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 2;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}