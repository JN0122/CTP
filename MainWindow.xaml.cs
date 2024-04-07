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
using System.IO;

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

        //Po kliknieciu przycisku file picker od MC zwraca nam sciezke do pliku
        private void FilePickerButton_Click(object sender, RoutedEventArgs e)
        {

            var file_reader_inst = new FileReader();

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "NULL_PATH"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true && dialog.FileName != "NULL_PATH")
            {
                // Open document
                string filename = dialog.FileName;

                FilePickerButton.Content = "Wybierz nowy plik";

                FilePathDisplay.Content = filename;

                FileContentDisplay.Content = file_reader_inst.GetFileContent(filename);
            }
        }
    }
}