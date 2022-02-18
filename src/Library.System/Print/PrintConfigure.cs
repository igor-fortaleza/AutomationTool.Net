using System;

namespace Library.System
{
    public static class PrintConfigure
    {
        public static bool registerLog;
        public static void ActivePrintLog(bool active)
        {
            if (!string.IsNullOrEmpty(SystemConfigurate.NameFileLog))
            {
                registerLog = active;
            }
            else
            {
                new Exception("Diretorio vazio");
            }

        }
    }
}
