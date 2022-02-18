using System.ComponentModel;

namespace Model.Generic
{
    public enum TypeFile
    {
        [Description("")] Null,
        [Description(".html")] Html,
        [Description(".jpeg")] Jpeg,
        [Description(".pdf")] Pdf,
        [Description(".json")] Json,
        [Description(".gif")] Gif,
        [Description(".png")] Png,
        [Description(".js")] JavaScript,
        [Description(".css")] Css,
        [Description(".txt")] Txt,
        [Description(".zip")] Zip,
    }
}
