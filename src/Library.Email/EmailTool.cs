using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Library.System;
using Library.TextHelp.RegularExpression;
using Model.Generic.Model;

namespace Library.Email
{
    public class EmailTool : IDisposable
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        private readonly Provider _provider;

        private MailMessage _mail;
        private SmtpClient _client;

        public EmailTool(Provider provider, string senderEmail, string senderPassword)
        {
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
            _provider = provider;

            Inicialize();
        }
        
        private void Inicialize()
        {
            if (_senderEmail.ValidEmailAddress())            
                ConfigureClient();            
            else
            {
                Print.Error("*[Library.Email] Email configurado é invalido");
                throw new Exception("*[Library.Email] Email configurado é invalido");
            }
        }

        public void Send(string recipient, string subject, string bodyMensagem, List<FileInfo> files = (List<FileInfo>)null)
        {
            try
            {
                _mail = new MailMessage(_senderEmail, recipient) { Subject = subject, Body = bodyMensagem };
                ValidEmailRecipient(recipient);                
                IndexFile(files);

                this._client.Send(_mail);
            }

            catch (Exception ex)
            {
                Print.Error($"*[Library.Email] Erro ao enviar email para: {recipient}");
                throw new Exception(ex.Message + ex.InnerException);
            }
            finally
            {
                Print.Sucess("[Library.Email] ** Email enviado **");
            }
        }

        public void Send(string recipient, string subject, string bodyMensagem, FileInfo file = (FileInfo)null)
        {
            try
            {
                _mail = new MailMessage(_senderEmail, recipient) { Subject = subject, Body = bodyMensagem };
                ValidEmailRecipient(recipient);                
                IndexFile(file);                

                this._client.Send(_mail);
            }
            catch (Exception ex)
            {
                Print.Error($"*[Library.Email] Erro ao enviar email para: {recipient}");
                throw new Exception(ex.Message.ToString() + ex.InnerException.ToString());
            }
            finally
            {
                Print.Sucess("[Library.Email] ** Email enviado **");
            }
        }

        private void IndexFile(List<FileInfo> files)
        {
            if (files != null)
            {
                foreach (FileInfo file in files)
                {
                    ValidFile(file);
                    AddAttachments(new ModelDataAdditional()
                    {
                        DadoAdicional = File.ReadAllBytes(file.FullName),
                        Nome = file.FullName.Split('\\').LastOrDefault()
                    });
                };
            }
        }

        private void IndexFile(FileInfo file)
        {
            if (file != null)
            {
                ValidFile(file);
                AddAttachments(new ModelDataAdditional()
                {
                    DadoAdicional = File.ReadAllBytes(file.FullName),
                    Nome = file.FullName.Split('\\').LastOrDefault()
                });
            }
        }

        private void ValidFile(FileInfo file)
        {
            if (!File.Exists(file.FullName))
            {
                string error = $"*[Library.Email] O arquivo \"{file.FullName}\" não foi encontrado para envio!";
                Print.Error(error);
                throw new Exception(error);
            }
        }

        private void AddAttachments(ModelDataAdditional attachment)
        {
            var ms = new MemoryStream(attachment.DadoAdicional);
            this._mail.Attachments.Add(new Attachment(ms, attachment.Nome));
        }

        private void ValidEmailRecipient(string recipient)
        {
            if (recipient.ValidEmailAddress()) return;
            string error = $"*[Library.Email] Erro ao enviar Email - Endereço \"{recipient}\" inválido!";
            Print.Error(error);
            throw new Exception(error);
        }

        private void ConfigureClient()
        {
            this._client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };

            switch (_provider)
            {
                case Provider.Gmail:
                    SetClientProvider(465, "smtp.gmail.com");
                    break;
                case Provider.Hotmail:
                    SetClientProvider(587, "smtp.office365.com");    
                    break;
                case Provider.Outlook:
                    SetClientProvider(587, "smtp.office365.com");
                    break;
                case Provider.Icloud:
                    SetClientProvider(465, "smtp.aol.com");
                    break;
                case Provider.MSN:
                    SetClientProvider(587, "smtp-mail.outlook.com");
                    break;
                case Provider.Yahoo:
                    SetClientProvider(465, "smtp.mail.yahoo.com");
                    break;
                case Provider.OUL:
                    SetClientProvider(587, "smtp.mail.me.com");
                    break;
                default:
                    throw new Exception("Nenhum provedor configurado!");
            }

            this._client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
        }

        private void SetClientProvider(int port, string host)
        {
            this._client.Port = port;
            this._client.Host = host;
        }

        public void Dispose()
        {
            this._mail.Dispose();
            this._client.Dispose();
        }
    }

}
