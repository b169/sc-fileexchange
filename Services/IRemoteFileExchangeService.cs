using Foundation.SitecoreFileExchange.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.SitecoreFileExchange.Services
{
    public interface IRemoteFileExchangeService
    {
        void Store(Stream stream, string fileName, FileExchangeEventData data);
        Stream Get(Guid fileId);
        void Remove(Guid fileId);
    }
}
