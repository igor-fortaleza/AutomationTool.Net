using System.ComponentModel;

namespace Library.Email
{
    public enum Provider
    {
        [Description("Outlook")] Outlook,
        [Description("Gmail")] Gmail,
        [Description("Hotmail")] Hotmail,
        [Description("Icoud")] Icloud,
        [Description("MSN")] MSN,
        [Description("Yahoo")] Yahoo,
        [Description("UOL")] OUL
    }
}