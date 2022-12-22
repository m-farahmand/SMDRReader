﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SMDRReader
{
    public class SMDRCapture
    {
        public int BufferSize { get; set; }
        public int Port { get; set; }
        public string ServerAddr { get; set; }
        public Action<SMDRStruct> CallBack { get; set; }
        public SMDRCapture(string serverAddr, int port, int bufferSize, Action<SMDRStruct> callback)
        {
            Port = port;
            ServerAddr = serverAddr;
            BufferSize = bufferSize;
            CallBack = callback;
        }

        public async Task Capture()
        {

            while (true)
            {
                var client = new TcpClient();
                // Buffer for reading data
                // Get a stream object for reading and writing

                try
                {
                    Console.Write("Waiting for a connection... ");
                    // Start listening for client requests.
                    client.Connect(ServerAddr, Port);
                    Console.WriteLine("Connected!");
                    NetworkStream stream = client.GetStream();

                    client.Client.SendString("SMDR\r\n", SocketFlags.Partial);
                    Console.WriteLine(stream.ReadString(BufferSize));

                    client.Client.SendString("PCCSMDR\r\n", SocketFlags.Partial);
                    Console.WriteLine(stream.ReadString(BufferSize));

                    int i;
                    var bytes = new Byte[BufferSize];

                    while ((i = stream.Read(bytes, 0, BufferSize)) != 0)
                    {
                        Parser(bytes);
                        Array.Clear(bytes, 0, bytes.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            }
        }
        public async Task Parser(byte[] bytes)
        {
            var lines = Encoding.ASCII.GetString(bytes).Split("\r\n".ToCharArray());
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                SMDRStruct info;
                if (IsDataValid(line))
                {
                    info.extension = line.Substring(19, 3);
                    info.number = ParsePhoneNumber(line.Substring(26, 26));
                    info.rawData = line;
                    await Task.Run(()=> CallBack(info));
                    Console.WriteLine("ok");
                }
            }
        }
        private bool IsDataValid(string data)
        {
            if (data?.Length < 52 || data[2] != '/' || data[5] != '/')
                return false;
            return true;
        }
        private string ParsePhoneNumber(string data) => int.TryParse(data, out int i) ? data : data?.Length > 3 ? data.Substring(3) : "0";
    }
    public static class ExMethods
    {
        public static void SendString(this Socket client, string data, SocketFlags flag)
        {
            var bData = Encoding.ASCII.GetBytes(data);
            client.Send(bData, bData.Length, flag);

        }
        public static string ReadString(this NetworkStream stream, int bufferSize)
        {
            var bytes = new Byte[bufferSize];
            var len = stream.Read(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(bytes, 0, len);
        }
    }
    public struct SMDRStruct
    {
        public string rawData;
        public string extension;
        public string number;
    }
}