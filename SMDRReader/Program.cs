using System;

namespace SMDRReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = Util.GetSettings();
            var capture = new SMDRCapture(settings.ServerAddr, settings.ServerPort, settings.BufferSize, async (info) =>
             {
                 Console.WriteLine($"Sent data:{info.extension},{info.number}");
                 await new FileService(settings.LogPath, "CALLID_{0}.log").Write(info.rawData);
                 if (info.dir == ECallDir.input && info.type == ECallType.number && info.extension != "601")
                     Console.WriteLine($"Response: {await new WebService(settings.ApiEndPoint.Replace("{{number}}", info.number), Util.GetExtKey(info.extension)).GetRequest(info.number)}");
             });
            capture.Capture();
            Console.ReadKey();
        }

    }
}
