using System.Collections.Generic;

namespace Library.WebRequest.Model
{
    public class ModelSession<T>
    {
        public T Session { get; set; }

        public object OutrasInfos { get; set; } = new object();

        public List<string> ResponsesAdicionais { get; set; } = new List<string>();

        public string CaptchaCodificada { get; set; } = string.Empty;

        public string CaptchaResolvida { get; set; } = string.Empty;
    }
}
