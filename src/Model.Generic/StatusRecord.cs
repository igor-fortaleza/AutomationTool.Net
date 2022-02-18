using System.ComponentModel;

namespace Model.Generic
{
    public enum StatusRecord
    {
        [Description("")] Null,
        [Description("Com erro")] Erro,
        [Description("Não processado")] NaoProcessado,
        [Description("Processado")] Processado,
        [Description("Em espera")] EmEspera,
    }
}
