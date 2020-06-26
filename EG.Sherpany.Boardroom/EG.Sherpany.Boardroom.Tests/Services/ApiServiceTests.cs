using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EG.Sherpany.Boardroom.Enums;
using EG.Sherpany.Boardroom.Interfaces;
using EG.Sherpany.Boardroom.Model;
using EG.Sherpany.Boardroom.Services;
using EG.Sherpany.Boardroom.Tests.Fakes;
using EG.Sherpany.Boardroom.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace EG.Sherpany.Boardroom.Tests.Services
{
    [TestClass]
    public class ApiServiceTests
    {
        private const string LoginApi = "/api/v2/user/login/";
        private const string MtanLoginApi = "/api/v2/user/mtan_login/";
        private const string MtanResendApi = "/api/v2/user/mtan_resend/";
        private const string RoomApi = "/api/v2/room/?";
        private const string DocumentApi = "/api/v2/document?";
        private const string MeetingApi = "/api/v2/meeting?";
        private const string LabelApi = "/api/v2/label?";
        private const string GroupApi = "/api/v2/group?";
        private const string PrivateDocumentApi = "/api/v2/private_document?";
        private const string UserApi = "/api/v2/user/";
        //private readonly Uri apiService.ServerUrl = new Uri("https://board.sherpany.com/");
        //private readonly Uri apiService.ServerUrl = new Uri("https://boardv2-preprod.sherpany.com/");
        //private readonly Uri apiService.ServerUrl = new Uri("https://board-preprod.sherpany.com/");

        public class FakeCredentialManager : ICredentialManager
        {
            public async Task<SessionCredentials> GetSessionCredentials(CancellationTokenSource tokenSource = null, bool loginRequired = false,
                bool mTanRequired = false)
            {
                return GetCredentials();
            }

            public SessionCredentials GetStoredSessionCredentials()
            {
                return GetCredentials();
            }

            public void SetSessionCredentialsInAppSettings(SessionCredentials credentials)
            {

            }

            public async Task<bool> FullAuthenticationTask()
            {
                return true;
            }

            public async Task<bool> MTanAuthenticationTask()
            {
                return true;
            }
        }

        #region Login
        [TestMethod]
        public async Task LoginAsyncCallsPostAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.LoginAsync("username", "password");
            var content = "{\r\n  \"username\": \"username\",\r\n  \"password\": \"password\"\r\n}";
            var uri = new Uri(apiService.ServerUrl, LoginApi);
            var expected = new JObject
            {
                ["url"] = uri.AbsolutePath,
                ["content"] = content
            }.ToString();

            Assert.AreEqual(expected, dataProvider.Parameters);
        }

        [TestMethod]
        public async Task LoginAsyncWithNoResultReturnsError()
        {
            var dataProvider = new FakeDataProvider();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginAsync("username", "password");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task LoginAsyncWithBlankResultReturnsError()
        {
            var dataProvider = new FakeDataProvider {Result = ""};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginAsync("username", "password");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task LoginAsyncWithSuccessReturnsLoginResultSuccess()
        {
            var dataProvider = new FakeDataProvider {Result = "{\"success\": true}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginAsync("username", "password");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task LoginAsyncWithSuccessReturnsLoginResultStep1()
        {
            var dataProvider = new FakeDataProvider {Result = "{\"success\": true}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginAsync("username", "password");

            Assert.AreEqual(1, result.LoginStep);
        }
        #endregion
        
        #region LoginMTan

        [TestMethod]
        public async Task LoginMtanAsyncWithNoResultReturnsError()
        {
            var dataProvider = new FakeDataProvider();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task LoginMtanAsyncWithBlankResultReturnsError()
        {
            var dataProvider = new FakeDataProvider {Result = ""};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task LoginMtanAsyncWithSuccessReturnsLoginResultSuccess()
        {
            var dataProvider = new FakeDataProvider {Result = "{\"success\": true}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task LoginMtanAsyncWithSuccessReturnsLoginResultStep2()
        {
            var dataProvider = new FakeDataProvider {Result = "{\"success\": true}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");

            Assert.AreEqual(2, result.LoginStep);
        }

        [TestMethod]
        public async Task LoginMtanAsyncWithSuccessReturnsLoginResultStep2WithError()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");

            Assert.AreEqual(2, result.LoginStep);
        }

        [TestMethod]
        public async Task LoginMtanAsyncWithSuccessReturnsLoginResultStep1()
        {
            var dataProvider = new FakeDataProvider { Result = "{\r\n\"success\": false,\r\n \"error_code\":\"login-required\"\r\n}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.LoginMTanAsync("code");

            Assert.AreEqual(1, result.LoginStep);
        }
        #endregion

        #region GetRooms
        [TestMethod]
        public async Task GetRoomsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider {Result = "{\"success\": true}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRoomsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider {Result = null};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRoomsAsyncWithBlankReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider {Result = null};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRoomsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider {Result = ""};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetRoomsAsyncWithWrongJsonReturnNotSuccess()
        {
            var dataProvider = new FakeDataProvider {Result = "{success:false}"};
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public async Task GetRoomsAsyncWithCredentialsAndWithNoRoomsReturnsNotSuccess()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRoomsAsyncWithCredentialsAndWithRoomsReturnsRooms()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":" +
                    "[{\"about\":null,\"documents\":null,\"flag_assistant_role_visible\":false,\"groups\":null,\"id\":0," +
                    "\"labels\":null,\"logo\":null,\"logo_updated_at\":null,\"meetings\":null,\"members\":null,\"name\":\"room1\"," +
                    "\"resource_uri\":null,\"slug\":null,\"private_documents\":null,\"private_documents_enabled\":false}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetRoomsAsync();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("room1", result[0].Name);
        }

        [TestMethod]
        public async Task GetRoomsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetRoomsAsync();
            var uri = new Uri(apiService.ServerUrl, RoomApi + "api_key=apiToken&username=userName");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = null
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion

        public static SessionCredentials GetCredentials() => new SessionCredentials("userName", "apiToken");
        
        #region GetMeetings
        [TestMethod]
        public async Task GetAllMeetingsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllMeetingsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.IsNull(result);
        }

        
        [TestMethod]
        public async Task GetAllMeetingsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllMeetingsAsyncWithWrongJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllMeetingsAsyncWithCredentialsAndWithNoMeetingsReturnsNull()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllMeetingsAsyncWithCredentialsAndWithMeetingsReturnsMeetings()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0}," +
                    "\"objects\":[{\"agenda_items\":null,\"all_boardbooks_generated\":false,\"cover_page\":null," +
                    "\"description\":null,\"end\":null,\"id\":0,\"is_admin_visible_only\":false,\"is_published\":false," +
                    "\"location\":null,\"modified\":null,\"publishing_count\":0,\"resource_uri\":null,\"room\":null," +
                    "\"room_id\":0,\"start\":null,\"title\":\"meeting1\"}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllMeetingsAsync("1", "slug");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("meeting1",result[0].Name);
        }

        [TestMethod]
        public async Task GetAllMeetingsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetAllMeetingsAsync("1", "slug");
            var uri = new Uri(apiService.ServerUrl, MeetingApi + "api_key=apiToken&username=userName&limit=10000&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion
        
        #region GetDocuments
        [TestMethod]
        public async Task GetAllDocumentsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync( "1",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllDocumentsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllDocumentsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllDocumentsAsyncWithWrongJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllDocumentsAsyncWithCredentialsAndWithNoMeetingsReturnsNull()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllDocumentsAsyncWithCredentialsAndWithDocumentsReturnsDocuments()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0}," +
                    "\"objects\":[{\"annotated_doc_id\":null,\"annotated_doc_uploaded_at\":null," +
                    "\"caption\":\"document1\",\"created\":null,\"description\":null,\"file\":null," +
                    "\"group_ids\":null,\"id\":0,\"label_ids\":null,\"last_file_updated\":null,\"mime_type\":null," +
                    "\"modified\":null,\"orig_creation_date\":null,\"page_count\":null,\"resource_uri\":null," +
                    "\"room\":null,\"room_id\":0,\"size\":0,\"type\":null,\"doc_type\":null,\"revision\":null}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllDocumentsAsync("1", "slug");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("document1",result[0].Name);
        }

        [TestMethod]
        public async Task GetAllDocumentsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetAllDocumentsAsync("1", "slug");
            var uri = new Uri(apiService.ServerUrl, DocumentApi + "api_key=apiToken&username=userName&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion
        
        #region GetLogo
        [TestMethod]
        public async Task GetLogoAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetLogoAsync( "api/v2/room/1/logo/",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetLogoAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetLogoAsync("api/v2/room/1/logo/", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetLogoAsyncWithBlankStringReturnNoBytes()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetLogoAsync("api/v2/room/1/logo/", "slug");
            Assert.AreEqual(0,result.Length);
        }


        [TestMethod]
        public async Task GetLogoAsyncWithWrongStringReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetLogoAsync("api/v2/room/1/logo/", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetLogoAsyncWithCredentialsWithLogoReturnsLogo()
        {
            var dataProvider = new FakeDataProvider
            {
                Result = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQ" +
                         "UAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAAM" +
                         "SURBVBhXY/jPZAoAAzoBN+HYLrYAAAAASUVORK5CYII="
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetLogoAsync("api/v2/room/1/logo/", "slug");
            Assert.AreEqual(155, result.Length);
            // PNG Signature
            var expected = new List<byte> {0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a};
            CollectionAssert.AreEqual(expected,result.Take(6).ToList());
        }

        [TestMethod]
        public async Task GetLogoAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var logoUrl = "api/v2/room/1/logo/";
            await apiService.GetLogoAsync(logoUrl, "slug");
            var uri = new Uri(apiService.ServerUrl, logoUrl + "?api_key=apiToken&username=userName");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion
        
        #region GetLabels
        [TestMethod]
        public async Task GetAllLabelsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync( "1",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllLabelsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllLabelsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllLabelsAsyncWithWrongJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllLabelsAsyncWithCredentialsAndWithNoLabelsReturnsNull()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllLabelsAsyncWithCredentialsAndWithDocumentsReturnsDocuments()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0}," +
                    "\"objects\":[{\"created\":null,\"doc_ids\":null,\"id\":0,\"modified\":null,\"name\":\"label1\"," +
                    "\"order\":0,\"quick_access\":false,\"resource_uri\":null,\"room\":null,\"room_id\":0,\"usage\":0}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllLabelsAsync("1", "slug");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("label1", result[0].Name);
        }

        [TestMethod]
        public async Task GetAllLabelsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetAllLabelsAsync("1", "slug");
            var uri = new Uri(apiService.ServerUrl, LabelApi + "api_key=apiToken&username=userName&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion

        #region GetGroups
        [TestMethod]
        public async Task GetAllGroupsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllGroupsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllGroupsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllGroupsAsyncWithWrongJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllGroupsAsyncWithCredentialsAndWithNoLabelsReturnsNull()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllGroupsAsyncWithCredentialsAndWithDocumentsReturnsDocuments()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0}," +
                    "\"objects\":[{\"doc_ids\":null,\"group_color\":null,\"id\":0,\"meeting_ids\":null,\"member_ids\":null," +
                    "\"resource_uri\":null,\"room\":null,\"room_id\":0,\"title\":\"group1\"}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllGroupsAsync("1", "slug");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("group1", result[0].Name);
        }

        [TestMethod]
        public async Task GetAllGroupsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetAllGroupsAsync("1", "slug");
            var uri = new Uri(apiService.ServerUrl, GroupApi + "api_key=apiToken&username=userName&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion

        #region DownloadDocument
        [TestMethod]
        public async Task DownloadDocumentAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadDocumentAsync("api/v2/document/1/download/", "slug", "docType");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadDocumentAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadDocumentAsync("api/v2/document/1/download/", "slug", "docType");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task DownloadDocumentAsyncWithBlankStringReturnNoBytes()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadDocumentAsync("api/v2/document/1/download/", "slug", "docType");
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task DownloadDocumentAsyncWithWrongStringReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadDocumentAsync("api/v2/document/1/download/", "slug", "docType");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadDocumentAsyncWithCredentialsWithLogoReturnsLogo()
        {
            var dataProvider = new FakeDataProvider
            {
                Result = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQ" +
                         "UAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAAM" +
                         "SURBVBhXY/jPZAoAAzoBN+HYLrYAAAAASUVORK5CYII="
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadDocumentAsync("api/v2/document/1/download/", "slug", "docType");
            Assert.AreEqual(155, result.Length);
            // PNG Signature
            var expected = new List<byte> { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a };
            CollectionAssert.AreEqual(expected, result.Take(6).ToList());
        }

        [TestMethod]
        public async Task DownloadDocumentAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService()) {LogManager = logManager};
            var docUrl = "api/v2/document/1/download/";
            await apiService.DownloadDocumentAsync(docUrl, "slug", "docType");
            var uri = new Uri(apiService.ServerUrl, docUrl + "?api_key=apiToken&username=userName&revision=0&doc_type=docType");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion


        #region DownloadManagedDocument
        [TestMethod]
        public async Task DownloadManagedDocumentAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadManagedDocumentAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task DownloadManagedDocumentAsyncWithBlankStringReturnNoBytes()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task DownloadManagedDocumentAsyncWithWrongStringReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadManagedDocumentAsyncWithCredentialsWithLogoReturnsLogo()
        {
            var dataProvider = new FakeDataProvider
            {
                Result = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQ" +
                         "UAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAAM" +
                         "SURBVBhXY/jPZAoAAzoBN+HYLrYAAAAASUVORK5CYII="
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            Assert.AreEqual(155, result.Length);
            // PNG Signature
            var expected = new List<byte> { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a };
            CollectionAssert.AreEqual(expected, result.Take(6).ToList());
        }

        [TestMethod]
        public async Task DownloadManagedDocumentAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var docUrl = "api/v2/document/1/download/";
            await apiService.ManagerDownloadStandardDocumentAsync(new DownloadViewModel(new DocumentViewModel(new Document() { Id = "1" }, "slug") { DocType = "docType", }, true, null));
            var uri = new Uri(apiService.ServerUrl, docUrl + "?api_key=apiToken&username=userName&revision=0&doc_type=docType");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion


        #region GetPrivateDocuments
        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithWrongJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithCredentialsAndWithNoMeetingsReturnsNull()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0},\"objects\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncWithCredentialsAndWithDocumentsReturnsDocuments()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"meta\":{\"limit\":0,\"next\":null,\"offset\":0,\"previous\":null,\"total_count\":0}," +
                    "\"objects\":[{\"annotated_doc_id\":null,\"annotated_doc_uploaded_at\":null," +
                    "\"caption\":\"document1\",\"created\":null,\"description\":null,\"file\":null," +
                    "\"group_ids\":null,\"id\":0,\"label_ids\":null,\"last_file_updated\":null,\"mime_type\":null," +
                    "\"modified\":null,\"orig_creation_date\":null,\"page_count\":null,\"resource_uri\":null," +
                    "\"room\":null,\"room_id\":0,\"size\":0,\"type\":null,\"doc_type\":null,\"revision\":null}]}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetAllPrivateDocumentsAsync("1", "slug");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("document1", result[0].Name);
        }

        [TestMethod]
        public async Task GetAllPrivateDocumentsAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetAllPrivateDocumentsAsync("1",  "slug");
            var uri = new Uri(apiService.ServerUrl, PrivateDocumentApi + "api_key=apiToken&username=userName&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
                
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion

        #region UploadDocument
        [TestMethod]
        public async Task UploadDocumentsAsyncWithNullCredentialsReturnEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadDocumentAsync("", "1", "slug","docType",1);
            Assert.AreEqual("",result);
        }

        [TestMethod]
        public async Task UploadDocumentAsyncCallsPostAsyncWithPassedContent()
        {
            var dataProvider = new FakeDataProvider();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.UploadDocumentAsync("file", "1", "slug", "docType",1);
            var content = "{\"username\":\"userName\",\"pk\":1,\"revision\":1,\"doc_type\":\"docType\",\"api_key\":\"apiToken\",\"file\":\"file\"}";
            var uri = new Uri(apiService.ServerUrl, @"api/v2/annotated_document/upload/");
            var expected = new JObject
            {
                ["url"] = uri.AbsolutePath,
                ["content"] = content
            }.ToString();

            Assert.AreEqual(expected, dataProvider.Parameters);
        }

        [TestMethod]
        public async Task UploadDocumentAsyncWithNoResultReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadDocumentAsync("file", "1", "slug", "docType",1);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public async Task UploadDocumentAsyncWithBlankResultReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadDocumentAsync("file", "1", "slug", "docType",1);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public async Task UploadDocumentAsyncWithSuccessReturnsResource()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true, \"resource_uri\": \"/doc/1\"}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadDocumentAsync("file", "1", "slug", "docType",1);

            Assert.AreEqual("/doc/1", result);
        }

        [TestMethod]
        public async Task UploadDocumentAsyncWithNoSuccessReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadDocumentAsync("file", "1", "slug", "docType",1);

            Assert.AreEqual("", result);
        }
        #endregion

        #region UploadPrivateDocument
        [TestMethod]
        public async Task UploadPrivateDocumentsAsyncWithNullCredentialsReturnEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadPrivateDocumentAsync(null, DocumentState.create, null);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public async Task UploadPrivateDocumentAsyncCallsPostAsyncWithPassedContent()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.UploadPrivateDocumentAsync(null,DocumentState.create, "slug");
            var content = "{\r\n  \"action\": \"create\",\r\n  \"username\": \"userName\",\r\n  \"api_key\": \"apiToken\",\r\n  \"private_doc\": null\r\n}";
            var uri = new Uri(apiService.ServerUrl, @"api/v2/private_document/upload/");
            var expected = new JObject
            {
                ["url"] = uri.AbsolutePath,
                ["content"] = content
            }.ToString();

            Assert.AreEqual(expected, dataProvider.Parameters);
        }

        [TestMethod]
        public async Task UploadPrivateDocumentAsyncWithNoResultReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadPrivateDocumentAsync(null, DocumentState.create, "slug");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public async Task UploadPrivateDocumentAsyncWithBlankResultReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadPrivateDocumentAsync(null, DocumentState.create, "slug");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public async Task UploadPrivateDocumentAsyncWithSuccessReturnsResource()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true, \"status\": \"created\"}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadPrivateDocumentAsync(null, DocumentState.create, "slug");
            Assert.AreEqual("created", result);
        }

        [TestMethod]
        public async Task UploadPrivateDocumentAsyncWithNoSuccessReturnsEmptyString()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": false}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.UploadPrivateDocumentAsync(null, DocumentState.create, "slug");

            Assert.AreEqual("", result);
        }
        #endregion

        #region GetSingleUser

        [TestMethod]
        public async Task GetSingleUserAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetSingleUserAsync("1", "1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetSingleUserAsyncWithBlankJsonReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetSingleUserAsync("1", "1", "slug");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetSingleUserAsyncWithWrongJsonReturnEmptyUser()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetSingleUserAsync("1", "1", "slug");
            var expected = new Member {Id = "0"};
            Assert.AreEqual(JsonConvert.SerializeObject(expected),JsonConvert.SerializeObject(result));
        }

        
        [TestMethod]
        public async Task GetSingleUserAsyncWithCredentialsAndWithUserReturnsUser()
        {
            var dataProvider = new FakeDataProvider
            {
                Result =
                    "{\"address\":null,\"email\":null,\"first_name\":\"John\",\"group_ids\":null,\"id\":0," +
                    "\"last_name\":\"Doe\",\"password_counter\":0,\"phone\":null,\"preferred_language\":null," +
                    "\"profile_picture\":null,\"profile_picture_updated_at\":null,\"resource_uri\":null," +
                    "\"room_ids\":null,\"title\":null}"
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.GetSingleUserAsync("1", "1", "slug");
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        public async Task GetSingleUserAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.GetSingleUserAsync("1", "1", "slug");
            var uri = new Uri(apiService.ServerUrl, UserApi + "1/?api_key=apiToken&username=userName&room=1");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = "slug"
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion

        #region DownloadProfilePicture
        [TestMethod]
        public async Task DownloadProfilePictureAsyncWithNullCredentialsReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{\"success\": true}" };
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadProfilePictureAsyncWithNullReturnReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = null };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task DownloadProfilePictureAsyncWithBlankStringReturnNoBytes()
        {
            var dataProvider = new FakeDataProvider { Result = "" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task DownloadProfilePictureAsyncWithWrongStringReturnNull()
        {
            var dataProvider = new FakeDataProvider { Result = "{}" };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DownloadProfilePictureAsyncWithCredentialsWithLogoReturnsLogo()
        {
            var dataProvider = new FakeDataProvider
            {
                Result = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQ" +
                         "UAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAAM" +
                         "SURBVBhXY/jPZAoAAzoBN+HYLrYAAAAASUVORK5CYII="
            };
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            var result = await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            Assert.AreEqual(155, result.Length);
            // PNG Signature
            var expected = new List<byte> { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a };
            CollectionAssert.AreEqual(expected, result.Take(6).ToList());
        }

        [TestMethod]
        public async Task DownloadProfilePictureAsyncCallsGetJsonStringAsyncWithUsernameAndPassword()
        {
            var dataProvider = new FakeDataProvider();
            var logManager = new FakeLogManager();
            var apiService = new ApiService(dataProvider, new FakeCredentialManager(), new FakePushNotificationService());
            await apiService.DownloadProfilePictureAsync("1", "a@b.c");
            var uri = new Uri(apiService.ServerUrl, UserApi + "1/profile_picture/?api_key=apiToken&username=userName&email=a%40b.c");
            var expected = new JObject
            {
                ["url"] = uri.AbsoluteUri,
                ["slug"] = null
            }.ToString();
            Assert.AreEqual(expected, dataProvider.Parameters);
        }
        #endregion
    }
}