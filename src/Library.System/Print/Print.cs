using System;
using System.IO;

namespace Library.System
{
    public static class Print
    {
        private static string nameFileLogNow = null;

        public static void Message(string text, bool line = true)
        {
            if (!line)
                Console.Write(text);
            else
                Console.WriteLine(text);

            if (PrintConfigure.registerLog)
                SaveConsoleLog(text);
        }
        public static void Error(string textError, bool line = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            if (!line)
                Console.Write(textError);
            else
                Console.WriteLine(textError);

            Console.ResetColor();

            if (PrintConfigure.registerLog)
                SaveConsoleLog(textError);
        }
        public static void Info(string text, bool line = true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            if (!line)
                Console.Write(text);
            else
                Console.WriteLine(text);

            Console.ResetColor();

            if (PrintConfigure.registerLog)
                SaveConsoleLog(text);
        }
        public static void Sucess(string text, bool line = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (!line)
                Console.Write(text);
            else
                Console.WriteLine(text);

            Console.ResetColor();

            if (PrintConfigure.registerLog)
                SaveConsoleLog(text);
        }

        public static void SaveConsoleLog(string text)
        {
            RegisterLog(text);
        }
        private static void RegisterLog(string text)
        {
            try
            {
                nameFileLogNow = SystemConfigurate.GetFileLog();
                if (!string.IsNullOrEmpty(nameFileLogNow))
                {
                    string pathLog = SystemConfigurate.GetPathLog();

                    if (Directory.Exists(pathLog))
                    {
                        SaveLogTxt(text, Path.Combine(pathLog, nameFileLogNow).ToString());
                    }
                    else
                    {
                        Print.Error("\n[library.system.print] Seu diretório não foi configurado ou não existe, use a função ConfiguratePathLog() e o SystemConfigurate corretamente");
                        throw new Exception();
                    }
                }
                else
                {
                    Print.Error("\n[library.system.print]  Impossivel de gravar log pois não foi denifido nenhum nome de arquivo");
                    throw new Exception();
                }
            }
            catch
            {
                Print.Error("\n[library.system.print] Erro ao criar diretorio do LOG");
                throw;
            }
        }
        private static void SaveLogTxt(string conteudo, string arquivoLog)
        {
            try
            {
                if (!File.Exists(arquivoLog))
                {
                    File.Create(arquivoLog).Dispose();
                }

                using (StreamWriter writer = File.AppendText(arquivoLog))
                {
                    string linha = conteudo;
                    writer.WriteLine(linha, true);
                }
            }
            catch (Exception x)
            {
                Print.Error("\n[library.system.print] Erro ao Gravar Arquivo Log!! - " + arquivoLog);
                throw new Exception(x.ToString());
            }
        }
    }
}
