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

        private bool CzyOdleglosc = true;
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

        public void ConfigureSensorsButton_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 1;

            XChart.ClearChart();
            VelocityChart.ClearChart();
            AccelerationChart.ClearChart();

            XChart.AllValues = data.GetValues(1);
            VelocityChart.AllValues = data.GetVelocityValues(1);
            AccelerationChart.AllValues = data.GetAccelerationValues(1);

            /*Trace.WriteLine(String.Join(", ", _timeValues));*/
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void AddSensorToList(string Content, StackPanel SensorList, bool IsChecked = true)
        {
            CheckBox sensor_checkbox = new()
            {
                Content = Content,
                IsChecked = IsChecked
            };
            sensor_checkbox.Checked += CheckBox_Checked;
            sensor_checkbox.Unchecked += CheckBox_Unchecked;
            SensorList.Children.Add(sensor_checkbox);
        }

        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {
            AllSensorsChart.ShowSensor("Sensor 1");
        }
        
        private void FinishSensorConfiguration_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 2;
            AllSensorsChart.SetLabels(data.GetValues(0));

            SensorList1.Children.Clear();
            SensorList2.Children.Clear();
            AllSensorsChart.ClearAllSeries();
            for (int i = 1; i < data.Table.Columns.Count; i++)
            {
                AddSensorToList("Sensor " + i, SensorList1);
                AddSensorToList("Sensor " + i, SensorList2);
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

        private void RbRuch_Checked(object sender, RoutedEventArgs e)
        {
            this.CzyOdleglosc = true;
            MessageBox.Show(this.CzyOdleglosc.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RbPrObr_Checked(object sender, RoutedEventArgs e)
        {
            this.CzyOdleglosc = false;
            MessageBox.Show(this.CzyOdleglosc.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}