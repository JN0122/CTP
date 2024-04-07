using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    internal class FileReader
    {
        public string GetFileContent(string path)
        {
            string filename = path;
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
