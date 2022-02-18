using System.ComponentModel.DataAnnotations;

namespace Model.Generic.Attributes
{
    public class ListAnyValidationAttribute : ValidationAttribute
    {
        public readonly int _Minimum;
        public readonly int _Maximum;

        public ListAnyValidationAttribute(int minimum = 0, int maximum = 0)
        {

            this._Minimum = minimum < 0 ? 0 : minimum;
            this._Maximum = maximum < 1 ? int.MaxValue : maximum;
            if (this.ErrorMessage != null && this.ErrorMessage.Length >= 1)
                return;
            string str;
            if (this._Minimum <= 0 && this._Maximum == int.MaxValue)
                str = "A lista está vazia ou não segue os criterios de no Mínimo" + (object)this._Minimum + " e no Máximo " + (object)this._Maximum;
            else
                str = "A lista está vazia";
            this.ErrorMessage = str;
        }

        //protected override ValidationResult IsValid(
        //    object value,
        //    ValidationContext validationContext)
        //{
        //    ValidationResult validationResult;
        //    if (value == null)
        //    {
        //        validationResult = new ValidationResult(this.ErrorMessage);
        //    }
        //    else
        //    {
        //        //if (this.ErrorMessage == null)
        //        //{
        //        //    var variable = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(ListAnyValidationAttribute)));
        //        //}
        //        //// ISSUE: reference to a compiler-generated field
        //        //Func<CallSite, object, int> target = ListAnyValidationAttribute.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
        //        //// ISSUE: reference to a compiler-generated field
        //        //CallSite<Func<CallSite, object, int>> p1 = ListAnyValidationAttribute.;
        //        //// ISSUE: reference to a compiler-generated field
        //        //if (ErrorMessage == null)
        //        //{
        //        //    // ISSUE: reference to a compiler-generated field
        //        //    var variable = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(
        //        //        CSharpBinderFlags.None, "Count", typeof(ListAnyValidationAttribute),
        //        //        (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        //        //        {
        //        //            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        //        //        }));
        //        //}

        //        //// ISSUE: reference to a compiler-generated field
        //        //// ISSUE: reference to a compiler-generated field
        //        //object obj = variable.Target((CallSite)ListAnyValidationAttribute.\u003C\u003Eo__3.\u003C\u003Ep__0, value);
        //        //int num = target((CallSite)p1, obj);
        //        //validationResult = num < this._Minimum || num > this._Maximum ? new ValidationResult(this.ErrorMessage) : ValidationResult.Success;
        //    }
        //    validationResult = null;
        //    return validationResult;
        //}
    }
}
