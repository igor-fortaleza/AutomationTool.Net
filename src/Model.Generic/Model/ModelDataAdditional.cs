using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Model
{
    public class ModelDataAdditional
    {
        public long IdDadoAdicional { get; set; }

        [Display(Name = "Tipo de dado adicional")]
        public TypeFile TipoDado { get; set; }

        [Display(Name = "Dado adicional")]
        public byte[] DadoAdicional { get; set; } = new byte[0];

        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

    }
}
