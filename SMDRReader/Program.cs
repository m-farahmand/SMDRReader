using System;

namespace SMDRReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var capture = new SMDRCapture(GetSettings().ServerAddr, GetSettings().ServerPort, GetSettings().BufferSize, async (info) =>
             {
                 Console.WriteLine($"Sent data:{info.extension},{info.number}");
                 await new FileService(GetSettings().LogPath).Write(info.rawData);
                 Console.WriteLine($"Response: {await new WebService(GetSettings().ApiEndPoint, GetExtKey(info.extension)).SendRequest(info.number)}");
             });
            capture.Capture();
            Console.ReadKey();
        }
        static Properties.Settings GetSettings() => Properties.Settings.Default;
        static string GetExtKey(string Ext)
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
