using System.ComponentModel;

namespace Library.WebDriver.Enum
{
    public enum TypeEvent
    {
        [Description("")] Null,
        [Description("change")] OnChange,
        [Description("click")] OnClick,
        [Description("keydown")] OnKeyDown,
        [Description("keyup")] OnKeyUp,
        [Description("load")] OnLoad,
        [Description("blur")] OnBlur,
        [Description("focus")] OnFocus,
        [Description("submit")] OnSubmit,
        [Description("keypress")] OnKeyPress,
        [Description("select")] OnSelect,
        [Description("input")] OnInput,
    }
}
