using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    internal class FilePicker
    {

        ///<summary>
        ///Zwraca string ze sciezka lub "NullPath" jezeli
        /// sciezka nie zostala wybrana
        ///</summary>
        public static string GetFilePath()
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "NullPath"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            dialog.ShowDialog();

            return dialog.FileName;
        }

        ///<summary>
        ///Zwraca string z zawartoscia pliku, 
        ///jezeli zostal rzucony wyjatek to zwraca String.Empty
        ///</summary>
        public static string GetFileContent(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid path given to GetFileContent()");
            }

            string filename = path;
            char[] result;
            StringBuilder builder = new StringBuilder();

            try
            {
                using (StreamReader reader = File.OpenText(filename))
                {
                    result = new char[reader.BaseStream.Length];
                    //reader.ReadAsync(result, 0, (int)reader.BaseStream.Length);
                    reader.Read(result, 0, (int)reader.BaseStream.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not open or read the specified file");
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            foreach (char c in result)
            {
                //if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                //{
                builder.Append(c);
                //}
            }
            //FileOutput.Text = builder.ToString();
            //Console.WriteLine(builder.ToString());
            return builder.ToString();
        }
    }
}
