using CTP.core;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.VisualElements;

using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO.Packaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

//nuget pls

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
                data.SetDataTable(DataScaler.ScaleData(Parser.Parse(FileContentRaw)));

                // Wywołanie metody DisplayLoadedFilePath z nazwą zaczytanego pliku
                DisplayLoadedFilePath(FilePath);
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


        /*
        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {
            //AllSensorsChart.ShowSensor("Sensor 1");

            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            // This is your window
            writer.Write(Content2);

            doc.Close();
            package.Close();

            // Convert 
            MemoryStream outStream = new MemoryStream();
            PdfSharp.Xps.XpsConverter.Convert(lMemoryStream, outStream, false);

            // Write pdf file - path 
            //FileStream fileStream = new FileStream("C:\\Users\\puech\\Documents\\ctp_proj\\test.pdf", FileMode.Create);
            //outStream.CopyTo(fileStream);


            // Display Save File Dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Save PDF File"
            };

            if (saveFileDialog.ShowDialog() == true) // Check if the user pressed 'Save'
            {
                // Write PDF to selected file path
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    outStream.Seek(0, SeekOrigin.Begin); // Ensure the stream position is at the beginning
                    outStream.CopyTo(fileStream);
                }

                // Show success message (optional)
                MessageBox.Show("PDF file saved successfully!");
            }


            // Clean up
            outStream.Flush();
            outStream.Close();
            //fileStream.Flush(); - do sztywnego
            //fileStream.Close(); - do sztywnego

        }
        */


        void DisplayLoadedFilePath(string filePath)
        {
            loaded_file.Text = $"Załadowano plik:   {System.IO.Path.GetFileName(filePath)}";
        }



        private void FilePDFButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Create a new PDF document
                PdfDocument pdfDocument = new PdfDocument();

                foreach (TabItem tabItem in myTabControl.Items)
                {
                    if (tabItem.Header.ToString() != "Import danych" && tabItem.Content is UIElement uiElement)
                    {
                        RenderUIElementToPdf(uiElement, pdfDocument);
                    }
                }

                // Display Save File Dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    Title = "Save PDF File"
                };

                if (saveFileDialog.ShowDialog() == true) // Check if the user pressed 'Save'
                {
                    // Save PDF to selected file path
                    pdfDocument.Save(saveFileDialog.FileName);

                    // Show success message (optional)
                    MessageBox.Show("PDF file saved successfully!");
                }

                pdfDocument.Close();
            }
            catch (Exception ex)
            {
                // Log the exception and show a message
                MessageBox.Show("An error occurred while saving the PDF: " + ex.Message);
            }
        }



        private void RenderUIElementToPdf(UIElement uiElement, PdfDocument pdfDocument)
        {

            try
            {
                // Measure and arrange the UIElement
                Size size = new Size(uiElement.RenderSize.Width, uiElement.RenderSize.Height);
                uiElement.Measure(size);
                uiElement.Arrange(new Rect(size));

                // Create a render target to draw the UIElement
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                renderBitmap.Render(uiElement);

                // Encode the render to a bitmap (PNG)
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ms.Position = 0;

                    string tempFileName = System.IO.Path.GetTempFileName();
                    File.WriteAllBytes(tempFileName, ms.ToArray());




                    // Create an XImage from the bitmap
                    XImage xImage = XImage.FromFile(tempFileName);
                    //XImage xImage = XImage.FromStream(() => ms);

                    // Create a new page in the PDF document
                    PdfPage pdfPage = pdfDocument.AddPage();
                    pdfPage.Width = xImage.PointWidth;
                    pdfPage.Height = xImage.PointHeight;

                    // Draw the XImage to the PDF page
                    XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
                    gfx.DrawImage(xImage, 0, 0);


                    File.Delete(tempFileName);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show a message
                Trace.WriteLine("An error occurred while rendering UIElement to PDF: " + ex.Message);
            }

        }


        private void FinishSensorConfiguration_Click(object sender, RoutedEventArgs e)
        {
            myTabControl.SelectedIndex = 2;
            AllSensorsChart.SetLabels(data.GetValues(0));

            SensorList1.Children.Clear();
            SensorList2.Children.Clear();
            AllSensorsChart.ClearAllSeries();
            XChart.ClearChart();
            VelocityChart.ClearChart();
            AccelerationChart.ClearChart();

            for (int i = 1; i < data.Table.Columns.Count; i++)
            {
                AddSensorToList("Sensor " + i, SensorList1);
                AddSensorToList("Sensor " + i, SensorList2);
                AllSensorsChart.SetSensorValues(i-1, data.GetValues(i), "Sensor " + i);
                XChart.SetSensorValues(i - 1, data.GetDistanceValues(i), "Sensor " + i);
                VelocityChart.SetSensorValues(i - 1, data.GetVelocityValues(i), "Sensor " + i);
                AccelerationChart.SetSensorValues(i - 1, data.GetAccelerationValues(i), "Sensor " + i);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string? Content = GetCheckboxContent(sender);

            if (Content == null) return;
            if(myTabControl.SelectedIndex == 3)
            {
                AllSensorsChart.ShowSensor(Content);
            }
            else
            {
                XChart.ShowSensor(Content);
                VelocityChart.ShowSensor(Content);
                AccelerationChart.ShowSensor(Content);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string? Content = GetCheckboxContent(sender);

            if (Content == null) return;

            if (myTabControl.SelectedIndex == 3)
            {
                AllSensorsChart.HideSensor(Content);
            }
            else
            {
                XChart.HideSensor(Content);
                VelocityChart.HideSensor(Content);
                AccelerationChart.HideSensor(Content);
            }
        }

        private static string? GetCheckboxContent(object sender)
        {
            if (sender.GetType() != typeof(CheckBox)) return null;
            CheckBox sensor_checkbox = (CheckBox)sender;

            return (string)sensor_checkbox.Content;
        }
    }
}