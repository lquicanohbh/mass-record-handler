using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Helpers
{
    public static class IOHelper
    {
        public static string[] ReadFile(string path)
        {
            try
            {
                string[] allText = File.ReadAllLines(path);
                return allText;
            }
            catch
            {
                throw new ArgumentNullException("The file path could not be located");
            }
        }
    }
}
