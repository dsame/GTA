using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EG.Sherpany.Boardroom.Model;
using EG.Sherpany.Boardroom.Persistance;
using EG.Sherpany.Boardroom.Services;
using EG.Sherpany.Boardroom.Tests.Fakes;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json.Linq;

namespace EG.Sherpany.Boardroom.Tests.Services
{
    [TestClass]
    public class HttpProviderTests
    {
        const int Port = 8000;
        const string ServerUrl = "http://localhost:8000";
        #region GetJsonString
        [TestMethod]
        public async Task GetJsonStringAsyncWithNulllCredentialsReturnNull()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port,"success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response =
                    await provider.GetJsonStringAsync(null, ServerUrl);
                Assert.IsNull(response);
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncWithNonNulllCredentialsReturnsValid()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl);
                Assert.AreEqual("success",response);
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncWithInvalidResultReturnsNull()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, null))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsNull(response);
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncFillsDateTime()
        {
            var fakeLogManager = new FakeLogManager();
            var appSettings = new AppSettings(null,null);
            appSettings.ApiKeyAccessDateTime = DateTime.MinValue;
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(appSettings, fakeLogManager);
                var response = await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl);
                Assert.AreNotEqual(appSettings.ApiKeyAccessDateTime,DateTime.MinValue);
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncFillsPlatformHeader()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsTrue(server.Request.Contains("platform: Win"));
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncDoesntFillRoomSlug()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsFalse(server.Request.Contains("room_slug"));
            }
        }

        [TestMethod]
        public async Task GetJsonStringAsyncWithSlugFillsRoomSlug()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStringAsync(GetSessionCredentials(), ServerUrl,"slug");
                Assert.IsTrue(server.Request.Contains("room_slug: slug"));
            }
        }
        #endregion

        #region GetJsonStream
        [TestMethod]
        public async Task GetJsonStreamAsyncWithNulllCredentialsReturnNull()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response =
                    await provider.GetJsonStreamAsync(null, ServerUrl);
                Assert.IsNull(response);
            }
        }


        [TestMethod]
        public async Task GetJsonStreamAsyncWithInvalidResultReturnsNull()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, null))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.GetJsonStreamAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsNull(response);
            }
        }

        [TestMethod]
        public async Task GetJsonStreamAsyncFillsDateTime()
        {
            var fakeLogManager = new FakeLogManager();
            var appSettings = new AppSettings(null, null) {ApiKeyAccessDateTime = DateTime.MinValue};
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(appSettings, fakeLogManager);
                var response = await provider.GetJsonStreamAsync(GetSessionCredentials(), ServerUrl);
                Assert.AreNotEqual(appSettings.ApiKeyAccessDateTime, DateTime.MinValue);
            }
        }

        [TestMethod]
        public async Task GetJsonStreamAsyncFillsPlatformHeader()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStreamAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsTrue(server.Request.Contains("platform: Win"));
            }
        }

        [TestMethod]
        public async Task GetJsonStreamAsyncDoesntFillRoomSlug()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStreamAsync(GetSessionCredentials(), ServerUrl);
                Assert.IsFalse(server.Request.Contains("room_slug"));
            }
        }

        [TestMethod]
        public async Task GetJsonStreamAsyncWithSlugFillsRoomSlug()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.GetJsonStreamAsync(GetSessionCredentials(), ServerUrl, "slug");
                Assert.IsTrue(server.Request.Contains("room_slug: slug"));
            }
        }
        #endregion

        #region PostData
        [TestMethod]
        public async Task PostDataAsyncWithNulllContentReturnNull()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.PostDataAsync(null, new Uri(ServerUrl));
                Assert.AreEqual("",response);
            }
        }

        [TestMethod]
        public async Task PostDataAsyncWithNonNulllContentReturnsValid()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.PostDataAsync(CreateHttpContent(), new Uri(ServerUrl));
                Assert.AreEqual("success", response);
            }
        }

        private static HttpContent CreateHttpContent()
        {
            var request = new JObject
            {
                ["username"] = "userName",
                ["password"] = "password"
            };

            HttpContent content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
            return content;
        }

        [TestMethod]
        public async Task PostDataAsyncWithInvalidResultReturnsBlankData()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, null))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                var response = await provider.PostDataAsync(CreateHttpContent(), new Uri(ServerUrl));
                Assert.AreEqual("", response);
            }
        }

        [TestMethod]
        public async Task PostDataAsyncFillsDateTime()
        {
            var fakeLogManager = new FakeLogManager();
            var appSettings = new AppSettings(null,null);
            appSettings.ApiKeyAccessDateTime = DateTime.MinValue;
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(appSettings, fakeLogManager);
                var response = await provider.PostDataAsync(CreateHttpContent(), new Uri(ServerUrl));
                Assert.AreNotEqual(appSettings.ApiKeyAccessDateTime, DateTime.MinValue);
            }
        }

        [TestMethod]
        public async Task PostDataAsyncFillsPlatformHeader()
        {
            var fakeLogManager = new FakeLogManager();
            var fakeAppSettingsManager = new AppSettings(null,null);
            using (FakeHttpServer server = new FakeHttpServer(Port, "success"))
            {
                var provider = new HttpProvider(fakeAppSettingsManager, fakeLogManager);
                await provider.PostDataAsync(CreateHttpContent(), new Uri(ServerUrl));
                Assert.IsTrue(server.Request.Contains("platform: Win"));
            }
        }
        #endregion


        private static SessionCredentials GetSessionCredentials()
        {
            return new SessionCredentials("userName","apiToken");
        }
    }
}
