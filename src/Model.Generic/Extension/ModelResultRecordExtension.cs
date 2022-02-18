using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Model.Generic.Model;

namespace Model.Generic.Extension
{
    public static class ModelResultRecordExtension
    {
        public static void SetItemCsv<T>(this ModelRecordResult<T> result, string content)
        {
            result.Content = result.Content.Length > 0 ? result.Content + ";" + content : content;
        }

        public static void SetEndProcess<T>(
            this ModelRecordResult<T> result,
            StatusAutomation statusAutomation)
        {
            result.SetEndProcess<T>(statusAutomation, default(T), string.Empty);
        }

        public static void SetEndProcess<T>(
            this ModelRecordResult<T> result,
            StatusAutomation statusAutomation,
            string conteudo)
        {
            result.SetEndProcess<T>(statusAutomation, default(T), conteudo);
        }

        public static void SetEndProcess<T>(
            this ModelRecordResult<T> result,
            StatusAutomation statusAutomation,
            T retorno)
        {
            result.SetEndProcess<T>(statusAutomation, retorno, string.Empty);
        }

        public static void SetEndProcess<T>(
            this ModelRecordResult<T> result,
            StatusAutomation statusAutomation,
            T retorno,
            string conteudo)
        {
            result.StatusAutomation = statusAutomation;
            result.Return = retorno;
            result._Watch.Stop();
            result.TimeExecution = result._Watch.ElapsedMilliseconds;
            if (conteudo.Length <= 0)
                return;
            result.Content = conteudo;
        }
    }
}
