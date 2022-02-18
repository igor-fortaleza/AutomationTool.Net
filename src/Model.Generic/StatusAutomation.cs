using System.ComponentModel;

namespace Model.Generic
{
    public enum StatusAutomation
    {
        [Description("")] Null,
        [Description("Erro Genérico")] ErroGenerico,
        [Description("OK")] Ok,
        [Description("Continuacao")] Continuacao,
        [Description("Usuário ou senha incorreto")] UsuarioSenhaErrado,
        [Description("Usuário Bloqueado")] UsuarioBloqueado,
        [Description("Senha Expirada")] SenhaExpirada,
        [Description("Site Fora")] SiteFora,
        [Description("Captcha Inválido")] CaptchaErro,
        [Description("Sessão Expirada")] SessaoExpirada,
        [Description("Sessão Derrubada")] SessaoDerrubada,
        [Description("Parâmetros Inválidos")] ParametrosInvalido,
        [Description("Usuário não Autorizado")] UsuarioNaoAutorizado,
        [Description("Erro no download do arquivo")] ErroDownloadArquivo,
        [Description("Erro interno da aplicação")] ErroInternoAplicacao,
        [Description("Erro ao buscar dados de acesso")] ErroBuscarDadosAcesso,
    }
}
