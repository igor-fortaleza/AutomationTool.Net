using Library.WebRequest;
using System;

namespace Library.TwoCaptcha
{
    public class TwoCaptchaBase
    {
        public const string UrlBase = "http://2captcha.com/";

        public Request GetRequest()
        {
            return new Request(new ModelWebRequest("http://2captcha.com/"));
        }
    }
}
