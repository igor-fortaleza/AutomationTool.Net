using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Library.System
{
    public static class SystemConfigurate
    {
        private static string PathLog;
        public static bool registerLog = false;
        public static string NameFileLog;
        public static string PathFile;

        public static void ConfiguratePathLog(string path, string nameFile)
        {
            if (!Directory.Exists(path))
            {
                Print.Info("Criando diretório " + path);
                try
                {
                    Directory.CreateDirectory(path);
                    NameFileLog = nameFile;
                    PathLog = path;
                    PathFile = Path.Combine(PathLog, NameFileLog).ToString();
                }
                catch
                { 
                    Print.Error("Não foi possivel criar o diretório " + path);
                }
            }
            else
            {
                NameFileLog = nameFile;
                PathLog = path;
                PathFile = Path.Combine(PathLog, NameFileLog).ToString();
            }
        }

        public static string GetPathLog()
        {
            if (string.IsNullOrEmpty(PathLog))
            {
                Print.Error("Diretorio do Log não foi configurado! \n -- Chame a função ConfiguratePathLog e SystemConfigurate --");
                throw new Exception("Diretorio do Log não foi configurado!");
            }
            return PathLog;
        }
        public static string GetFileLog()
        {
            if (string.IsNullOrEmpty(NameFileLog))
            {
                return string.Empty;
            }
            return NameFileLog;
        }
    }
}
