﻿using Foundation.SitecoreFileExchange.Models;
using log4net;
using Sitecore.Data;
using Sitecore.Data.Managers;
using System;
using System.IO;

namespace Foundation.SitecoreFileExchange.Services
{
    public class RemoteFileExchangeService : IRemoteFileExchangeService
    {
        private readonly ILog _log;

        public RemoteFileExchangeService()
        {
            _log = Sitecore.Diagnostics.LoggerFactory.GetLogger(typeof(RemoteFileExchangeService));
        }
        public void Store(Stream stream, string fileName, FileExchangeEventData data)
        {            
            var sharedDatabase = Database.GetDatabase(Settings.SharedDatabaseName);
            Guid blobId = Guid.NewGuid();
            ItemManager.SetBlobStream(stream, blobId, sharedDatabase);
            if (data.RaiseEvent)
            {                
                var args = new RemoteFileExchangeServiceArgs
                {
                    SiteName = Sitecore.Context.Site?.Name,
                    Database = Sitecore.Context.Database?.Name,
                    FileName = fileName,
                    Key = data.Key,
                    CustomData = data.Data,
                    FileId = blobId
                };

                Sitecore.Eventing.EventManager.QueueEvent(args, true, Settings.AddRemoteEventToLocalQueue);
            }
        }

        public Stream Get(Guid fileId)
        {
            try
            {                
                if (!string.IsNullOrEmpty(Settings.SharedDatabaseName))
                {
                    var sharedDatabase = Database.GetDatabase(Settings.SharedDatabaseName);
                    return ItemManager.GetBlobStream(fileId, sharedDatabase);                    
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error while getting blob stream", ex);
            }

            return null;
        }

        public void Remove(Guid fileId)
        {            
            if (!string.IsNullOrEmpty(Settings.SharedDatabaseName))
            {
                var sharedDatabase = Database.GetDatabase(Settings.SharedDatabaseName);
                ItemManager.RemoveBlobStream(fileId, sharedDatabase);
            }
        }
    }
}
