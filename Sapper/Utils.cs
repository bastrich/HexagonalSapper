using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Sapper
{
    class Utils
    {
        public static List<String> readTop()
        {
            try
            {
                List<string> top;
                FileStream fs = new FileStream("top.sapper", FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                top = (List<string>)bf.Deserialize(fs);
                fs.Close();
                return top;
            }
            catch (Exception)
            {
                return new List<string>(0);
            }
        }

        public static void writeTop(List<String> top)
        {
            try
            {
                FileStream fs = new FileStream("top.sapper", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, top);
                fs.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}
