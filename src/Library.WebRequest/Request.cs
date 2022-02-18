using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using HtmlAgilityPack;
using Library.WebRequest.Model.Enum;
using Library.WebRequest.Tools;

namespace Library.WebRequest
{
    public class Request
    {
        private Request()
        {
        }

        public Request(ModelWebRequest modelPost)
        {
            this.RequestConstrutor(modelPost, (ModelWebRequest)null);
        }

        public Request(ModelWebRequest modelPost, ModelWebRequest modelGet)
        {
            this.RequestConstrutor(modelPost, modelGet);
        }

        private void RequestConstrutor(ModelWebRequest modelPost, ModelWebRequest modelGet)
        {
            this._ModelPost = modelPost;
            this._ModelGet = modelGet ?? modelPost;
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((_param1, _param2, _param3, _param4) => true);
        }

        public string Get()
        {
            return this.Get(string.Empty, TypeRequest.Get);
        }

        public string Get(string partUrl)
        {
            return this.Get(partUrl, TypeRequest.Get);
        }

        public string Get(TypeRequest tipo)
        {
            return this.Get(string.Empty, tipo);
        }

        public string Get(string partUrl, TypeRequest tipo)
        {
            this._TipoRequest = tipo;
            return this.ExecutarPostGet(partUrl);
        }

        public string Post()
        {
            return this.Post(string.Empty, string.Empty);
        }

        public string Post(string partUrl)
        {
            return this.Post(partUrl, string.Empty);
        }

        public string Post(MultipartFormDataContent form)
        {
            return this.Post(string.Empty, form);
        }

        public string Post(string partUrl, MultipartFormDataContent form)
        {
            this._MultiFormData = form.ReadAsByteArrayAsync().Result;
            return this.Post(partUrl, string.Empty);
        }

        public string Post(string partUrl, string parametros)
        {
            this._Parametros = parametros;
            this._TipoRequest = TypeRequest.Post;
            return this.ExecutarPostGet(partUrl);
        }

        //--------------------------------
        public string Put()
        {
            return this.Post(string.Empty, string.Empty);
        }

        public string Put(string partUrl)
        {
            return this.Put(partUrl, string.Empty);
        }

        public string Put(MultipartFormDataContent form)
        {
            return this.Put(string.Empty, form);
        }

        public string Put(string partUrl, MultipartFormDataContent form)
        {
            this._MultiFormData = form.ReadAsByteArrayAsync().Result;
            return this.Put(partUrl, string.Empty);
        }

        public string Put(string partUrl, string parametros)
        {
            this._Parametros = parametros;
            this._TipoRequest = TypeRequest.Put;
            return this.ExecutarPostGet(partUrl);           
        }

        //--------------------------------

        private string ExecutarPostGet(string partUrl)
        {
            this._ModelGet.UserAgent = this._ModelPost.UserAgent;
            try
            {
                this.CreateWebRequestObject().Load(this._ModelPost.UrlBase + partUrl, this._TipoRequest.ToString());
            }
            catch
            {
                this._Response = string.Empty;
                this.ResponseUri = (Uri)null;
            }
            finally
            {
                this._Request.Abort();
            }
            return this._Response;
        }

        public byte[] GetArquivoPost()
        {
            return this.ExecutarPostGetArquivo(string.Empty, string.Empty, true);
        }

        public byte[] GetArquivoPost(string partUrl)
        {
            return this.ExecutarPostGetArquivo(partUrl, string.Empty, true);
        }

        public byte[] GetArquivoPost(string partUrl, string parametros)
        {
            return this.ExecutarPostGetArquivo(partUrl, parametros, true);
        }

        public byte[] GetArquivo()
        {
            return this.ExecutarPostGetArquivo(string.Empty, string.Empty, false);
        }

        public byte[] GetArquivo(string partUrl)
        {
            return this.ExecutarPostGetArquivo(partUrl, string.Empty, false);
        }

        private byte[] ExecutarPostGetArquivo(string partUrl, string parametros, bool post)
        {
            this._GetArquivo = true;
            if (post)
                this.Post(partUrl, parametros);
            else
                this.Get(partUrl);
            byte[] arquivo = this._Arquivo;
            this._Arquivo = (byte[])null;
            return arquivo;
        }

