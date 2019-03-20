namespace Foundation.SitecoreFileExchange.Models
{
    public class FileExchangeEventData
    {
        public string Key { get; set; }
        public string Data { get; set; }
        public bool RaiseEvent { get; set; }
    }
}