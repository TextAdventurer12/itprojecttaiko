// Code is a modified version of the WebRequest class of https://github.com/ppy/osu-framework/tree/master/osu.Framework/IO/Network

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace taikoclone.Network
{
    internal class WebRequest
    {
        public string Url;
        public int Timeout = 10000;
        public const int BUFFER_SIZE = 32768;

        private HttpResponseMessage response;
        public Stream ResponseStream { get; private set; }
        private int responseBytesRead;
        public bool Completed = false;


        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            Credentials = CredentialCache.DefaultCredentials,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        }
        )
        {
            Timeout = new System.TimeSpan(100000)
        };
        public WebRequest(string url, params object[] args)
        {
            if (!string.IsNullOrEmpty(url))
                Url = args.Length == 0 ? url : string.Format(url, args);
        }
        public void Perform()
        {
            ResponseStream = CreateOutputStream();
            internalPerform();
        }
        private async void internalPerform()
        {

            string url = Url;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            using (request)
            {
                var task = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, default);
                task.RunSynchronously();
                while (!task.IsCompleted)
                    Console.WriteLine(task.Status);
                response = task.Result;
                await beginResponse().ConfigureAwait(false);
            }
        }
        private async Task beginResponse()
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                while (true)
                {
                    int read = await responseStream.ReadAsync(buffer, 0, BUFFER_SIZE);
                    Console.WriteLine(read);
                    if (read > 0)
                    {
                        await ResponseStream.WriteAsync(buffer, 0, BUFFER_SIZE);
                        responseBytesRead += read;
                    }
                    else
                    {
                        ResponseStream.Seek(0, SeekOrigin.Begin);
                        Completed = true;
                        break;
                    }
                }
            }
        }
        protected virtual Stream CreateOutputStream() => new MemoryStream();    
    }
}
