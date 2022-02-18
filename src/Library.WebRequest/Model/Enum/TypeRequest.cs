using System.ComponentModel;

namespace Library.WebRequest.Model.Enum
{
    public enum TypeRequest
    {
        [Description("")] Null,
        [Description("GET")] Get,
        [Description("POST")] Post,
        [Description("HEAD")] Head,
        [Description("PUT")] Put,
        [Description("DELETE")] Delete,
    }
}
