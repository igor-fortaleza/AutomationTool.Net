using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Library.WebDriver.Enum;
using Library.WebDriver.Options;
using Library.System;
using OpenQA.Selenium.Firefox;

namespace Library.WebDriver
{

    public class Browser
    {
        public Browser()
        {
            this.BrowserAssistantBuilder(TypeBrowser.Chrome, false, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public Browser(TypeBrowser browser)
        {
            this.BrowserAssistantBuilder(browser, false, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public Browser(TypeBrowser browser, bool isHidden)
        {
            this.BrowserAssistantBuilder(browser, isHidden, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public Browser(TypeBrowser browser, bool isHidden, string drivePath, List<string> arguments = null, List<ModelPreference> modelPreference = null,PreferenceOptions preferenceOptions = null)
        {
            if (preferenceOptions != null)
                if (string.IsNullOrEmpty(preferenceOptions.DownloadPath))
                    preferenceOptions.DownloadPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    else
                    {
                        if (!Directory.Exists(preferenceOptions.DownloadPath))
                        {
                            Print.Error("*[Library.WebDriver] Diretório de Download não existe");
                            throw new Exception();
                        }
                    }
            this.BrowserAssistantBuilder(browser, isHidden, drivePath, arguments, preferenceOptions);
        }

        public void BrowserAssistantBuilder(
            TypeBrowser typeBrowser,
            bool isHidden,
            string driverPath,
            List<string> arguments = null,
            PreferenceOptions preferenceOptions = null)
        {
            switch (typeBrowser)
            {
                case TypeBrowser.IE:
                    BuilderIE(isHidden, driverPath, null, preferenceOptions);
                    break;

                case TypeBrowser.Edge:
                    break;

                case TypeBrowser.Firefox:
                    BuilderFirefox(isHidden, driverPath, arguments, preferenceOptions);
                    break;

                case TypeBrowser.Chrome:
                    BuilderChrome(isHidden, driverPath, arguments, preferenceOptions);
                    break;

                default:
                    throw new Exception();
            }
        }

        private void BuilderChrome(bool isHidden,
            string driverPath,
            List<string> _arguments = null,
            PreferenceOptions _preferenceOptions = null,
            List<ModelPreference> _modelPreferences = null)
        {

            List<string> listArgumentsDefault = new List<string>()
                      {
                        "test-type",
                        "--disable-web-security",
                        "--allow-running-insecure-content",
                        "--allow-insecure-localhost",
                        "--reduce-security-for-testing",
                        "--harmony-async-await",
                        "--disable-extensions"
                      };

            if (isHidden)
                listArgumentsDefault.Add("headless");

            if (_arguments != null)
            {
                foreach (string argument in _arguments)
                {
                    listArgumentsDefault.Add(argument);
                }
            }

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.LeaveBrowserRunning = (true);
            chromeOptions.MinidumpPath = (driverPath);
            chromeOptions.AddArguments((IEnumerable<string>)listArgumentsDefault);

            if (_modelPreferences == null) _modelPreferences = new List<ModelPreference>();
            if (_preferenceOptions != null)
                    AddModelPreferencesChrome(ref _preferenceOptions, ref _modelPreferences, ref driverPath);

            AddPreferencesDefaultChrome(ref _modelPreferences);

            foreach (var modelUserProfilePreference in _modelPreferences)
                chromeOptions.AddUserProfilePreference(modelUserProfilePreference.PreferenceName, modelUserProfilePreference.PreferenceValue);            

            //((DriverOptions)chromeOptions).SetLoggingPreference((string)LogType.Browser, (LogLevel)5);
            //((DriverOptions)chromeOptions).SetLoggingPreference((string)LogType.Driver, (LogLevel)5);

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            this.Driver = (IWebDriver)new ChromeDriver(driverPath, chromeOptions, TimeSpan.FromSeconds(90.0));
        }              

        private void BuilderIE(bool isHidden,
            string driverPath,
            List<string> arguments,
            PreferenceOptions _preferenceOptions = null,
            List<ModelPreference> _modelPreferences = null)
        {

            this.Driver = (IWebDriver)new InternetExplorerDriver(driverPath, null, TimeSpan.FromSeconds(90.0));
        }


        private void BuilderFirefox(bool isHidden,
            string driverPath,
            List<string> _arguments = null,
            PreferenceOptions _preferenceOptions = null,
            List<ModelPreference> _modelPreferences = null)
        {
            if (isHidden)
                _arguments.Add("headless");

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@driverPath);

            if (_modelPreferences == null) _modelPreferences = new List<ModelPreference>();
            if (_preferenceOptions != null)
                 AddModelPreferencesFirefox(ref _preferenceOptions, ref _modelPreferences);

            AddPreferencesDefaultFirefox(ref _modelPreferences);

            foreach (var modelUserProfilePreference in _modelPreferences)
                firefoxOptions.SetPreference(modelUserProfilePreference.PreferenceName, (string)modelUserProfilePreference.PreferenceValue);

            if (_arguments != null) firefoxOptions.AddArguments((IEnumerable<string>)_arguments);

            this.Driver = (IWebDriver)new FirefoxDriver(driverPath, firefoxOptions, TimeSpan.FromSeconds(60));
        }

        private void AddPreferencesDefaultChrome(ref List<ModelPreference> modelPreferences)
        {
            modelPreferences.Add(new ModelPreference { PreferenceName = "download.directory_upgrade", PreferenceValue = true });
            modelPreferences.Add(new ModelPreference { PreferenceName = "profile.content_settings.exceptions.automatic_downloads.*.setting", PreferenceValue = 1 });
            modelPreferences.Add(new ModelPreference { PreferenceName = "Prints.Printing.Prints.Print_preview_sticky_settings.appState", PreferenceValue = (object)"{\"version\":2,\"isGcpPromoDismissed\":false,\"selectedDestinationId\":\"Save as PDF\"" });
            modelPreferences.Add(new ModelPreference { PreferenceName = "plugins.always_open_pdf_externally", PreferenceValue = (object)true });
            modelPreferences.Add(new ModelPreference { PreferenceName = "download.prompt_for_download", PreferenceValue = false });
            modelPreferences.Add(new ModelPreference { PreferenceName = "download.default_directory", PreferenceValue = false });
        }
        private void AddPreferencesDefaultFirefox(ref List<ModelPreference> modelPreferences)
        {
            //nothing yet
        }

        private void AddModelPreferencesChrome(ref PreferenceOptions preferenceOptions, ref List<ModelPreference> modelPreferences, ref string driverPath)
        {
            if (string.IsNullOrEmpty(preferenceOptions.DownloadPath))
                modelPreferences.Add(new ModelPreference { PreferenceName = "download.default_directory", PreferenceValue = (object)preferenceOptions.DownloadPath });
            if (string.IsNullOrEmpty(preferenceOptions.DriverPath))
                driverPath = preferenceOptions.DriverPath;
        }
        private void AddModelPreferencesFirefox(ref PreferenceOptions preferenceOptions, ref List<ModelPreference> modelPreferences)
        {
            if (string.IsNullOrEmpty(preferenceOptions.DownloadPath))
            {
                modelPreferences.Add(new ModelPreference { PreferenceName = "browser.download.folderList", PreferenceValue = 2 });
                modelPreferences.Add(new ModelPreference { PreferenceName = "browser.download.dir", PreferenceValue = preferenceOptions.@DownloadPath });
                modelPreferences.Add(new ModelPreference { PreferenceName = "browser.download.useDownloadDir", PreferenceValue = true });
            }
            if (preferenceOptions.NeverAskSavePdf)
                modelPreferences.Add(new ModelPreference { PreferenceName = "browser.helperApps.neverAsk.saveToDisk", PreferenceValue = "application/pdf" });
            if (preferenceOptions.OpenPdfExternally)
                modelPreferences.Add(new ModelPreference {PreferenceName = "pdfjs.disabled", PreferenceValue = true});
        }


        public IWebDriver Driver { get; set; }
        public string MasterWindowHandle { get; set; }
    }
}
