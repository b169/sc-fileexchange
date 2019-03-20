using System;

namespace Foundation.SitecoreFileExchange.Models
{
    public class RemoteFileExchangeFinishedEventArgs : EventArgs
    {
        public string SiteName { get; set; }
        public string Database { get; set; }        
        public string FileName { get; set; }
        public string PathToFile{ get; set; }        
        public string Key { get; set; }
        public string CustomData { get; set; }
    }
}