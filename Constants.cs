using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.SitecoreFileExchange
{
    public struct Constants
    {
        public struct Events
        {
            public const string FileUploadedLocalEventName = "remotefile:uploaded";
            public const string FileUploadedRemoteEvent = FileUploadedLocalEventName+":remote";
            public const string FileUploadFinishedLocalEventName = "remotefile:uploadfinished";
        }

        public struct Settings
        {
            public const string SharedDatabaseSettingsKey = "Foundation.SitecoreFileExchange.SharedDatabaseName";
            public const string TempFolderSettingsKey = "Foundation.SitecoreFileExchange.TempFolder";
            public const string RemoveFileWhenUploadFinishedSettingsKey = "Foundation.SitecoreFileExchange.RemoveFileWhenUploadFinished";
            public const string RemoveBlobWhenUploadedSettingsKey = "Foundation.SitecoreFileExchange.RemoveBlobWhenUploaded";
            public const string AddRemoteEventToLocalQueueSettingsKey = "Foundation.SitecoreFileExchange.AddRemoteEventToLocalQueue";
        }
    }
}