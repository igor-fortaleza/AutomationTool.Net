using GetEventsFacebook.Models;
using Library.System;
using Library.WebDriver;
using Library.WebDriver.Enum;
using Library.WebDriver.Options;
using Model.Generic;
using Model.Generic.Extension;
using Model.Generic.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace GetEventsFacebook.Navigator
{
    public class ModelFilter
    {
        public string name { get; set; }
        public string args { get; set; }
    }

    class FacebookNavigator : SeleniumTool
    {
        public readonly string Link = @"https://pt-br.facebook.com/";
        public readonly string InputPesquisa = "//input[@aria-label='Pesquisar no Facebook']";
        
        public ModelRecordResult<object> Login(string user, string pass)
        {
            var result = new ModelRecordResult<object>();

            var prefs = new Dictionary<string, int>();
            prefs.Add("profile.default_content_setting_values.notifications", 2);

            var preferences = new List<ModelPreference> {
                new ModelPreference
                {
                PreferenceName = "prefs",
                PreferenceValue = prefs,
                }
            };
            var arguments = new List<string> { "--disable-infobars", "--disable-notifications" };

            StartDriver(Link, TypeBrowser.Chrome, false, arguments: arguments, modelPreference: preferences);

            //or no preferences
            StartDriver(Link, TypeBrowser.Chrome);

            if (AwaitElement("//a[text()='Esqueceu a senha?']"))
            {
                SendKeys("//input[@id='email']", user);
                SendKeys("//input[@id='pass']", pass + Keys.Enter);

                if (AwaitElement(InputPesquisa, 10))
                {
                    result.SetEndProcess(StatusAutomation.Ok);
                }
                else if (AwaitElement("//*[contains(text(), 'A senha inserida está incorreta.')]", 3))
                {
                    var error = "A senha inserida está incorreta.";
                    Print.Error(error);
                    result.SetEndProcess(StatusAutomation.UsuarioSenhaErrado, error);
                    return result;
                }
                else if (AwaitElement("//a[text()='Não é você?']", 2))
                {
                    var error = "Usuário incorreto.";
                    Print.Error(error);
                    result.SetEndProcess(StatusAutomation.UsuarioSenhaErrado, error);
                    return result;
                }
                else
                    result.SetEndProcess(StatusAutomation.ErroGenerico, "Erro genérico");
            }
            return result;
        }

        public ModelRecordResult<List<Event>> GetEvents(string pesquisa, int daysFilter = 7)
        {
            var result = new ModelRecordResult<List<Event>>();            

            var filter = GetFilter(daysFilter);
            GoToUrl(Link + $"search/events?q={pesquisa}&filters={filter}");
            AwaitElement("//*[@aria-label='Resultados da pesquisa']", 10);
            var resultadoPesquisa = GetElement("//*[@aria-label='Resultados da pesquisa']");

            //OBS: passar scroll
            //ScrollPage(0, 5);

            AwaitElement("//*[@aria-label='Tenho interesse']", 10);
            var eventsComponent = resultadoPesquisa.GetElements(By.XPath("//*[@aria-label='Tenho interesse']"));

            var events = new List<Event>();
            foreach (var eventoComponent in eventsComponent)
            {
                var eventoe = eventoComponent.GetParentElement("div").GetParentElement("div");
                if (eventoe.Text.ToUpper().Contains("EVENTO ONLINE"))
                    continue;

                var clicks = eventoe.GetElements(By.TagName("a"));
                ExecuteJScript(string.Format("window.open('{0}', '_blank');", clicks[1].GetAttribute("href")));

                var windows = driver.WindowHandles;
                SwitchToWindow(windows[1]);
                AwaitElement("//h2[.='Detalhes']", 30);

                var nome = GetElement("//div[@role='main']").
                    FindFirstElement(By.TagName("div"))
                    .GetElements(By.TagName("h2"));

                var date = nome[0].Text.ToUpper();
                var nameEvent = nome[1].Text;

                var datails = GetElement("//h2[.='Detalhes']")
                    .GetParentElement("div")
                    .GetParentElement("div")
                    .GetParentElement("div")
                    .GetParentElement("div").GetElements(By.TagName("i"));

                var address = datails[1].GetParentElement("div").GetParentElement("div").Text;
                var imageLink = GetElement("//img[@data-imgperflogname='profileCoverPhoto']").GetAttribute("src");

                byte[] bytes = null;
                using (WebClient client = new WebClient())
                    bytes = client.DownloadData(new Uri(imageLink));
                Thread.Sleep(1000);

                events.Add(new Event
                {
                    Name = nameEvent,
                    Date = date,
                    Address = address,
                    ImageBytes = bytes
                });

                driver.Close();
                SwitchToWindow(windows[0]);
            }

            result.SetEndProcess(StatusAutomation.Ok, events);

            return result;
        }

        private string GetFilter(int days)
        {
            var dateNow = DateTime.Now;
            dateNow.ToString("yyyy-MM-dd");

            var jsonFilter = JsonConvert.SerializeObject(new ModelFilter
            {
                name = "filter_events_date",
                args = dateNow.ToString("yyyy-MM-dd") + "~" + dateNow.AddDays(days).ToString("yyyy-MM-dd")
            });
            jsonFilter = jsonFilter.Replace("\"", @"\""");

            var filter = "{\"filter_events_date_range:0\":\"" + jsonFilter + "\"}7";
            var filterBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(filter);
            var filter64 = System.Convert.ToBase64String(filterBytes);

            var last = filter64.Substring(filter64.Length - 1, 1);
            last = $"%{last}D";

            filter64 = filter64.Remove(filter64.Length - 1);
            filter64 += last;

            return filter64;
        }
    }
}
