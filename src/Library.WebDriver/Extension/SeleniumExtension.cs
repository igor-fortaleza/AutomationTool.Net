using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Model.Generic.Extension;
using Library.WebRequest;  //Para liberar DownloadFileGet e DownloadFilePost
using Library.WebDriver.Enum;
using Library.System;

namespace Library.WebDriver
{
    public static class SeleniumExtension
    {
        public static IWebDriver GetWebDriver(this IWebElement webElement)
        {
            return ((IWrapsDriver)webElement).WrappedDriver;
        }

        public static IWebElement GetElement(this IWebElement element, By by)
        {
            try
            {
                return element.FindElement(by);
            }
            catch (Exception x)
            {
                Print.Error("Não foi possivel encontrar elemento (" + by.ToString() + ") dentro do elemento anterior"); // obs
                throw new Exception(x.ToString());
            }
        }
        public static IList<IWebElement> GetElements(this IWebElement element, By by)
        {
            try
            {
                return element.FindElements(by);
            }
            catch (Exception x)
            {
                Print.Error("Não foi possivel encontrar os elementos (" + by.ToString() + ") dentro do elemento anterior"); // obs
                throw new Exception(x.ToString());
            }
        }

        public static string AcceptAndGetAlertMessage(this IWebDriver driver)
        {
            string str;
            try
            {
                try
                {
                    driver.SwitchTo().Alert();
                }
                catch
                {

                    Print.Message("Não foi encontrado nenhuma alerta");
                    str = string.Empty;
                    return str;
                }
                str = driver.SwitchTo().Alert().Text;
                driver.SwitchTo().Alert().Accept();
                driver.SwitchTo().DefaultContent();
            }
            catch
            {
                Print.Error("\n Erro ao aceitar Alerta \n");
                str = string.Empty;
            }
            return str;
        }

