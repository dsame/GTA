using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Json;
using Windows.Networking.PushNotifications;

namespace EG.Sherpany.Boardroom.PushBackgroundTasks
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            RawNotification notification = (RawNotification)taskInstance.TriggerDetails;
            if (notification != null && JsonObject.TryParse(notification.Content, out JsonObject result))
            {
                var identifier = result.GetNamedObject("action")?.GetNamedString("identifier");
                if (identifier == "documents_available_for_download")
                {
                    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    var task = new Task(async () =>
                    {
                        await storageFolder.CreateFileAsync("NewDocumentFlag", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    });
                    task.RunSynchronously();
                }
            }
        }
    }
}