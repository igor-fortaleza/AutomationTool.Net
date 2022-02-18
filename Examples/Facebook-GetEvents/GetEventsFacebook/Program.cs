using GetEventsFacebook.Navigator;
using Library.System;
using Model.Generic;
using System;

namespace GetEventsFacebook
{
    class Program
    {
        static void Main(string[] args)
        {
            var userName = "";
            var password = "";
            var seeachEvent = "party";

            var rpaFace = new FacebookNavigator();

            var result = rpaFace.Login(userName, password);
            if (result.StatusAutomation == StatusAutomation.Ok)
            {
                var rEvents = rpaFace.GetEvents(seeachEvent);
                if (rEvents.StatusAutomation == StatusAutomation.Ok)
                {
                    foreach(var @event in rEvents.Return)
                    {
                        Print.Info($"Evento ({@event.Name}): \n");
                        Print.Info($"Data ({@event.Date}): \n");
                        Print.Info($"Address ({@event.Address}): \n");
                    }
                }
            }                
            else
                throw new Exception(result.Return.ToString());
        }
    }
}
