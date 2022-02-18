using Library.WebRequest.Model.Enum;
using Model.Captcha;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.TwoCaptcha
{
    public class TwoCaptchaValidations
    {
        public void ValidateResponse(string response, ModelCaptcha model, TypeRequest request)
        {
            if (response.Length > 0)
            {
                string[] strArray = response.Split('|');
                model.StatusSolicitation = strArray[0];

                if (request.Equals((object)TypeRequest.Post))
                {
                    this.ValidateStatusPost(model);
                    if (strArray.Length <= 1)
                        return;
                    model.IdSolicitation = strArray[1];
                }
                else
                {
                    this.ValidateStatusGet(model);
                    if (strArray.Length <= 1)
                        return;
                    model.CaptchaResolved = strArray[1];
                }
            }
            else
                model.Validation.MsgError = "O response não tem um conteúdo válido, o Post ou Get apresentaram problemas";
        }

        private void ValidateStatusGet(ModelCaptcha model)
        {
            switch (model.StatusSolicitation)
            {
                case "CAPCHA_NOT_READY":
                    model.Validation.ReTry = true;
                    break;
                case "ERROR_BAD_DUPLICATES":
                    model.Validation.MsgError = "ERROR_BAD_DUPLICATES - O erro é retornado quando o recurso de precisão de 100% está ativado. O erro significa que o número máximo de tentativas é atingido, mas o número mínimo de correspondências não foi encontrado. Você pode tentar enviar seu captcha novamente.";
                    break;
                case "ERROR_CAPTCHA_UNSOLVABLE":
                    model.Validation.MsgError = "ERROR_CAPTCHA_UNSOLVABLE - Não conseguimos resolver o seu captcha - três dos nossos trabalhadores não conseguiram resolvê-lo ou não obtivemos uma resposta dentro de 90 segundos (300 segundos para o ReCaptcha V2). Não cobraremos você por esse pedido. Você pode tentar enviar o seu captcha.";
                    break;
                case "ERROR_KEY_DOES_NOT_EXIST":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_KEY_DOES_NOT_EXIST - A chave que você forneceu não existe. Pare de enviar solicitações. Verifique sua chave de API.";
                    break;
                case "ERROR_WRONG_CAPTCHA_ID":
                    model.Validation.MsgError = "ERROR_WRONG_CAPTCHA_ID - Você forneceu um ID de captcha incorreto. Verifique o ID do captcha ou seu código que recebe o ID.";
                    break;
                case "ERROR_WRONG_ID_FORMAT":
                    model.Validation.MsgError = "ERROR_WRONG_ID_FORMAT - Você forneceu ID de captcha no formato errado. O ID pode conter apenas números. Verifique o ID do captcha ou seu código que recebe o ID.";
                    break;
                case "ERROR_WRONG_USER_KEY":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_WRONG_USER_KEY - Você forneceu o valor do parâmetro-chave no formato incorreto, ele deve conter 32 símbolos. Pare de enviar solicitações. Verifique sua chave de API.";
                    break;
                case "OK":
                    break;
                case "REPORT_NOT_RECORDED":
                    model.Validation.MsgError = "REPORT_NOT_RECORDED - O erro é retornado quando você já solicitou diversas vezes captchas já resolvidos.";
                    break;
                default:
                    this.ValidateStatusCodBlock(model);
                    break;
            }
        }

        private void ValidateStatusPost(ModelCaptcha model)
        {
            switch (model.StatusSolicitation)
            {
                case "ERROR_BAD_TOKEN_OR_PAGEURL":
                    model.Validation.MsgError = "ERROR_BAD_TOKEN_OR_PAGEURL - Você pode obter este código de erro ao enviar o ReCaptcha V2. Isso acontece se sua solicitação contiver um par inválido de googlekey e pageurl. A razão comum para isso é que o ReCaptcha é carregado dentro de um iframe hospedado em outro domínio / subdomínio";
                    break;
                case "ERROR_CAPTCHAIMAGE_BLOCKED":
                    model.Validation.MsgError = "ERROR_CAPTCHAIMAGE_BLOCKED - Você enviou uma imagem marcada em nosso banco de dados como irreconhecível. Geralmente isso acontece se o site onde você encontrou o captcha parou de enviar captchas e começou a enviar a imagem \"negar acesso\" .Tente substituir as limitações do site.";
                    break;
                case "ERROR_IMAGE_TYPE_NOT_SUPPORTED":
                    model.Validation.MsgError = "ERROR_IMAGE_TYPE_NOT_SUPPORTED - O servidor não pode reconhecer o tipo de arquivo de imagem. Verifique o arquivo de imagem.";
                    break;
                case "ERROR_IP_NOT_ALLOWED":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_IP_NOT_ALLOWED - A solicitação é enviada do IP que não está na lista de seus IPs permitidos.";
                    break;
                case "ERROR_KEY_DOES_NOT_EXIST":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_KEY_DOES_NOT_EXIST - A chave que você forneceu não existe. Pare de enviar solicitações. Verifique sua chave de API.";
                    break;
                case "ERROR_NO_SLOT_AVAILABLE":
                    model.Validation.TimeBlock = 3000;
                    model.Validation.ReTry = true;
                    model.Validation.MsgError = "ERROR_NO_SLOT_AVAILABLE - Você pode receber este erro em dois casos: 1.Se você resolver ReCaptcha: a fila de seus captchas que não são distribuídos para os trabalhadores é muito longa.O limite de filas muda dinamicamente e depende da quantidade total de captchas que aguardam solução e geralmente está entre 50 e 100 captchas. 2.Se você resolver Captcha Normal: sua taxa máxima para captchas normais é menor que a taxa atual no servidor. Você pode alterar sua taxa máxima nas configurações da sua conta.Se você recebeu este erro, não tente enviar sua solicitação novamente imediatamente.Faça um tempo limite de 2 a 3 segundos e tente novamente para enviar sua solicitação.";
                    break;
                case "ERROR_PAGEURL":
                    model.Validation.MsgError = "ERROR_PAGEURL - O parâmetro pagurl está ausente em sua solicitação. Pare de enviar solicitações e altere seu código para fornecer um parâmetro de pagurl válido.";
                    break;
                case "ERROR_TOO_BIG_CAPTCHA_FILESIZE":
                    model.Validation.MsgError = "ERROR_TOO_BIG_CAPTCHA_FILESIZE - O tamanho da imagem é superior a 100 kB. Verifique o arquivo de imagem.";
                    break;
                case "ERROR_UPLOAD":
                    model.Validation.MsgError = "ERROR_UPLOAD - O servidor não pode obter dados do arquivo da sua solicitação POST. Isso acontece se sua solicitação POST for malformada ou se os dados base64 não forem válidos para a base64.Você tem que consertar seu código que faz o pedido do POST.";
                    break;
                case "ERROR_WRONG_FILE_EXTENSION":
                    model.Validation.MsgError = "ERROR_WRONG_FILE_EXTENSION - O arquivo de imagem tem extensão não suportada. Extensões aceitas: jpg, jpeg, gif, png. Verifique o arquivo de imagem.";
                    break;
                case "ERROR_WRONG_USER_KEY":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_WRONG_USER_KEY - Você forneceu o valor do parâmetro-chave no formato incorreto, ele deve conter 32 símbolos. Pare de enviar solicitações. Verifique sua chave de API.";
                    break;
                case "ERROR_ZERO_BALANCE":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "ERROR_ZERO_BALANCE - Você não tem fundos na sua conta. Pare de enviar solicitações. Deposite sua conta para continuar resolvendo captchas.";
                    break;
                case "ERROR_ZERO_CAPTCHA_FILESIZE":
                    model.Validation.MsgError = "ERROR_ZERO_CAPTCHA_FILESIZE - O tamanho da imagem é inferior a 100 bytes. Verifique o arquivo de imagem.";
                    break;
                case "IP_BANNED":
                    model.Validation.Impediment = true;
                    model.Validation.MsgError = "IP_BANNED - Seu endereço IP é proibido devido a muitas tentativas freqüentes de acessar o servidor usando chaves de autorização erradas.";
                    break;
                case "MAX_USER_TURN":
                    model.Validation.TimeBlock = 10000;
                    model.Validation.ReTry = true;
                    model.Validation.MsgError = "MAX_USER_TURN - Você fez mais de 60 solicitações para in.php dentro de 3 segundos. Sua conta é banida por 10 segundos.Banimento será levantado automaticamente. Definir pelo menos 100 ms timeout entre as solicitações para in.php";
                    break;
                case "OK":
                    break;
                default:
                    this.ValidateStatusCodBlock(model);
                    break;
            }
        }

        private void ValidateStatusCodBlock(ModelCaptcha model)
        {
            string statusSolicitation = model.StatusSolicitation;

            if (statusSolicitation != "ERROR: 1001")
            {
                if (statusSolicitation != "ERROR: 1002")
                {
                    if (statusSolicitation != "ERROR: 1003")
                    {
                        if (statusSolicitation != "ERROR: 1004")
                        {
                            if (statusSolicitation == "ERROR: 1005")
                            {
                                model.Validation.TimeBlock = 300000;
                                model.Validation.MsgError = "ERROR: 1005 - Tempo de bloqueio 5 minutos. Você está fazendo muitas solicitações para res.php para obter respostas. Tempo de bloqueio 5 minutos. Você está fazendo muitas solicitações para res.php para obter respostas. Isso significa que você não precisa fazer mais de 20 solicitações para res.php por cada captcha.";
                            }
                            else
                                model.Validation.MsgError = model.StatusSolicitation + " - Código de erro não identificada, por favor consultar a API do 2Captcha";
                        }
                        else
                        {
                            model.Validation.TimeBlock = 600000;
                            model.Validation.MsgError = "ERROR: 1004 - Tempo de bloqueio 10 minutos. Seu endereço IP está bloqueado porque houve 5 solicitações com chave de API incorreta do seu IP.";
                        }
                    }
                    else
                    {
                        model.Validation.TimeBlock = 30000;
                        model.Validation.MsgError = "ERROR: 1003 - Tempo de bloqueio de 30 segundos. Você está recebendo ERROR_NO_SLOT_AVAILABLE porque está fazendo upload de muitos captchas e o servidor tem uma longa fila de seus captchas que não são distribuídos aos trabalhadores. Você recebeu três vezes mais erros do que a quantidade de captchas que você enviou(mas não menos de 120 erros).Aumente o tempo limite se você vir esse erro.";
                    }
                }
                else
                {
                    model.Validation.TimeBlock = 300000;
                    model.Validation.MsgError = "ERROR: 1002 - Tempo de bloqueio 5 minutos. Você recebeu 120 erros de ERROR_ZERO_BALANCE em um minuto porque seu saldo está fora";
                }
            }
            else
            {
                model.Validation.TimeBlock = 600000;
                model.Validation.MsgError = "ERROR: 1001 - Tempo de bloqueio 10 minutos. Você recebeu 120 erros de ERROR_NO_SLOT_AVAILABLE em um minuto porque seu lance atual é menor do que o lance atual no servidor";
            }
        }
    }
}
