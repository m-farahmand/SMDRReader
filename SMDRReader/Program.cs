using System;

namespace SMDRReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var capture = new SMDRCapture("192.168.1.101", 2300, 168,(info)=> {
                Console.WriteLine($"Sent data:{info.extension},{info.number}");
            });
            capture.Capture();
            Console.ReadKey();
        }
    }
}
