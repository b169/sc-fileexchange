using Foundation.SitecoreFileExchange;
using Foundation.SitecoreFileExchange.Models;
using Foundation.SitecoreFileExchange.Services;
using log4net;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.IO;
using System.Web.Hosting;

namespace Foundation.SitecoreFileExchange.Events
{
    public class RemoteFileExchangeEvents
    {     
        private readonly ILog _log;
        private readonly IRemoteFileExchangeService _fileService;
        
        public RemoteFileExchangeEvents()
        {
            _log = LoggerFactory.GetLogger(typeof(RemoteFileExchangeEvents));
            _fileService = (IRemoteFileExchangeService)ServiceLocator.ServiceProvider.GetService(typeof(IRemoteFileExchangeService));
        }
        public void HandleFileUploadRemote(object sender, EventArgs args)
        {
            var entryArgs = Event.ExtractParameter(args, 0) as RemoteFileExchangeServiceArgs;
            Event.RaiseEvent(Constants.Events.FileUploadedLocalEventName, entryArgs);
        }

        public void HandleFileUpload(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 0) as RemoteFileExchangeServiceArgs;
            Assert.IsNotNull(eventArgs, "Remote File Service argument is null");
            var stream = _fileService.Get(eventArgs.FileId);
            if(stream != null)
            {                
                if(!string.IsNullOrEmpty(Settings.LocalTempFolder))
                {
                    var localPath = Path.Combine(HostingEnvironment.MapPath(Settings.LocalTempFolder), $"{Guid.NewGuid()}-{eventArgs.FileName}");
                    using(var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fs);
                    }

                    stream.Dispose();
                    var localArgs = new RemoteFileExchangeFinishedEventArgs
                    {
                        CustomData = eventArgs.CustomData,
                        Database = eventArgs.Database,
                        SiteName = eventArgs.SiteName,
                        FileName = eventArgs.FileName,
                        Key = eventArgs.Key,
                        PathToFile = localPath
                    };

                    Event.RaiseEvent(Constants.Events.FileUploadFinishedLocalEventName, localArgs);
                    if (Settings.RemoveBlobWhenUploaded)
                    {
                        _fileService.Remove(eventArgs.FileId);
                    }

                    return;
                }

                _log.Warn($"Temp folder setting was not found or empty -{Constants.Settings.TempFolderSettingsKey}");
            }
        }

        public void HandleFileUploadFinished(object sender, EventArgs args)
        {
            var eventArgs = Event.ExtractParameter(args, 0) as RemoteFileExchangeFinishedEventArgs;
            Assert.IsNotNull(eventArgs, "RemoteFileUploadFinishedEvent argument is null");            
            if (Settings.RemoveFileWhenUploadFinished)
            {
                _log.Info($"Removing file {eventArgs.PathToFile} as configured in {Constants.Settings.RemoveFileWhenUploadFinishedSettingsKey} setting.");                                
                File.Delete(eventArgs.PathToFile);
            }
        }
    }
}