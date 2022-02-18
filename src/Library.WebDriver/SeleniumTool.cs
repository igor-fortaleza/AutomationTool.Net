using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Library.WebDriver;
using Library.WebDriver.Enum;
using Library.System;
using Library.WebDriver.Options;
using Library.WebRequest.Model;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace Library.WebDriver
{
    public class SeleniumTool
    {
        public IWebDriver driver { get; set; }
        private IWebElement element { get; set; }
        public ModelSession<Browser> session { get; set; }

        /// <summary>
        /// Método StartDriver() abre o navegador, Chrome como padrão. 
        /// </summary>
        public void StartDriver()
        {
            Print.Message("*[Library.WebDriver] Abrindo Navegador...");

            try
            {
                session = new ModelSession<Browser>()
                {
                    Session = new Browser(TypeBrowser.Chrome, false, AppDomain.CurrentDomain.BaseDirectory, null, new List<ModelPreference>())
                };

                this.driver = session.Session.Driver;
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel abrir o navegador\n\n" + x.ToString());
                throw;
            }
        }

        /// <param name="url"> Determina a url a iniciar junto ao navegador.</param>
        /// <param name="isHidden"> Se instância do navegador vai ser oculta 
        /// ou visivel.</param>
        /// <param name="typeNavigation"> Enum(TypeBrowser) Tipo de navegador 
        /// a ser instanciado.</param>
        public void StartDriver(
            string url,
            TypeBrowser typeBrowser = TypeBrowser.Chrome,
            bool isHidden = false,
            List<string> arguments = null,
            List<ModelPreference> modelPreference = null,
            PreferenceOptions preferenceOptions = null)
        {
            Print.Message("[Library.WebDriver] Abrindo Navegador...");
            try
            {
                session = new ModelSession<Browser>()
                {
                    Session = new Browser(typeBrowser, isHidden, AppDomain.CurrentDomain.BaseDirectory, arguments, modelPreference, preferenceOptions)
                };

                this.driver = session.Session.Driver;
                this.driver.Url = url;
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel abrir o navegador \n\n" + x.ToString());
                throw;
            }
        }

        /// <summary>
        /// Método GoToUrl() leva a url. 
        /// </summary>
        /// <param name="url"> Determina a url.</param>
        public void GoToUrl(string url)
        {
            Print.Message($"[Library.WebDriver] Acessando URL ({url})");
            try
            {
                this.driver = session.Session.Driver;
                this.driver.Url = (url);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel acessar URL");
                throw new Exception(x.ToString());
            }
        }

        /// <summary>
        /// Método GetCookies() retorna todos cookies atual do navegador. 
        /// </summary>
        /// <returns>List<Cookies> - lista de cookies</returns>
        public List<Cookie> GetCookies()
        {
            return driver.GetCookie();
        }

        /// <summary>
        /// Método SetCookie() insere um cookie ao navegador. 
        /// </summary>
        /// <param name="cookie"> <Cookie> - cookie a inserir.</param>
        public void SetCookie(Cookie cookie)
        {
            driver.Manage().Cookies.AddCookie(cookie);
        }
        /// <summary>
        /// Método SetCookies() insere uma lista de cookies ao navegador. 
        /// </summary>
        /// <param name="cookies"> List<Cookie> - lista de cookies a inserir.</param>
        public void SetCookies(List<Cookie> cookies)
        {
            driver.SetCookie(cookies);
        }

        /// <summary>
        /// Método GetElement() busca um elemento na página. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento.</param>
        /// <param name="generateError"> Se opta por gerar erro ao não encontar elemento, 
        /// padrão<true> .</param>
        /// <returns> Um elemento <IWebElement>.</returns>
        public IWebElement GetElement(string elementLocalXpath, bool generateError = true)
        {
            try
            {
                return this.driver.FindElement(By.XPath(elementLocalXpath));
            }
            catch (Exception x)
            {
                if (generateError)
                {
                    Print.Error("*[Library.WebDriver] Não foi possivel encontrar elemento (" + elementLocalXpath + ")");
                    throw new Exception(x.ToString());
                }

                return (IWebElement)null;
            }
        }
        /// <param name="by"> Localizador do elemento.</param>
        /// <param name="generateError"> Se opta por gerar erro ao não encontar elemento, 
        /// padrão<true> .</param>
        /// <returns> Um elemento <IWebElement>.</returns>
        public IWebElement GetElement(By by, bool generateError = true)
        {
            try
            {
                return this.driver.FindElement(by);
            }
            catch (Exception x)
            {
                if (generateError)
                {
                    Print.Error("*[Library.WebDriver] Não foi possivel encontrar elemento (" + by.ToString() + ")"); // obs
                    throw new Exception(x.ToString());
                }
                return (IWebElement)null;
            }
        }

        /// <summary>
        /// Método GetElements() busca uma lista de elementos na página. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento.</param>
        /// <returns> Lista de elementos IList<IWebElement>.</returns>
        public IList<IWebElement> GetElements(string elementLocalXpath)
        {
            return this.driver.FindElements(By.XPath(elementLocalXpath));
        }
        /// <param name="by"> Localizador do elemento.</param>
        /// <returns> Lista de elementos IList<IWebElement>.</returns>
        public IList<IWebElement> GetElements(By by)
        {
            try
            {
                return this.driver.FindElements(by);
            }
            catch (Exception x)
            {
                Print.Error(x.ToString());
            }
            return null;
        }

        /// <summary>
        /// Método GetElementSelect() busca um elemento selector na página. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento.</param>
        /// <returns> O elemento buscado SelectElement.</returns>
        public SelectElement GetElementSelect(string elementLocalXpath)
        {
            try
            {
                return new SelectElement(GetElement(elementLocalXpath));
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel selecionar o Select(" + elementLocalXpath + ")");
                throw new Exception(x.ToString());
            }
        }
        /// <param name="by"> Localizador do elemento.</param>
        /// <returns> O elemento buscado SelectElement.</returns>
        public SelectElement GetElementSelect(By by)
        {
            try
            {
                return new SelectElement(this.driver.FindElement(by));
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel selecionar o Select(" + by.ToString() + ")");
                throw new Exception(x.ToString());
            }
        }

        /// <summary>
        /// Método SelectByText() seleciona um elemento no selector na página,
        /// pelo texto do elemento. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento.</param>
        /// <param name="byText"> Texto do elemento no select.</param>
        public void SelectByText(string elementLocalXpath, string byText)
        {
            try
            {
                new SelectElement(GetElement(elementLocalXpath)).SelectByText(byText);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel selecionar o Select(" + elementLocalXpath + ") do texto(" + byText + ")");
                throw new Exception(x.ToString());
            }
        }
        /// <summary>
        /// Método SelectByText() seleciona um elemento no selector na página,
        /// pelo texto do elemento. 
        /// </summary>
        /// <param name="element"> Elemento. </param>
        /// <param name="byText"> Texto do elemento no select. </param>
        public void SelectByText(IWebElement element, string byText)
        {
            try
            {
                new SelectElement(element).SelectByText(byText);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel selecionar o ElementSelect do texto(" + byText + ")");
                throw new Exception(x.ToString());
            }
        }
        /// <param name="by"> Localizador do elemento.</param>
        /// <param name="byText"> Texto do elemento no select.</param>
        public void SelectByText(By by, string byText)
        {
            try
            {
                new SelectElement(GetElement(by)).SelectByText(byText);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel selecionar o Select(" + by.ToString() + ") do texto(" + byText + ")");
                throw new Exception(x.ToString());
            }
        }

        /// <summary>
        /// Método GetElementContains() Busca um elemento que contenha tal texto
        /// dentro de um elemento pai. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento.</param>
        /// <param name="text"> Texto que contenha no elemento buscado.</param>
        /// <returns> O elemento buscado IWebElement.</returns>
        public IWebElement GetElementContains(string elementLocalXpath, string text)
        {
            return this.driver.FindElementContains(By.XPath(elementLocalXpath), text);
        }
        /// <param name="by"> Localizador do elemento.</param>
        /// <param name="text"> Texto que contenha no elemento buscado.</param>
        /// <returns> O elemento buscado IWebElement.</returns>
        public IWebElement GetElementContains(By by, string text)
        {
            return this.driver.FindElementContains(by, text);
        }

        /// <summary>
        /// Método GetElementByAttribute() Busca um elemento pelo atributo
        /// dentro de um elemento pai. 
        /// </summary>
        /// <param name="elementLocalXpath"> Xpath do elemento pai.</param>
        /// <param name="attribute"> Atributo do elemento.</param>
        /// <param name="valueAttribute"> Valor do atributo.</param>
        /// <returns> O elemento buscado IWebElement.</returns>
        public IWebElement GetElementByAttribute(string elementLocalXpath, string attribute, string valueAttribute)
        {
            return this.driver.FindElementByAttribute(By.XPath(elementLocalXpath), attribute, valueAttribute);
        }
        /// <param name="by"> Localizador do elemento pai.</param>
        /// <param name="attribute"> Atributo do elemento.</param>
        /// <param name="valueAttribute"> Valor do atributo.</param>
        /// <returns> O elemento buscado IWebElement.</returns>
        public IWebElement GetElementByAttribute(By by, string attribute, string valueAttribute)
        {
            return this.driver.FindElementByAttribute(by, attribute, valueAttribute);
        }

        /// <summary>
        /// Método GetElementsAttributeContains() Busca um elemento pelo atributo
        /// que contenha tal texto, dentro de um elemento pai. 
        /// </summary>
        /// <param name="by"> Localizador do elemento pai.</param>
        /// <param name="attributeName"> Atributo do elemento.</param>
        /// <param name="attributeValue"> Texto que contenha no atributo.</param>
        /// <returns> O elemento buscado IWebElement.</returns>
        public IList<IWebElement> GetElementsAttributeContains(By by, string attributeName, string attributeValue)
        {
            return this.driver.FindElementsByAttributeContains(by, attributeName, attributeValue);
        }

        public void Click(string elementLocalXpath)
        {
            try
            {
                GetElement(elementLocalXpath).Click();
            }
            catch (Exception x)
            {
                Print.Error($"*[Library.WebDriver] Não foi possivel clicar no elemento ({elementLocalXpath})");
                throw new Exception(x.ToString());
            }
        }
        public void Click(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (Exception x)
            {
                Print.Error($"*[Library.WebDriver] Não foi possivel clicar no elemento ({element.GetType()})");
                throw new Exception(x.ToString());
            }

        }
        public void Click(By by)
        {
            try
            {
                GetElement(by).Click();
            }
            catch (Exception x)
            {
                Print.Error($"*[Library.WebDriver] Não foi possivel clicar no elemento ({by.ToString()})");
                throw new Exception(x.ToString());
            }
        }
        public void ClickJs(string elementLocalXpath)
        {
            GetElement(elementLocalXpath).ClickJs();
        }
        public void ClickJs(IWebElement element)
        {
            element.ClickJs();
        }
        public void ClickJs(By by)
        {
            GetElement(by).ClickJs();
        }

        public void SetValue(string elementLocalXpath, string text)
        {
            GetElement(elementLocalXpath).SetAttributeJs("value", text);
        }
        public void SetValue(IWebElement element, string text)
        {
            element.SetAttributeJs("value", text);
        }
        public void SetValue(By by, string text)
        {
            GetElement(by).SetAttributeJs("value", text);
        }

        public void SendKeys(string elementLocal, string text)
        {
            GetElement(elementLocal).SendKeys(text);
        }
        public void SendKeys(IWebElement element, string text)
        {
            element.SendKeys(text);
        }
        public void SendKeys(By by, string text)
        {
            GetElement(by).SendKeys(text);
        }

        public void SendKeysTime(string elementLocalXPath, string text, int TimeSleep)
        {
            element = this.driver.FindElement(By.XPath(elementLocalXPath));

            text = text.TrimEnd();

            foreach (char c in text)
            {
                if (c.ToString() != "")
                {
                    Thread.Sleep(TimeSleep);
                    element.SendKeys(c.ToString());
                }
            }

            element.Clear();
        }
        public void SendKeysTime(By by, string text, int TimeSleep)
        {
            text = text.TrimEnd();

            foreach (char c in text)
            {
                if (c.ToString() != "")
                {
                    Thread.Sleep(TimeSleep);
                    GetElement(by).SendKeys(c.ToString());
                }
            }
        }

        public Boolean AwaitElement(string localXpath, int timeSecondse = 10, bool generateError = false)
        {
            WebDriverWait wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeSecondse));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(localXpath)));

                #region Metódo antigo
                //wait.Until(condition =>
                //{
                //    try
                //    {
                //        return GetElement(localXpath).Displayed;
                //    }
                //    catch (StaleElementReferenceException)
                //    {
                //        return false;
                //    }
                //    catch (NoSuchElementException)
                //    {
                //        return false;
                //    }
                //});
                #endregion

                return true;
            }
            catch
            {
                if (generateError)
                    throw new Exception("Elemento não encontrado");
                else
                    return false;
            }            
        }
        public Boolean AwaitElement(By by, int timeSecondse = 10, bool generateError = false)
        {
            WebDriverWait wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeSecondse));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(by)); 
                return true;
            }
            catch
            {
                if (generateError)
                    throw new Exception("Elemento não encontrado");
                else
                    return false;
            }
        }
        public Boolean AwaitElementContains(By by, string text, TimeSpan time)
        {
            if (AwaitElement(by, (int)time.TotalSeconds))
            {
                IList<IWebElement> elements = GetElements(by);
                foreach (IWebElement element in elements)
                {
                    if (element.Text.ToUpper().Contains(text.ToUpper()))                    
                        return true;
                    
                }
            }
            return false;
        }

        public Boolean ElementExist(string localXpath)
        {
            if (GetElement(localXpath, false) == (IWebElement)null)            
                return false;            
            return true;
        }
        public Boolean ElementExist(By by)
        {
            if (GetElement(by, false) == (IWebElement)null)            
                return false;            

            return true;
        }

        public Boolean HasElementContains(string elementLocalXPath, string text)
        {
            return this.driver.HasElementContains(By.XPath(elementLocalXPath), text);
        }

        public T ExecuteJScript<T>(string jsScript)
        {
            return driver.ExecuteJScript<T>(jsScript);
        }

        public void ExecuteJScript(string jsScript)
        {
            driver.ExecuteJScript(jsScript);
        }

        public FileInfo TakeScreenshot(string path, string fileName)
        {
            Print.Info("[Library.WebDriver] Efetuando captura de tela");
            try
            {
                if (Directory.Exists(path))
                {
                    this.driver.TakeScreenshotSaveAs(path, fileName, (ScreenshotImageFormat)0);
                    return SystemTool.WaitFile(path, fileName);
                }
                else
                {
                    Directory.CreateDirectory(path);
                    Print.Info("[Library.WebDriver] Diretório (" + path + ") foi criado");
                    Print.Info("[Library.WebDriver] Efetuando captura de tela");
                    this.driver.TakeScreenshotSaveAs(path, fileName, (ScreenshotImageFormat)0);
                    return SystemTool.WaitFile(path, fileName);
                }
            }
            catch
            {
                Print.Error("*[Library.WebDriver] Erro ao tirarPrint.Message");
            }
            return null;
        }

        public string AcceptAndGetAlertMessage()
        {
            this.driver.SwitchTo().Window(session.Session.MasterWindowHandle);
            return driver.AcceptAndGetAlertMessage();
        }

        public void SwitchToWindow()
        {
            try
            {
                this.driver.SwitchTo().Window(session.Session.MasterWindowHandle);
            }
            catch
            {
                Print.Error("*[Library.WebDriver] Não foi possivel instanciar a janela");
                throw new Exception();
            }
        }

        public IWebDriver SwitchToWindow(string windowName)
        {
            try
            {
                return this.driver.SwitchTo().Window(windowName);
            }
            catch
            {
                Print.Error("*[Library.WebDriver] Não foi possivel instanciar a janela");
                throw;
            }
        }

        public IWebDriver SwitchToContentDefault()
        {
            return driver.SwitchTo().DefaultContent();
        }

        public IWebDriver SwitchToFrame(int nFrame)
        {
            try
            {
                return this.driver.SwitchTo().Frame(nFrame);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel instanciar para o frame " + nFrame);
                throw x;
            }
        }
        public IWebDriver SwitchToFrame(IWebElement elementFrame)
        {
            try
            {
                return this.driver.SwitchTo().Frame(elementFrame);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel instanciar para o frame");
                throw x;
            }
        }
        public IWebDriver SwitchToFrame(string nameFrame)
        {
            try
            {
                return this.driver.SwitchTo().Frame(nameFrame);
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Não foi possivel instanciar para o frame");
                throw x;
            }
        }

        public byte[] ImageToByte(string localXPath) => ImageToByte(GetElement(localXPath));
        public byte[] ImageToByte(By by) => ImageToByte(GetElement(by));

        public void CloseDriver(bool generateError = true)
        {
            Print.Message("[Library.WebDriver] Fechando aba navegador");
            try
            {
                this.driver.Close();
            }
            catch
            {
                if (generateError)
                    throw;
            }
        }
        public void QuitDriver(bool generateError = true)
        {
            Print.Message("[Library.WebDriver] Fechando navegador");
            try
            {
                this.driver.Quit();
            }
            catch
            {
                if (generateError)
                    throw;
            }
        }

        public byte[] ImageToByte(IWebElement imageElement)
        {
            try
            {
                Bitmap screenshot = imageElement.ToBitmap();

                MemoryStream ms = new MemoryStream();
                screenshot.Save(ms, ImageFormat.Jpeg);

                return ms.ToArray();
            }
            catch (Exception x)
            {
                Print.Error("*[Library.WebDriver] Erro ao tranformar o elemento imagem para byte");
                throw new Exception(x.ToString());
            }
        }

        public void SetRecaptchaKey(string captchaKey) =>
           driver.SetRecaptchaKey(captchaKey);        

        public void ScrollToElement(IWebElement element) =>
            driver.ExecuteJScript("arguments[0].scrollIntoView(true);", element);

        public void ScrollToElement(string elementLocalXpath) =>      
            driver.ExecuteJScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath(elementLocalXpath)));

        public void ScrollToElement(By by) =>
            driver.ExecuteJScript("arguments[0].scrollIntoView(true);", driver.FindElement(by));

        public void ScrollPage(int up, int down) =>
            this.ExecuteJScript($"window.scrollBy({up},{down});");


        public void PageLoad(int seconds)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(seconds);
        }

        public string OpenNewTab(string link)
        {            
            this.ExecuteJScript(string.Format("window.open('{0}', '_blank');", link));
            var windows = driver.WindowHandles;
            var indexNewTab = windows.Count - 1;
            
            return windows[indexNewTab];
        }

        public string OpenNewTab(IWebElement a)
        {            
            this.ExecuteJScript(string.Format("window.open('{0}', '_blank');", a.GetAttribute("href")));
            var windows = driver.WindowHandles;
            var indexNewTab = windows.Count - 1;

            return windows[indexNewTab];
        }

        public void DeleteAllCookies()
        {
            driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}
