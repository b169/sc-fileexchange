using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Foundation.SitecoreFileExchange.Models
{
    [DataContract]
    public class RemoteFileExchangeServiceArgs : EventArgs
    {
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public string Database { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public Guid FileId { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string CustomData { get; set; }
    }
}