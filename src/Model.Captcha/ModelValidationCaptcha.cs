using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Captcha
{
    public class ModelValidationCaptcha
    {
        public bool ReTry { get; set; }

        public bool Impediment { get; set; }

        public string MsgError { get; set; } = string.Empty;

        public int TimeBlock { get; set; }
    }
}