        public static string GetFieldByBoundaries(this IWebDriver driver, string lBound, string rBound)
        {
            try
            {
                string pageSource = driver.PageSource;
                string str = lBound;
                int startIndex = pageSource.IndexOf(str) + str.Length;
                return pageSource.Substring(startIndex, pageSource.IndexOf(rBound, startIndex) - startIndex);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static List<Cookie> GetCookie(this IWebDriver driver)
        {
            return ((IEnumerable<Cookie>)driver.Manage().Cookies.AllCookies).ToList<Cookie>();
        }

        public static void SetCookie(this IWebDriver driver, Cookie cookie)
        {
            driver.Manage().Cookies.AddCookie(cookie);
        }

        public static void SetCookie(this IWebDriver driver, List<Cookie> cookies)
        {
            ICookieJar cookies1 = driver.Manage().Cookies;
            using (List<Cookie>.Enumerator enumerator = cookies.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Cookie current = enumerator.Current;
                    cookies1.AddCookie(current);
                }
            }
        }

        public static void SetCaptchaKey(this IWebDriver driver, string CaptchaKey)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('style', 'display: true;');", new object[1]
                {
          (object) ((ISearchContext) driver).FindElement(By.XPath("//*[@id=\"g-recaptcha-response\"]"))
                });
                ((ISearchContext)driver).FindElement(By.XPath("//*[@id=\"g-recaptcha-response\"]")).SendKeys(CaptchaKey);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao preencher Google Captcha: " + ex.Message);
            }
        }

        public static bool FireEvent(this IWebElement webElement, TypeEvent eventType)
        {
            try
            {
                webElement.GetWebDriver().ExecuteJScript("var fireOnThis = arguments[0];\r\n                                        fireOnThis.scrollIntoView(false);\r\n                                        var evt = arguments[1];\r\n                                        if (document.createEvent) {\r\n                                            var evObj = document.createEvent('MouseEvents');\r\n                                            evObj.initEvent(evt, true, false);\r\n                                            fireOnThis.dispatchEvent(evObj);\r\n                                        } else if (document.createEventObject) {\r\n                                            fireOnThis.fireEvent('on' + evt);\r\n                                        }", (object)webElement, (object)eventType.GetDescription());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<IWebElement> GetChildren(this IWebElement webElement)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)webElement).FindElements(By.XPath(".//child::*"))).ToList<IWebElement>();
            }
            catch
            {
                return new List<IWebElement>();
            }
        }

        public static void TakeScreenshotSaveAs(
          this IWebDriver driver,
          string urlFile,
          string fileName)
        {
            driver.TakeScreenshotSaveAs(urlFile, fileName, (ScreenshotImageFormat)0);
        }

        public static void TakeScreenshotSaveAs(
          this IWebDriver driver,
          string urlFile,
          string fileName,
          ScreenshotImageFormat format)
        {
            Screenshot screenshot = (driver as ITakesScreenshot).GetScreenshot();
            switch ((int)format)
            {
                case 0:
                    screenshot.SaveAsFile(urlFile + $@"\" + fileName + ".png", (ScreenshotImageFormat)0);
                    break;
                case 1:
                    screenshot.SaveAsFile(urlFile + $@"\" + fileName + ".jpg", (ScreenshotImageFormat)1);
                    break;
                case 3:
                    screenshot.SaveAsFile(urlFile + $@"\" + fileName + ".tiff", (ScreenshotImageFormat)3);
                    break;
                case 4:
                    screenshot.SaveAsFile(urlFile + $@"\" + fileName + ".bmp", (ScreenshotImageFormat)4);
                    break;
            }
        }

        public static void PopupHandler(this IWebDriver browser, TypePopupAction action)
        {
            browser.PopupHandler(action, string.Empty);
        }

        public static void PopupHandler(this IWebDriver browser, TypePopupAction action, string title)
        {
            List<string> list = browser.WindowHandles.ToList<string>();
            switch (action)
            {
                case TypePopupAction.Open:
                    if (title == string.Empty)
                    {
                        browser.SwitchTo().Window(list.LastOrDefault<string>());
                        break;
                    }
                    using (IEnumerator<string> enumerator = browser.WindowHandles.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            string current = enumerator.Current;
                            browser.SwitchTo().Window(current);
                            browser.StopLoad();
                            if (browser.Title.Contains(title))
                                break;
                        }
                        break;
                    }
                case TypePopupAction.Close:
                    if (title == string.Empty)
                    {
                        browser.SwitchTo().Window(list.LastOrDefault<string>());
                        if (!(browser.CurrentWindowHandle != browser.WindowHandles.ToString()))
                            break;
                        browser.Close();
                        browser.SwitchTo().Window(browser.WindowHandles.ToString());
                        break;
                    }
                    foreach (string windowHandle in browser.WindowHandles)
                    {
                        browser.SwitchTo().Window(windowHandle);
                        browser.StopLoad();
                        if (browser.Title.Contains(title))
                        {
                            browser.Close();
                            break;
                        }
                    }
                    browser.SwitchTo().Window(browser.WindowHandles.ToString());
                    break;
                case TypePopupAction.CloseAll:
                    foreach (string windowHandle in browser.WindowHandles)
                    {
                        try
                        {
                            browser.SwitchTo().Window(windowHandle);
                        }
                        catch
                        {
                            continue;
                        }
                        if (browser.CurrentWindowHandle != browser.WindowHandles.ToString())
                            browser.Close();
                    }
                    browser.SwitchTo().Window(browser.WindowHandles.ToString());
                    break;
            }
        }

        public static IWebElement FindFirstElement(this IWebElement element, By by)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).FirstOrDefault<IWebElement>();
        }

        public static IWebElement FindElementContains(
          this IWebElement element,
          By by,
          string text)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x => x.Text.Contains(text)));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement FindElementByAttribute(
          this IWebElement element,
          By by,
          string attributeName,
          string attributeValue)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x =>
                {
                    if (x.GetAttribute(attributeName) != null)
                        return x.GetAttribute(attributeName).Equals(attributeValue, StringComparison.InvariantCulture);
                    return false;
                }));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement FindElementByAttributeContains(
          this IWebElement element,
          By by,
          string attributeName,
          string attributeValue)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x =>
                {
                    if (x.GetAttribute(attributeName) != null)
                        return x.GetAttribute(attributeName).Contains(attributeValue);
                    return false;
                }));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static List<IWebElement> FindListOfElements(
          this IWebElement element,
          By by)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsContains(
          this IWebElement element,
          By by,
          string text)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x => x.Text.Contains(text))).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsByAttribute(
          this IWebElement element,
          By by,
          string attributeName,
          string attributeValue)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x =>
            {
                if (x.GetAttribute(attributeName) != null)
                    return x.GetAttribute(attributeName).Equals(attributeValue, StringComparison.InvariantCulture);
                return false;
            })).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsByAttributeContains(
          this IWebElement element,
          By by,
          string attributeName,
          string attributeValue)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)element).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x =>
            {
                if (x.GetAttribute(attributeName) != null)
                    return x.GetAttribute(attributeName).Contains(attributeValue);
                return false;
            })).ToList<IWebElement>();
        }

        public static bool HasElement(this IWebElement element, By by)
        {
            return element.FindFirstElement(by) != null;
        }

        public static bool HasElementContains(this IWebElement element, By by, string text)
        {
            return element.FindElementContains(by, text) != null;
        }

        public static bool HasElementContains(
          this IWebElement element,
          By by,
          string attribute,
          string attributeContent)
        {
            return element.FindElementByAttributeContains(by, attribute, attributeContent) != null;
        }

        public static bool HasElement(
          this IWebElement element,
          By by,
          string attribute,
          string attributeContent)
        {
            return element.FindElementByAttribute(by, attribute, attributeContent) != null;
        }

        public static byte[] DownloadFileGet(
          this IWebDriver driver,
          Request request,
          string urlEspecifica)
        {
            using (List<Cookie>.Enumerator enumerator = driver.GetCookie().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Cookie current = enumerator.Current;
                    request.AddCookie(current.Name + "=" + current.Value);
                }
            }
            return request.GetArquivo(urlEspecifica);
        }

        public static byte[] DownloadFilePost(
          this IWebDriver driver,
          Request request,
          string urlEspecifica,
          string parametros)
        {
            using (List<Cookie>.Enumerator enumerator = driver.GetCookie().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Cookie current = enumerator.Current;
                    request.AddCookie(current.Name + "=" + current.Value);
                }
            }
            return request.GetArquivoPost(urlEspecifica, parametros);
        }

        public static bool SelectComboByText(this IWebElement webElement, string text)
        {
            return webElement.SelectComboByText(text, false);
        }

        public static bool SelectComboByText(this IWebElement webElement, string text, bool contains)
        {
            try
            {
                new SelectElement(webElement).SelectByText(text, contains);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SelectComboByIndex(this IWebElement webElement, int index)
        {
            try
            {
                new SelectElement(webElement).SelectByIndex(index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SelectComboByValue(this IWebElement webElement, string value)
        {
            try
            {
                new SelectElement(webElement).SelectByValue(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement GetParentElement(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<IWebElement>("return arguments[0].parentElement;", (object)webElement);
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement GetParentElement(this IWebElement webElement, string tagName)
        {
            webElement = webElement.GetParentElement();

            while (webElement.TagName != tagName)
                webElement = webElement.GetParentElement();

            return webElement.Equals(null) ? (IWebElement)null : webElement;
        }

        public static IWebElement GetFirstElementChild(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<IWebElement>("return arguments[0].firstElementChild;", (object)webElement);
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement GetPreviousElementSibling(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<IWebElement>("return arguments[0].previousElementSibling;", (object)webElement);
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement GetNextElementSibling(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<IWebElement>("return arguments[0].nextElementSibling;", (object)webElement);
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement GetOuterHtml(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<IWebElement>("return arguments[0].outerHtml;", (object)webElement);
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static void ExecuteJScript(
          this IWebDriver driver,
          string jsFunction,
          params object[] args)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(jsFunction, args);
        }

        public static T ExecuteJScript<T>(
          this IWebDriver driver,
          string jsFunction,
          params object[] args)
        {
            return (T)((IJavaScriptExecutor)driver).ExecuteScript(jsFunction, args);
        }

        public static bool SetAttributeJs(
          this IWebElement webElement,
          string attributeName,
          string attributeValue)
        {
            try
            {
                ((IJavaScriptExecutor)webElement.GetWebDriver()).ExecuteScript("return arguments[0]." + attributeName + " = arguments[1]", new object[2]
                {
          (object) webElement,
          (object) attributeValue
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ClickJs(this IWebElement webElement)
        {
            try
            {
                webElement.GetWebDriver().ExecuteJScript("arguments[0].click();", (object)webElement);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool StopLoad(this IWebDriver driver)
        {
            try
            {
                driver.ExecuteJScript("return window.stop;", new object[0]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] DownloadFile(this IWebDriver driver, string link)
        {
            try
            {
                return driver.ExecuteJScript<byte[]>("var xmlHttp = new XMLHttpRequest();" + "return (function() {" + "    xmlHttp.open('GET', arguments[0]);" + "    xmlHttp.onreadystatechange = function() {" + "        if (xmlHttp.readyState == 4)" + "        {" + "            return xmlHttp.response;" + "        }" + "    };" + "    xmlHttp.send(null);" + "})();", (object)link);
            }
            catch
            {
                return (byte[])null;
            }
        }

        public static string GetImageJS(this IWebElement webElement)
        {
            try
            {
                return webElement.GetWebDriver().ExecuteJScript<string>("return (function(img){" + "var canvas = document.createElement('canvas');" + "canvas.width = img.width;" + "canvas.height = img.height;" + "var ctx = canvas.getContext('2d');" + "ctx.drawImage(img, 0, 0);" + "var dataURL = canvas.toDataURL('image/png');" + "return dataURL.replace(/^data:image\\/(png|jpg);base64,/, '');" + "})(arguments[0])", (object)webElement);
            }
            catch
            {
                return (string)null;
            }
        }

        public static IWebElement FindFirstElement(this IWebDriver driver, By by)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).FirstOrDefault<IWebElement>();
        }

        public static IWebElement FindElementContains(
          this IWebDriver driver,
          By by,
          string text)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x => x.Text.Contains(text)));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement FindElementByAttribute(
          this IWebDriver driver,
          By by,
          string attributeName,
          string attributeValue)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x =>
                {
                    if (x.GetAttribute(attributeName) != null)
                        return x.GetAttribute(attributeName).Equals(attributeValue, StringComparison.InvariantCulture);
                    return false;
                }));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static IWebElement FindElementByAttributeContains(
          this IWebDriver driver,
          By by,
          string attributeName,
          string attributeValue)
        {
            try
            {
                return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).FirstOrDefault<IWebElement>((Func<IWebElement, bool>)(x =>
                {
                    if (x.GetAttribute(attributeName) != null)
                        return x.GetAttribute(attributeName).Contains(attributeValue);
                    return false;
                }));
            }
            catch
            {
                return (IWebElement)null;
            }
        }

        public static List<IWebElement> FindListOfElements(this IWebDriver driver, By by)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsContains(
          this IWebDriver driver,
          By by,
          string text)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x => x.Text.Contains(text))).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsByAttribute(
          this IWebDriver driver,
          By by,
          string attributeName,
          string attributeValue)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x =>
            {
                if (x.GetAttribute(attributeName) != null)
                    return x.GetAttribute(attributeName).Equals(attributeValue, StringComparison.InvariantCulture);
                return false;
            })).ToList<IWebElement>();
        }

        public static List<IWebElement> FindElementsByAttributeContains(
          this IWebDriver driver,
          By by,
          string attributeName,
          string attributeValue)
        {
            return ((IEnumerable<IWebElement>)((ISearchContext)driver).FindElements(by)).Where<IWebElement>((Func<IWebElement, bool>)(x =>
            {
                if (x.GetAttribute(attributeName) != null)
                    return x.GetAttribute(attributeName).Contains(attributeValue);
                return false;
            })).ToList<IWebElement>();
        }

        public static bool HasElement(this IWebDriver driver, By by)
        {
            return driver.FindFirstElement(by) != null;
        }

        public static bool HasElementContains(this IWebDriver driver, By by, string text)
        {
            return driver.FindElementContains(by, text) != null;
        }

        public static bool HasElementContains(
          this IWebDriver driver,
          By by,
          string attribute,
          string attributeContent)
        {
            return driver.FindElementByAttributeContains(by, attribute, attributeContent) != null;
        }

        public static bool HasElement(
          this IWebDriver driver,
          By by,
          string attribute,
          string attributeContent)
        {
            return driver.FindElementByAttribute(by, attribute, attributeContent) != null;
        }

        public static bool HasAlert(this IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                driver.SwitchTo().DefaultContent();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Bitmap ToBitmap(this IWebElement webElement)
        {
            byte[] byteArray = ((ITakesScreenshot)webElement.GetWebDriver()).GetScreenshot().AsByteArray;
            Bitmap screenshot = new Bitmap(new MemoryStream(byteArray));

            Rectangle croppedImage = new Rectangle(webElement.Location.X, webElement.Location.Y, webElement.Size.Width, webElement.Size.Height);
            return screenshot.Clone(croppedImage, screenshot.PixelFormat);
        }


        public static bool AwaitElement(this IWebDriver driver, By by)
        {
            return driver.AwaitElement(by, TimeSpan.FromSeconds(10.0));
        }

        public static bool AwaitElement(
          this IWebDriver driver,
          By by,
          string attribute,
          string attributeContent)
        {
            return driver.AwaitElement(by, TimeSpan.FromSeconds(10.0), attribute, attributeContent);
        }

        public static bool AwaitElementContains(this IWebDriver driver, By by, string text)
        {
            return driver.AwaitElementContains(by, TimeSpan.FromSeconds(10.0), text);
        }

        public static bool AwaitElementContains(
          this IWebDriver driver,
          By by,
          string attribute,
          string attributeContent)
        {
            return driver.AwaitElementContains(by, TimeSpan.FromSeconds(10.0), attribute, attributeContent);
        }

        public static bool AwaitElement(this IWebDriver driver, By by, TimeSpan time)
        {
            try
            {
                return ((DefaultWait<IWebDriver>)new WebDriverWait(driver, time)).Until(drv => drv.HasElement(by));
            }
            catch
            {
                return false;
            }
        }

        public static bool AwaitElement(
          this IWebDriver driver,
          By by,
          TimeSpan time,
          string attribute,
          string attributeContent)
        {
            try
            {
                return (bool)((DefaultWait<IWebDriver>)new WebDriverWait(driver, time)).Until<bool>(drv => drv.HasElement(by, attribute, attributeContent));
            }
            catch
            {
                return false;
            }
        }

        public static bool AwaitElementContains(
          this IWebDriver driver,
          By by,
          TimeSpan time,
          string text)
        {
            try
            {
                return (bool)((DefaultWait<IWebDriver>)new WebDriverWait(driver, time)).Until<bool>((drv => drv.HasElementContains(by, text)));
            }
            catch
            {
                return false;
            }
        }

        public static bool AwaitElementContains(
          this IWebDriver driver,
          By by,
          TimeSpan time,
          string attribute,
          string attributeContent)
        {
            try
            {
                return (bool)((DefaultWait<IWebDriver>)new WebDriverWait(driver, time)).Until<bool>(drv => drv.HasElementContains(by, attribute, attributeContent));
            }
            catch
            {
                return false;
            }
        }

        public static void SetRecaptchaKey(this IWebDriver driver, string captchaKey)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('style', 'display: true;');", new object[1]
                {
          (object) ((ISearchContext) driver).FindElement(By.XPath("//*[@id=\"g-recaptcha-response\"]"))
                });
                ((ISearchContext)driver).FindElement(By.XPath("//*[@id=\"g-recaptcha-response\"]")).SendKeys(captchaKey);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao preencher Google Captcha: " + ex.Message);
            }
        }
    }
}
