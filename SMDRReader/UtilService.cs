using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDRReader
{
    static class Util
    {
        public static Properties.Settings GetSettings()
        {
            return Properties.Settings.Default;
        }

        public static string GetExtKey(string Ext)
        {
            foreach (var item in GetSettings().ExtKey)
            {
                if (item.StartsWith(Ext))
                    return item.Split('=')[1];//return second part of ExtKey 
            }
            Console.WriteLine("Use default key");
            return GetSettings().DefaultKey;
        }
    }
}
