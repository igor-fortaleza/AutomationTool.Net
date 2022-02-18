using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Captcha
{
    public class ModelServiceCaptcha
    {
        public ModelServiceCaptcha()
        {
        }

        public ModelServiceCaptcha(string process, string cpf, string date)
        {
            this.Process = process;
            this.CPF_CNPJ = cpf;
            this.BirthDate = date;
        }

        public string Process { get; set; } = "";

        public string CPF_CNPJ { get; set; } = "";

        public string BirthDate { get; set; } = "";

        public ModelCaptcha ModelCaptcha { get; set; }

        public string Captcha { get; set; } = "";
    }
}
