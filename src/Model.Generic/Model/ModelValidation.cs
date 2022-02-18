using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Model
{
    public class ModelValidation
    {
        public ModelValidation(
            bool pIsValid,
            List<ValidationResult> pValidationResult,
            string pErrosMsg)
        {
            this._IsValid = pIsValid;
            this._ValidationResult = pValidationResult;
            this._ErrosMsg = pErrosMsg;
        }

        public bool IsValid => this._IsValid;

        public List<ValidationResult> ValidationResult => this._ValidationResult;

        public string ErrosMsg => this._ErrosMsg;

        private bool _IsValid { get; set; }

        private List<ValidationResult> _ValidationResult { get; set; }

        private string _ErrosMsg { get; set; }
    }
}
