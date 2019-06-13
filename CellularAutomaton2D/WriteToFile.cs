using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication5
{
    class WriteToFile
    {
        String path;



        public WriteToFile(String path)
        {
            this.path = path;
            File.WriteAllText(path, String.Empty);
        }

        public void SaveToFile(double var)
        {
            File.AppendAllText(path, var + Environment.NewLine);
        }

    }
}
