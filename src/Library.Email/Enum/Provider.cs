using System.ComponentModel;

namespace Library.Email
{
    public enum Provider
    {
        [Description("Outlook")] Outlook,
        [Description("Gmail")] Gmail,
        [Description("Hotmail")] Hotmail,
        [Description("UOL")] Icloud,
        [Description("UOL")] MSN,
        [Description("UOL")] Yahoo,
        [Description("UOL")] OUL
    }
}