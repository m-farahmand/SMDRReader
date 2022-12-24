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
        public string FilePatternName { get; }

        public FileService(string path,string filePatternName)
        {
            Path = path;
            FilePatternName = filePatternName;
        }

        public async Task<bool> Write(string data)
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                using (var stream = new StreamWriter($"{Path}\\{string.Format(FilePatternName, GetDateISOString(DateTime.Now))}", true))
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
        public string GetDateISOString(DateTime date) => $"{date.Year}-{date.Month}-{date.Day}";
    }
}
