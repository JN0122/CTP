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

                FileContentDisplay.Content = get_file_content(filename);

                //filePathDisplay.Content = filename;
            }
        }

        //Funkcja zwraca nam string z trescia pliku, akceptuje sciezke jako argument
        private string get_file_content(string s_file_path)
        {
            string filename = s_file_path;
            char[] result;
            StringBuilder builder = new StringBuilder();

            using (StreamReader reader = File.OpenText(filename))
            {
                result = new char[reader.BaseStream.Length];
                //reader.ReadAsync(result, 0, (int)reader.BaseStream.Length);
                reader.Read(result, 0, (int)reader.BaseStream.Length);
            }

            foreach (char c in result)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    builder.Append(c);
                }
            }
            //FileOutput.Text = builder.ToString();
            Console.WriteLine(builder.ToString());
            return builder.ToString();
        }
    }
}