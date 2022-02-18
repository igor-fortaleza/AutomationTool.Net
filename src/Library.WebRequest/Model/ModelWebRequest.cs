using System.Net;
using System.Net.Cache;

namespace Library.WebRequest
{
    public class ModelWebRequest
    {
        public ModelWebRequest(string urlBase)
        {
            this.UrlBase = urlBase;
        }

        public string UrlBase { get; set; }

        public string UserAgent { get; set; } = string.Empty;

        public string Accept { get; set; } = string.Empty;

        public string Expect { get; set; } = string.Empty;

        public string ContentType { get; set; } = "application/json";

        public int TimeOut { get; set; } = 120000;

        public bool KeepAlive { get; set; }

        public bool AllowAutoRedirect { get; set; }

        public WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();

        public DecompressionMethods AutomaticDecompression { get; set; }

        public RequestCachePolicy CachePolicy { get; set; } = new RequestCachePolicy();
    }
}
