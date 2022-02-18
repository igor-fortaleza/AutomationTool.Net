using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Model.Generic.Model;

namespace Model.Generic.Extension
{
    public static class ValidadorExtension
    {
        public static ModelValidation ValidateModel(this object model)
        {
            List<ValidationResult> validationResultList = new List<ValidationResult>();
            bool pIsValid = false;
            string empty = string.Empty;
            string pErrosMsg;
            if (model != null)
            {
                try
                {
                    ValidationContext validationContext = new ValidationContext(model, (IServiceProvider)null, (IDictionary<object, object>)null);
                    Validator.TryValidateObject(model, validationContext, (ICollection<ValidationResult>)validationResultList, true);
                    pIsValid = validationResultList.Count < 1;
                    pErrosMsg = ValidadorExtension.ConvertValidationResul(validationResultList);
                }
                catch (Exception ex)
                {
                    pErrosMsg = "Erro na validação dos campos: " + ex.Message;
                }
            }
            else
                pErrosMsg = "Parâmetros incorretos";
            return new ModelValidation(pIsValid, validationResultList, pErrosMsg);
        }

        private static string ConvertValidationResul(List<ValidationResult> resultValidation)
        {
            string empty = string.Empty;
            if (resultValidation.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ValidationResult validationResult in resultValidation)
                    stringBuilder.AppendLine(string.Concat(validationResult.MemberNames) + ": " + validationResult.ErrorMessage);
                empty = stringBuilder.ToString();
            }
            return empty;
        }
    }
}
