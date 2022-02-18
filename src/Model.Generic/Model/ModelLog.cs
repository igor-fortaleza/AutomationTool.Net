using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Model
{
    public class ModelLog
    {
        [Key]
        [Display(Name = "Id do Log")]
        public long IdLog { get; set; }

        [Key]
        [Display(Name = "Id do Usuario")]
        public Guid IdUser { get; set; } = Guid.Empty;

        [Display(Name = "Id da Automação")]
        public Guid IdAutomation { get; set; } = Guid.Empty;

        [Display(Name = "Id da Rotina")]
        public long? IdRoutine { get; set; } = new long?(0L);

        [Display(Name = "Tipo de Log")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo de Log é obrigatório")]
        [StringLength(1, ErrorMessage = "O tamanho máximo é 1 caractere")]
        public string TypeLog { get; set; } = string.Empty;

        [Display(Name = "Mensagem do Log")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mensagem do Log é obrigatório")]
        public string MsgLog { get; set; } = string.Empty;
    }
}
