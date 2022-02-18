using System;
using System.ComponentModel.DataAnnotations;
using Model.Generic.Extension;

namespace Model.Generic.Attributes
{
    public class DateTimeBetweenValidationAttribute : ValidationAttribute
    {
        public readonly DateTime _MinValue;
        public readonly DateTime _MaxValue;

        public DateTimeBetweenValidationAttribute(DateTime minValue, DateTime maxValue)
        {
            if (this.ErrorMessage == null || this.ErrorMessage.Length < 1)
                this.ErrorMessage = "Data inválida";
            this._MinValue = minValue;
            this._MaxValue = maxValue;
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            ValidationResult validationResult;
            if (value == null)
            {
                validationResult = new ValidationResult(this.ErrorMessage);
            }
            else
            {
                DateTime dateTime = (DateTime)value;
                validationResult = !(dateTime > this._MinValue) || !(dateTime < this._MaxValue) ? new ValidationResult(this.ErrorMessage) : ValidationResult.Success;
            }
            return validationResult;
        }
    }
}
