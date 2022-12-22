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

        public WebService(string url,string key)
        {
            Url = url;
            Key = key;
        }

        public async Task<string> SendRequest(string data)
        {
            WebRequest request = WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "text/x-gwt-rpc; charset=utf-8";
            request.Headers.Add("Authorization", $"Bearer{Key}");
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            Stream dataStream = await request.GetRequestStreamAsync();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return "";
        }
    }
}