        public string GetImagem(string partUrl)
        {
            this._GetArquivo = true;
            this._GetImagem = true;
            this.Get(partUrl);
            return this._Response;
        }

        private HtmlWeb CreateWebRequestObject()
        {

            return new HtmlWeb()
            {
                UseCookies = true,
                UserAgent = this._ModelGet.UserAgent,
                PreRequest = new HtmlWeb.PreRequestHandler(this.OnPreRequest),
                PostResponse = new HtmlWeb.PostResponseHandler(this.OnAfterResponse)               
            };

            //HtmlWeb htmlWeb = new HtmlWeb();
            //htmlWeb.UseCookies = true;
            //htmlWeb.UserAgent = this._ModelGet.UserAgent;
            //////ISSUE: method pointer
            ////htmlWeb.PreRequest = (Null)new HtmlWeb.PreRequestHandler((object)this,  __methodptr(OnPreRequest));
            //////// ISSUE: method pointer
            ///////htmlWeb.PostResponse = (__Null) new HtmlWeb.PostResponseHandler((object) this, __methodptr(OnAfterResponse));
            ////htmlWeb.PostResponse = new HtmlWeb.PostResponseHandler((object)this, __methodptr(OnAfterResponse));
            //return htmlWeb;
        }

        protected bool OnPreRequest(HttpWebRequest request)
        {
            bool flag = true;
            this._Request = request;
            this.AddHeadersCookiesTo(request);
            try
            {
                if (this._TipoRequest.Equals((object)TypeRequest.Post) || this._TipoRequest.Equals((object)TypeRequest.Put))
                {
                    if (this._MultiFormData != null)
                        this.AddFormDataTo(request);
                    else
                        this.AddPostDataTo(request);
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            this.HeaderResponse = response.Headers;
            if (this._GetArquivo)
            {
                this.GetArquivoResponse(response);
            }
            else
            {
                this._Response = response.ToHtmlString(this._Encoding);
                this.ResponseUri = response.ResponseUri;
            }
        }

        private void AddPostDataTo(HttpWebRequest request)
        {
            byte[] bytes = this._Encoding.GetBytes(this._Parametros.ToCharArray());
            this._Parametros = string.Empty;
            request.ContentLength = (long)bytes.Length;
            request.ContentType = this._ModelPost.ContentType;
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
        }

        private void AddFormDataTo(HttpWebRequest request)
        {
            request.GetRequestStream().Write(this._MultiFormData, 0, this._MultiFormData.Length);
            request.ContentLength = (long)this._MultiFormData.Length;
            this._MultiFormData = (byte[])null;
        }

        private void GetArquivoResponse(HttpWebResponse response)
        {
            byte[] inArray = new BinaryReader(response.GetResponseStream(), this._Encoding).ReadBytes(10485760);
            if (this._GetImagem)
                this._Response = Convert.ToBase64String(inArray);
            else
                this._Arquivo = inArray;
            this._GetArquivo = false;
            this._GetImagem = false;
        }

        public void AddHeadersCookiesTo(HttpWebRequest request)
        {
            this.AddHeadersCookiesTo(request, int.MinValue);
        }

        public void AddHeadersCookiesTo(HttpWebRequest request, int timeOut)
        {
            request.CookieContainer = this.Cookies;
            ModelWebRequest modelWebRequest = this._TipoRequest.Equals((object)TypeRequest.Post) ? this._ModelPost : this._ModelGet;
            request.Accept = modelWebRequest.Accept;
            HttpWebRequest httpWebRequest = request;
            bool? nullable;
            int num;
            if (!this._RedirectAction.HasValue)
            {
                num = modelWebRequest.AllowAutoRedirect ? 1 : 0;
            }
            else
            {
                nullable = this._RedirectAction;
                num = nullable.Value ? 1 : 0;
            }
            httpWebRequest.AllowAutoRedirect = num != 0;
            request.AutomaticDecompression = modelWebRequest.AutomaticDecompression;
            request.Timeout = modelWebRequest.TimeOut;

            if (modelWebRequest.CachePolicy != null)
                request.CachePolicy = modelWebRequest.CachePolicy;
                

            request.ContentType = modelWebRequest.ContentType;
            if (modelWebRequest.Expect != null && modelWebRequest.Expect.Length > 0)
                request.Expect = modelWebRequest.Expect;

            request.KeepAlive = modelWebRequest.KeepAlive;
            request.UserAgent = modelWebRequest.UserAgent;
            if (timeOut != int.MinValue)
                request.Timeout = timeOut;

            if (this._Referer != null && this._Referer.Length > 0)
                request.Referer = this._Referer;

            nullable = new bool?();
            this._RedirectAction = nullable;
            this._Referer = (string)null;
            request.Headers.Add((NameValueCollection)modelWebRequest.Headers);
        }

        public void AddHeader(string name, string value)
        {
            this._ModelPost.Headers.Remove(name);
            this._ModelPost.Headers.Add(name, value);
            this._ModelGet = this._ModelPost;
        }

        public void AddCookie(string strCookie)
        {
            string[] strArray = strCookie.Split('=');
            this.Cookies.Add(new Uri(this._ModelPost.UrlBase), new Cookie(((IEnumerable<string>)strArray).First<string>(), ((IEnumerable<string>)strArray).Last<string>()));
        }

        public void RemoveHeader(string name)
        {
            this._ModelPost.Headers.Remove(name);
            this._ModelGet.Headers.Remove(name);
        }

        public void SetUrlBase(string urlBase)
        {
            this._ModelGet.UrlBase = this._ModelPost.UrlBase = urlBase;
        }

        public void SetRedirectAction(bool? redirectAction)
        {
            this._RedirectAction = redirectAction;
        }

        public void SetUserAgent(string userAgent)
        {
            this._ModelGet.UserAgent = userAgent;
            this._ModelPost.UserAgent = this._ModelGet.UserAgent;
        }

        public void SetReferer(string referer)
        {
            this._Referer = referer;
        }

        public void SetContentType(string contentType)
        {
            this._ModelPost.ContentType = this._ModelGet.ContentType = contentType;
        }

        public void SetSecurity()
        {
            this.SetSecurity(SecurityProtocolType.Tls12);
        }

        //public void SetIp(string ip)
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    if (HttpContext.Current.Request.IsLocal)
        //    {
        //        webRequest.ServicePoint.BindIPEndPointDelegate = delegate (
        //        ServicePoint servicePoint,
        //        IPEndPoint remoteEndPoint,
        //        int retryCount)
        //        {
        //            return new IPEndPoint(
        //                IPAddress.Parse("192.168.1.1"),
        //                0);
        //        };
        //    }
        //}

        public void SetSecurity(SecurityProtocolType security)
        {
            ServicePointManager.SecurityProtocol = security;
        }

        public void SetEncoding(string encod)
        {
            this.SetEncoding(Encoding.GetEncoding(encod));
        }

        public void SetEncoding(Encoding encod)
        {
            this._Encoding = encod;
        }

        public void SetKeepAlive(bool keepAlive)
        {
            this._ModelGet.KeepAlive = this._ModelPost.KeepAlive = keepAlive;
        }

        public void SetAccept(string accept)
        {
            this._ModelPost.Accept = this._ModelGet.Accept = accept;
        }

        public string HtmlResponse
        {
            get
            {
                return this._Response;
            }
        }

        public Uri ResponseUri { get; set; }

        public CookieContainer Cookies { get; } = new CookieContainer();

        public WebHeaderCollection HeaderResponse { get; set; } = new WebHeaderCollection();

        public HttpWebRequest _Request { get; set; }

        private ModelWebRequest _ModelPost { set; get; }

        private ModelWebRequest _ModelGet { set; get; }

        private Encoding _Encoding { set; get; } = Encoding.Default;

        private string _Response { set; get; } = string.Empty;

        private string _Parametros { set; get; } = string.Empty;

        private string _Referer { set; get; }

        private bool? _RedirectAction { set; get; }

        private TypeRequest _TipoRequest { set; get; }

        private byte[] _MultiFormData { set; get; }

        private bool _GetArquivo { set; get; }

        private bool _GetImagem { set; get; }

        private byte[] _Arquivo { set; get; }
    }
}
