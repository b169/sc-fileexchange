using Foundation.SitecoreFileExchange.Models;
using System;
using System.IO;

namespace Foundation.SitecoreFileExchange.Services
{
    public interface IRemoteFileExchangeService
    {
        void Store(Stream stream, string fileName, FileExchangeEventData data);
        Stream Get(Guid fileId);
        void Remove(Guid fileId);
    }
}
