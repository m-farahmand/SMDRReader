using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMDRReader
{
    public class FileService
    {
        public string Path { get; }
        public FileService(string path)
        {
            Path = path;
        }

        public async Task<bool> Write(string data)
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                using (var stream = new StreamWriter(Path))
                {
                    await stream.WriteLineAsync(data);
                }
                Console.WriteLine("Log ok");
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

    }
}
