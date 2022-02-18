using System.ComponentModel;

namespace Library.WebDriver.Enum
{
    public enum TypeBrowser
    {
        [Description("")] Null,
        [Description("Internet Explorer")] IE,
        [Description("Microsoft Edge")] Edge,
        [Description("Mozilla Firefox")] Firefox,
        [Description("Google Chrome")] Chrome,
    }
}
