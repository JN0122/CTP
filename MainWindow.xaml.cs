using CTP.core;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.VisualElements;

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                ColViewModelInstance.SwapData(data);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ItemLoadError", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            }

        public void GraphsDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 1;

            XChart.AllValues = data.GetValues(1);
            VelocityChart.AllValues = data.GetVelocityValues(1);
            AccelerationChart.AllValues = data.GetAccelerationValues(1);

            /*Trace.WriteLine(String.Join(", ", _timeValues));*/
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void AddSensorToList(string Content)
        {
            CheckBox sensor_checkbox = new()
            {
                Content = Content,
                IsChecked = true
            };
            sensor_checkbox.Checked += CheckBox_Checked;
            sensor_checkbox.Unchecked += CheckBox_Unchecked;
            Sensor_List1.Children.Add(sensor_checkbox);
        }

        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {
            AllSensorsChart.ShowSensor("Sensor 1");
        }
        
        private void FinishSensorConfiguration_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 2;
            AllSensorsChart.Labels = data.GetValues(0);

            Sensor_List1.Children.Clear();
            for (int i = 1; i < data.Table.Columns.Count; i++)
            {
                AddSensorToList("Sensor " + i);
                AllSensorsChart.SetSensorValues(i-1, data.GetValues(i), "Sensor " + i);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string? Content = GetCheckboxContent(sender);

            if (Content == null) return;

            AllSensorsChart.ShowSensor(Content);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string? Content = GetCheckboxContent(sender);

            if (Content == null) return;

            AllSensorsChart.HideSensor(Content);
        }

        private static string? GetCheckboxContent(object sender)
        {
            if (sender.GetType() != typeof(CheckBox)) return null;
            CheckBox sensor_checkbox = (CheckBox)sender;

            return (string)sensor_checkbox.Content;
        }
    }
}