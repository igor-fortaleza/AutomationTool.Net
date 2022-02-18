using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Attributes
{
    public class RangeNullValidationAttribute : ValidationAttribute
    {
        private readonly bool _PermitirNull;
        private readonly double _Minimum;
        private readonly double _Maximum;

        public RangeNullValidationAttribute(bool permitirNull = false, double minimum = -1.79769313486232 + 308, double maximum = 1.79769313486232 + 308)
        {
            if (this.ErrorMessage == null || this.ErrorMessage.Length < 1)
                this.ErrorMessage = "O valor é inválido";
            this._PermitirNull = permitirNull;
            this._Minimum = minimum;
            this._Maximum = maximum;
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            ValidationResult validationResult;
            if (value == null)
            {
                validationResult = this._PermitirNull ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
            }
            else
            {
                double num = (double)value;
                validationResult = num >= this._Minimum ? (num <= this._Maximum ? ValidationResult.Success : new ValidationResult(this.ErrorMessage + " - O valor informado não pode ser maior que " + (object)this._Maximum)) : new ValidationResult(this.ErrorMessage + " - O valor informado não pode ser menor que " + (object)this._Minimum);
            }
            return validationResult;
        }
    }
}
