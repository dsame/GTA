using System;
using System.Net.Http;
using System.Threading.Tasks;
using EG.Sherpany.Boardroom.Interfaces;
using EG.Sherpany.Boardroom.Model;
using Newtonsoft.Json.Linq;

namespace EG.Sherpany.Boardroom.Tests.Fakes
{
    class FakeDataProvider : IDataProvider
    {
        public string Parameters { get; private set; }

        public FakeDataProvider()
        {
            Parameters = null;
        }

        public object Result { get; set; }

        public async Task<byte[]> GetJsonStreamAsync(SessionCredentials sessionCredentials, string url, string slug = null, bool isRetry = false)
        {
            Parameters = new JObject()
            {
                ["url"] = url,
                ["slug"] = slug
            }.ToString();
            var bytes = Convert.FromBase64String(Result.ToString());
            return bytes;
        }

        public async Task<string> GetJsonStringAsync(SessionCredentials sessionCredentials, string url, string slug = null)
        {
            Parameters = new JObject()
            {
                ["url"] = url,
                ["slug"] = slug
            }.ToString();
            return Result?.ToString();
        }

        public async Task<string> PostDataAsync(HttpContent content, Uri uri, string slug = null)
        {
            Parameters = new JObject()
            {
                ["url"] = uri.AbsolutePath,
                ["content"] = await content.ReadAsStringAsync()
            }.ToString();
            return Result?.ToString();
        }
    }
}
