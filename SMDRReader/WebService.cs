using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMDRReader
{
    public class WebService
    {
        public string Url { get; }
        public string Key { get; }
        public FileService FileService = new FileService(Util.GetSettings().LogPath, "ErrorLog_{0}.log");
        public WebService(string url, string key)
        {
            Url = url;
            Key = key;
            //used in didar CRM
            Url = Url.Replace("{{CallerIdKey}}", Key);
        }

        public async Task<string> GetRequest(string data)
        {
            try
            {
                WebRequest request = WebRequest.Create(Url);
                WebResponse response = await request.GetResponseAsync();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                var msg = $"error[{DateTime.Now.ToString("hh:mm")}]:{ex.Message}";
                Console.WriteLine(msg);
                await FileService.Write(msg);
                return "";
            }

        }
    }
}
