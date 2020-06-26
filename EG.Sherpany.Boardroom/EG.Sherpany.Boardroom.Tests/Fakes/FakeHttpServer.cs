using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace EG.Sherpany.Boardroom.Tests.Fakes
{
    class FakeHttpServer : IDisposable
    {
        private const uint BufferSize = 8192;

        private readonly StreamSocketListener _listener;

        public FakeHttpServer(int port, string response)
        {
            Response = response;
            this._listener = new StreamSocketListener();
            this._listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
            this._listener.BindServiceNameAsync(port.ToString());
        }

        public void Dispose()
        {
            this._listener.Dispose();
        }

        public string Response { get; private set; }
        public string Request { get; private set; }
        public string FullResponse { get; private set; }

        private async void ProcessRequestAsync(StreamSocket socket)
        {
            // this works for text only
            StringBuilder request = new StringBuilder();
            using (IInputStream input = socket.InputStream)
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }
            Request = request.ToString();
            using (IOutputStream output = socket.OutputStream)
            {
                string requestMethod = Request.Split('\n')[0];
                string[] requestParts = requestMethod.Split(' ');
                if (requestParts[0] == "POST" && Request.ToLowerInvariant().Contains("content-length: 0"))
                    Response = null;
                await WriteResponseAsync(requestParts[1], output);
            }
        }

        private async Task WriteResponseAsync(string path, IOutputStream os)
        {
            using (Stream resp = os.AsStreamForWrite())
            {
                if (Response != null)
                {
                    string header = "HTTP/1.1 200 OK\r\n" + $"Content-Length: {Response.Length}\r\n" +
                                        "Connection: close\r\n\r\n";
                    byte[] headerArray = Encoding.UTF8.GetBytes(header);
                    await resp.WriteAsync(headerArray, 0, headerArray.Length);

                    byte[] responseArray = Encoding.UTF8.GetBytes(Response);
                    await resp.WriteAsync(responseArray, 0, responseArray.Length);
                    FullResponse = Encoding.UTF8.GetString(headerArray, 0, headerArray.Length) +
                                   Encoding.UTF8.GetString(responseArray, 0, responseArray.Length);
                }
                else
                {
                    byte[] headerArray = Encoding.UTF8.GetBytes(
                                          "HTTP/1.1 404 Not Found\r\n" +
                                          "Content-Length:0\r\n" +
                                          "Connection: close\r\n\r\n");
                    await resp.WriteAsync(headerArray, 0, headerArray.Length);
                    FullResponse = Encoding.UTF8.GetString(headerArray, 0, headerArray.Length);
                }
                await resp.FlushAsync();
                
            }
        }
    }
}
