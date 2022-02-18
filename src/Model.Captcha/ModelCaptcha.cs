using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Captcha.Enum;

namespace Model.Captcha
{
    public class ModelCaptcha
    {
        public readonly string _IdUser;
        public readonly string _Password;
        public readonly string _CaptchaCoded;
        public readonly TypeCaptcha _TypeCaptcha;
        public readonly string _Site;
        public readonly string _MinScore;
        public readonly string _Action;

        public ModelCaptcha(
          string idUser,
          string password,
          string captchaCoded,
          TypeCaptcha typeCaptcha,
          string site,
          string minScore,
          string action)
        {
            this._IdUser = idUser;
            this._Password = password;
            this._CaptchaCoded = captchaCoded;
            this._TypeCaptcha = typeCaptcha;
            this._Site = site;
            this._MinScore = minScore;
            this._Action = action;
        }

        public string IdUser
        {
            get
            {
                return this._IdUser;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
        }

        public string CaptchaCoded
        {
            get
            {
                return this._CaptchaCoded;
            }
        }

        public TypeCaptcha TypeCaptcha
        {
            get
            {
                return this._TypeCaptcha;
            }
        }

        public string Site
        {
            get
            {
                return this._Site;
            }
        }

        public string MinScore
        {
            get
            {
                return this._MinScore;
            }
        }

        public string Action
        {
            get
            {
                return this._Action;
            }
        }

        public int TotalAttempts
        {
            get
            {
                return this._TotalAttempts;
            }
        }

        public StatusUserCaptcha Impediment { get; set; }

        public ModelValidationCaptcha Validation { get; set; } = new ModelValidationCaptcha();

        public string StatusSolicitation { get; set; } = string.Empty;

        public string IdSolicitation { get; set; } = string.Empty;

        public string CaptchaResolved { get; set; } = string.Empty;

        private int _TotalAttempts { get; set; }

        public void SetTotalAttempts()
        {
            ++this._TotalAttempts;
        }
    }
}
