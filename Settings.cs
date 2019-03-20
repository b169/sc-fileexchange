using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.SitecoreFileExchange
{
    public class Settings
    {
        public static string SharedDatabaseName => Sitecore.Configuration.Settings.GetSetting(Constants.Settings.SharedDatabaseSettingsKey);
        public static string LocalTempFolder => Sitecore.Configuration.Settings.GetSetting(Constants.Settings.TempFolderSettingsKey, string.Empty);
        public static bool RemoveFileWhenUploadFinished => Sitecore.Configuration.Settings.GetBoolSetting(Constants.Settings.RemoveFileWhenUploadFinishedSettingsKey, false);
        public static bool RemoveBlobWhenUploaded => Sitecore.Configuration.Settings.GetBoolSetting(Constants.Settings.RemoveBlobWhenUploadedSettingsKey, false);
        public static bool AddRemoteteEventToLocalQueue => Sitecore.Configuration.Settings.GetBoolSetting(Constants.Settings.AddRemoteEventToLocalQueueSettingsKey, false);
    }
}