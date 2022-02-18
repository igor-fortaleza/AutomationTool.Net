using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Library.System
{
    public static class SystemTool
    {
        private static string nameFileLogNow = null;

        public static void WriteLog(string texto, string nameFile)
        {
            try
            {
                nameFileLogNow = nameFile;
                string pathLog = SystemConfigurate.GetPathLog();

                if (!string.IsNullOrEmpty(pathLog))
                    SaveLogTxt(texto, Path.Combine(pathLog, nameFile).ToString());
                else
                    Print.Error("Seu diretório não foi configurado ou não existe, use a função ConfiguratePathLog() e o SystemConfigurate corretamente");
            }
            catch
            {
                Print.Error("\n" + DateTime.Now + "Erro ao criar diretorio do LOG");
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
                Print.Error("\n" + DateTime.Now + " - Erro ao Gravar Arquivo Log!! - " + arquivoLog + " \n");
                throw new Exception(x.ToString());
            }
        }

        public static void RegisterLog(string texto, bool obrigatory = true)
        {
            try
            {
                nameFileLogNow = GetFileLog();
                if (!string.IsNullOrEmpty(nameFileLogNow))
                {
                    string pathLog = SystemConfigurate.GetPathLog();

                    if (!string.IsNullOrEmpty(pathLog))
                    {
                        SaveLogTxt(texto, Path.Combine(pathLog, nameFileLogNow).ToString());
                    }
                    else
                    {
                        Print.Error("Seu diretório não foi configurado ou não existe, use a função ConfiguratePathLog() e o SystemConfigurate corretamente");
                    }
                }
                else if (obrigatory)
                {
                    Print.Error("Impossivel de gravar log pois não foi denifido nenhum nome de arquivo");
                }
                else
                {
                    return;
                }
            }
            catch
            {
                Print.Error("\n" + DateTime.Now + " Erro ao criar diretorio do LOG");
            }
        }
        public static void RegisterLog(string texto, string getPathLog, string nomeArquivo)
        {
            try
            {
                if (!Directory.Exists(getPathLog))
                {
                    Directory.CreateDirectory(getPathLog);
                }

                SaveLogTxt(texto, Path.Combine(getPathLog, nomeArquivo).ToString());
            }
            catch (Exception)
            {
                Print.Error("\n" + DateTime.Now + "Erro ao criar diretorio do LOG");
            }
        }

        //public static void RegisterLog(bool active)
        //{
        //    SystemConfigurate.registerLog = active;
        //}

        public static FileInfo WaitFile(string directory, string partialFileName, string extension = null, int sleepMilliseconds = 150000)
        {
            var dir = new DirectoryInfo(directory);
            var timer = new Stopwatch();

            FileInfo file = dir.GetFiles().FirstOrDefault(x => x.Name.ToLowerInvariant().Contains(partialFileName.ToLowerInvariant()));

            timer.Start();
            while (file == null && timer.ElapsedMilliseconds < sleepMilliseconds)
            {
                file = dir.GetFiles().FirstOrDefault(x => x.Name.ToLowerInvariant().Contains(partialFileName.ToLowerInvariant()) && extension == null ? true : x.Extension.Contains(".csv"));
            }
            timer.Stop();

            return file;
        }

        public static void DeleteFiles(string nameFiles, string dir)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] Files = d.GetFiles(nameFiles);

                foreach (FileInfo file in Files)
                {
                    file.Delete();
                }
            }
            catch
            {
                Print.Error("Não foi possivel deletar " + nameFiles + " no diretório " + dir);
            }
        }
        public static void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }

        public static string GetFileLog()
        {
            if (string.IsNullOrEmpty(SystemConfigurate.NameFileLog))
            {
                return string.Empty;
            }
            return SystemConfigurate.NameFileLog;
        }

        public static void KillProcess(string processName)
        {
            Print.Message("\n[System.Library] Finalizando o processo" + processName);
            try
            {
                Process.GetProcessesByName(processName)
                    .ToList()
                    .ForEach(process => process.Kill());
            }
            catch (Exception x)
            {
                Print.Error("\n[System.Library] Não foi possivel finalizar o processo " + processName + "\n\n" + x);
                throw;
            }
        }
        public static void Taskkill(string programExe, bool inUserNameLocal = false)
        {
            Print.Message($"[System.Library] Derrubando o processo {programExe}");

            if (inUserNameLocal)
                Process.Start(@"taskkill", string.Format(@"/im {0} {1}", programExe, " /f")).WaitForExit();
            else
                Process.Start(@"taskkill", $"/F /FI \"USERNAME eq %username% \" /IM {programExe}").WaitForExit();
        }

        public static void Taskkill(string programExe, string userName)
        {
            Print.Message($"[System.Library] Derrubando o processo {programExe} no usuario {userName}");
            Process.Start(@"taskkill", $"/F /IM {programExe} /FI \"username eq {userName}\"").WaitForExit();
        }

        public static void ExecuteProcess(string processFile)
        {
            Print.Message($"[Library.System] Iniciando processo {processFile}");
            Process.Start(processFile);
        }

        public static Boolean AlreadyRunning()
        {
            Process[] thisnameprocesslist;
            string modulename, processname;
            Process p = Process.GetCurrentProcess();
            modulename = p.MainModule.ModuleName.ToString();
            processname = Path.GetFileNameWithoutExtension(modulename);
            thisnameprocesslist = Process.GetProcessesByName(processname);

            if (thisnameprocesslist.Length > 1)
            {
                Print.Message("[System.Library] Já existe um processo igual aberto.");
                return false;
            }

            return true;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}
