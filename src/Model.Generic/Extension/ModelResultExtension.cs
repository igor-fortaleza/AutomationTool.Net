using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Model.Generic.Model;

namespace Model.Generic.Extension
{
    public static class ModelResultExtension
    {
        public static void SetError<T>(this ModelResult<T> result, string mensagemErro)
        {
            result.SetError<T>((Exception)null, mensagemErro, default(T));
        }

        public static void SetError<T>(this ModelResult<T> result, string mensagemErro, T retorno)
        {
            result.SetError<T>((Exception)null, mensagemErro, retorno);
        }

        public static void SetError<T>(this ModelResult<T> result, Exception ex, string CorpoMsgErro)
        {
            result.SetError<T>(ex, CorpoMsgErro, default(T));
        }

        public static void SetError<T>(
            this ModelResult<T> result,
            Exception ex,
            string CorpoMsgErro,
            T retorno)
        {
            result.ProcessOk = false;
            result.Exception = ex;
            result.MsgError = ex != null ? CorpoMsgErro + ex.Message : CorpoMsgErro;
            result.Return = retorno;
            result.StopWatchModelResult<T>();
        }

        public static void SetProcessOK<T>(this ModelResult<T> result, T retorno)
        {
            result.ProcessOk = true;
            result.Return = retorno;
            result.Exception = (Exception)null;
            result.MsgError = string.Empty;
            result.StopWatchModelResult<T>();
        }

        private static void StopWatchModelResult<T>(this ModelResult<T> result)
        {
            result._Watch.Stop();
            result.TimeExecution = result._Watch.ElapsedMilliseconds;
        }
    }
}
