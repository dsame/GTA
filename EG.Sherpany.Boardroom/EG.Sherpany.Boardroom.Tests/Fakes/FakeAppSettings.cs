using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.Sherpany.Boardroom.Interfaces;
using EG.Sherpany.Boardroom.Model;

namespace EG.Sherpany.Boardroom.Tests.Fakes
{
    class FakeAppSettings : IAppSettings
    {
        public string DeviceModel { get; set; }
        public string Manufacturer { get; set; }
        public string MachineName { get; set; }
        public string OperatingSystem { get; set; }
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Version { get; set; }
        public DateTime SuspensionTime { get; set; }
        public DateTime ApiKeyCreateDateTime { get; set; }
        public DateTime LastSyncDateTime { get; set; }
        public DateTime ApiKeyAccessDateTime { get; set; }
        public int LastRoom { get; set; }
        public int RemainingUnlockAttempts { get; set; }
        public int LastMeetingViewIndex { get; set; }
        public int DocumentsViewIndex { get; set; }
        public int MeetingsViewIndex { get; set; }
        public bool FirstStart { get; set; }
        public bool AutoSaveDocuments { get; set; }
        public bool IsAgendaItemDocumentsListView { get; set; }
        public bool PendingDocumentsToBeSynced { get; set; }
        public string Pen1Color { get; set; }
        public string Pen2Color { get; set; }
        public double Pen1Size { get; set; }
        public double Pen2Size { get; set; }
        public double Pen1Opacity { get; set; }
        public double Pen2Opacity { get; set; }
        public string AnnoColor { get; set; }
        public bool IsDemoMode { get; set; }
        public HashSet<AnnotationSettingsViewModel> AnnotationSettingsViewModels { get; set; }

        string IAppSettings.LastRoom
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool PushUrlToBeSynced { get; set; }
        public string PushUrl { get; set; }
        public bool AllowPushNotifications { get; set; }
    }
}
