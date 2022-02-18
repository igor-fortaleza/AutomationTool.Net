using System.ComponentModel;

namespace Model.Captcha.Enum
{
    public enum StatusUserCaptcha
    {
        [Description("")] Null,
        [Description("OK")] Ok,
        [Description("Bloqueado")] Blocked,
        [Description("Expirado")] TempBlocked,
    }
}
