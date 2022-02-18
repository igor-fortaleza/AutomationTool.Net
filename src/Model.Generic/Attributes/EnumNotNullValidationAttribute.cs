using System;
using System.ComponentModel.DataAnnotations;
using Model.Generic.Extension;

namespace Model.Generic.Attributes
{
    public class EnumNotNullValidationAttribute : ValidationAttribute
    {
        public readonly Type _Type;

        public EnumNotNullValidationAttribute(Type type)
        {
            if (this.ErrorMessage == null || this.ErrorMessage.Length < 1)
                this.ErrorMessage = "O valor é inválido";
            this._Type = type;
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            return value != null ? (((Enum)value).GetAttributeText(this._Type).ToUpperInvariant() != "NULL" ? ValidationResult.Success : new ValidationResult(this.ErrorMessage)) : new ValidationResult(this.ErrorMessage);
        }
    }
}
