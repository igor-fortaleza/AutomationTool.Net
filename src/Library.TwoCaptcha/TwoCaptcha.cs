using Library.Useful;
using Library.WebRequest;
using Library.WebRequest.Model.Enum;
using Model.Captcha;
using Model.Captcha.Enum;
using Model.Generic.Extension;
using Model.Generic.Model;
using System.Collections.Generic;
using System.Threading;

namespace Library.TwoCaptcha
{
        public class TwoCaptcha : TwoCaptchaBase
        {
            private readonly TwoCaptchaValidations _Validations = new TwoCaptchaValidations();
            private readonly TimerRequest _timer = new TimerRequest(60, 3000);
            private readonly Dictionary<string, StatusUserCaptcha> _Blocked;

            public TwoCaptcha(int totalAttempts, int waitTimeAttempts)
            {
                this.SetAttemps(totalAttempts, waitTimeAttempts);
                this._Blocked = new Dictionary<string, StatusUserCaptcha>();
            }

            private int _TotalAttempts { get; set; }

            private int _WaitTimeAttempts { get; set; }

            public void SetAttemps(int totalAttempts, int waitTimeAttempts)
            {
                this._WaitTimeAttempts = waitTimeAttempts;
                this._TotalAttempts = totalAttempts;
            }

            public void SetBloqueado(string idUsuario, StatusUserCaptcha status)
            {
                lock (_Blocked)
                {
                    if (_Blocked.ContainsKey(idUsuario))
                        return;
                    _Blocked.Add(idUsuario, status);
                }
            }

            public void RemoveBloqueado(string idUsuario)
            {
                lock (_Blocked)
                {
                    if (!_Blocked.ContainsKey(idUsuario))
                        return;
                    _Blocked.Remove(idUsuario);
                }
            }

            public ModelResult<ModelCaptcha> QuebrarCaptcha(
              string key,
              string captchaCodificada,
              TypeCaptcha tipo,
              string site,
              string minScore,
              string action)
            {
                _timer.VerificaTemporizador();
                ModelResult<ModelCaptcha> modelResult = new ModelResult<ModelCaptcha>();
                Request request = GetRequest();
                ModelCaptcha model = new ModelCaptcha(key, string.Empty, captchaCodificada, tipo, site, minScore, action);
                _Validations.ValidateResponse(request.Post("in.php", GetParametersPost(model)), model, TypeRequest.Post);

                if (!model.Validation.Impediment && !model.Validation.ReTry && model.Validation.MsgError.Length < 1)
                {
                    Thread.Sleep(5000);
                    WaitBreak(request, model);
                }

                modelResult.SetProcessOK<ModelCaptcha>(model);
                return modelResult;
            }
            private void WaitBreak(Request request, ModelCaptcha model)
            {
                do
                {
                    model.Validation.ReTry = false;
                    StatusUserCaptcha statusUsuarioCaptcha;
                    model.Impediment = this._Blocked.TryGetValue(model.IdUser, out statusUsuarioCaptcha) ? statusUsuarioCaptcha : StatusUserCaptcha.Null;

                    if (model.Impediment.Equals((object)(StatusUserCaptcha.Null)))
                    {
                        model.SetTotalAttempts();
                        _Validations.ValidateResponse(request.Get("res.php?key=" + model.IdUser + "&action=get&id=" + model.IdSolicitation), model, TypeRequest.Get);
                        if (model.Validation.ReTry)
                            Thread.Sleep(this._WaitTimeAttempts);
                    }
                    else
                        goto label_4;
                }
                while (model.Validation.ReTry && model.TotalAttempts < this._TotalAttempts && (!model.Validation.Impediment && model.Validation.MsgError.Length < 1));
                goto label_5;
            label_4:
                return;
            label_5:;
            }

            private string GetParametersPost(ModelCaptcha model)
            {
                string str;
                if (model.TypeCaptcha.Equals((object)(TypeCaptcha.Normal)))
                    str = "method=base64&key=" + model.IdUser + "&body=" + model.CaptchaCoded;
                else if (model.TypeCaptcha.Equals((object)(TypeCaptcha.ReCaptchaV3)))
                    str = "key=" + model.IdUser + "&method=userrecaptcha&googlekey=" + model.CaptchaCoded + "&pageurl=" + model.Site + "&version=v3&action=" + model.Action + "&min_score=" + model._MinScore;
                else
                    str = "key=" + model.IdUser + "&method=userrecaptcha&googlekey=" + model.CaptchaCoded + "&pageurl=" + model.Site + "&here=now";
                return str;
            }
        }
    }
