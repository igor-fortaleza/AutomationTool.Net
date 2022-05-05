using Model.Generic;

namespace Library.Useful
{
    public class UsefulWeb
    {
        public string GetContentType(TypeFile tipo)
        {
            string empty = string.Empty;
            string str;
            switch (tipo)
            {
                case TypeFile.Html:
                    str = "text/html";
                    break;
                case TypeFile.Jpeg:
                    str = "image/jpeg";
                    break;
                case TypeFile.Pdf:
                    str = "application/pdf";
                    break;
                case TypeFile.Json:
                    str = "application/json";
                    break;
                case TypeFile.Gif:
                    str = "image/gif";
                    break;
                case TypeFile.Png:
                    str = "image/png";
                    break;
                case TypeFile.JavaScript:
                    str = "application/javascript";
                    break;
                case TypeFile.Css:
                    str = "text/css";
                    break;
                case TypeFile.Zip:
                    str = "application/zip";
                    break;
                default:
                    str = "application/octet-stream";
                    break;
            }
            return str;
        }
    }
}
