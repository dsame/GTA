using System.Threading.Tasks;
using EG.Sherpany.Boardroom.Services;

namespace EG.Sherpany.Boardroom.Tests.Services
{
    internal class FakePushNotificationService : IPushNotificationService
    {
        public string ChannelUrl { get; }
        public bool PushUrlToBeSynced { get; set; }
        public Task<bool> CreatePushNotificationServiceAsync()
        {
            return new Task<bool>(()=>true);
        }

        public void Close()
        {
        }
    }
}