using System.ComponentModel;

namespace Library.WebDriver.Enum
{
    public enum TypePopupAction
    {
        [Description("")] Null,
        [Description("Open Popup")] Open,
        [Description("Close Popup")] Close,
        [Description("Close All Popups")] CloseAll,
    }
}
