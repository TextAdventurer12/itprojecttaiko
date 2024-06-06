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
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            Credentials = CredentialCache.DefaultCredentials,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        }
        )
        {
            // Timeout is controlled manually through cancellation tokens because
            // HttpClient does not properly timeout while reading chunked data
            Timeout = System.Threading.Timeout.InfiniteTimeSpan
        };
        private HttpResponseMessage response;
        public WebRequest(string url, params object[] args)
        {
            if (!string.IsNullOrEmpty(url))
                Url = args.Length == 0 ? url : string.Format(url, args);
        }
        public void Perform()
        {
            internalPerform();
        }
        private async void internalPerform()
        {

            string url = Url;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            using (request)
            {
                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, default).ConfigureAwait(false);

            }
        }
    }
}
