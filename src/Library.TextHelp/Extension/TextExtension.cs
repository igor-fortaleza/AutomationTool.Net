using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Library.TextHelp
{
    public static class TextExtension
    {
        public static List<string> GetCaracteresRemoval()
        {
            return new List<string>()
      {
        "&#0;",
        "&#x0000;",
        "&#1;",
        "&#01;",
        "&#x0001;",
        "&#2;",
        "&#02;",
        "&#x0002;",
        "&#3;",
        "&#03;",
        "&#x0003;",
        "&#4;",
        "&#04;",
        "&#x0004;",
        "&#5;",
        "&#05;",
        "&#x0005;",
        "&#6;",
        "&#06;",
        "&#x0006;",
        "&#7;",
        "&#07;",
        "&#x0007;",
        "&#8;",
        "&#08;",
        "&#x0008;",
        "&#9;",
        "&#09;",
        "\\t",
        "\t",
        "&#x0009;",
        "&#10;",
        "&#x000a;",
        "\n",
        "\\n",
        "&#11;",
        "&#x000b;",
        "&#12;",
        "&#x000c;",
        "&#13;",
        "&#x000d;",
        "\r",
        "\\r",
        "&#14;",
        "&#x000e;",
        "&#15;",
        "&#x000f;",
        "&#16;",
        "&#x0010;",
        "&#17;",
        "&#x0011;",
        "&#18;",
        "&#x0012;",
        "&#19;",
        "&#x0013;",
        "&#20;",
        "&#x0014;",
        "&#21;",
        "&#x0015;",
        "&#22;",
        "&#x0016;",
        "&#23;",
        "&#x0017;",
        "&#24;",
        "&#x0018;",
        "&#25;",
        "&#x0019;",
        "&#26;",
        "&#x001a;",
        "&#27;",
        "&#x001b;",
        "&#28;",
        "&#x001c;",
        "&#29;",
        "&#x001d;",
        "&#30;",
        "&#x001e;",
        "&#31;",
        "&#x001f;",
        "&#32;",
        "&#x0020;",
        "\\s",
        "&#160;",
        "&#x00a0;",
        "&nbsp;",
        "&#xa0;",
        "&ensp;",
        "&#8194;",
        "&#x2002;",
        "&emsp;",
        "&#8195;",
        "&#x2003;",
        "&thinsp;",
        "&#8201;",
        "&#x2009;",
        "&zwnj;",
        "&#8204;",
        "&#x200c;",
        "\x200D&zwj;",
        "&#8205;",
        "&#x200d;",
        "\x200E&lrm;",
        "&#8206;",
        "&#x200e;",
        "\x200F&rlm;",
        "&#8207;",
        "&#x200f;",
        " "
      };
        }

        public static byte[] GetBytesFromFile(this string file)
        {
            return file.GetBytesFromFile((Encoding)null);
        }

        public static byte[] GetBytesFromFile(this string file, Encoding encoding)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = encoding == null ? new StreamWriter((Stream)memoryStream) : new StreamWriter((Stream)memoryStream, encoding))
                {
                    streamWriter.Write(file);
                    streamWriter.Flush();
                    memoryStream.Position = 0L;
                    return memoryStream.ToArray();
                }
            }
        }

        public static string GetValueInside(
            this string texto,
            string chaveInicio,
            string ChaveFechamento)
        {
            string empty = string.Empty;
            string[] strArray = texto.Split(new string[1]
            {
        chaveInicio
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 1)
                empty = strArray[1].Split(new string[1]
                {
          ChaveFechamento
                }, StringSplitOptions.RemoveEmptyEntries)[0];
            else if (strArray.Length != 0)
                empty = strArray[0].Split(new string[1]
                {
          ChaveFechamento
                }, StringSplitOptions.RemoveEmptyEntries)[0];
            return empty;
        }

        public static List<string> GetAllValuesInside(
          this string texto,
          string chaveInicio,
          string ChaveFechamento)
        {
            List<string> stringList = new List<string>();
            string[] strArray = texto.Split(new string[1]
            {
        ChaveFechamento
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 0)
            {
                string[] separator = new string[1]
                {
          ChaveFechamento
                };
                for (int index = 1; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                    stringList.Add(strArray[index].Split(separator, StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            return stringList;
        }

        public static string ReplaceEscapeCode(this string valor)
        {
            return valor.ReplaceEscapeCode(" ");
        }

        public static string ReplaceEscapeCode(this string valor, string substituir)
        {
            return valor.Replace(TextExtension.GetCaracteresRemoval(), substituir).TrimDoubleSpace();
        }

        public static string TrimDoubleSpace(this string valor)
        {
            int length1 = valor.Length;
            char[] charArray = valor.ToCharArray();
            int length2 = 0;
            bool flag = true;
            for (int index = 0; index < length1; ++index)
            {
                char ch = charArray[index];
                if (ch != ' ')
                {
                    charArray[length2++] = ch;
                    flag = false;
                }
                else if (!flag)
                {
                    charArray[length2++] = ch;
                    flag = true;
                }
            }
            return new string(charArray, 0, length2).Trim();
        }

        public static string TrimMask(this string documento)
        {
            int length1 = documento.Length;
            char[] charArray = documento.ToCharArray();
            int length2 = 0;
            for (int index = 0; index < length1; ++index)
            {
                char ch = charArray[index];
                switch (ch)
                {
                    case '*':
                    case ',':
                    case '-':
                    case '.':
                    case '/':
                    case '\\':
                    case '_':
                        continue;
                    default:
                        charArray[length2++] = ch;
                        continue;
                }
            }
            return new string(charArray, 0, length2).Trim();
        }

        public static string ReplaceHexCode(this string valor)
        {
            valor = valor.Replace("\\xA0", " ");
            for (int index = 0; index <= (int)sbyte.MaxValue; ++index)
                valor = valor.Replace(string.Format("\\x{0:00}", (object)index), ((char)index).ToString());
            return valor;
        }

        public static string Replace(this string valor, List<string> remover, string substituidor)
        {
            foreach (string oldValue in remover)
                valor = valor.Replace(oldValue, substituidor);
            return valor;
        }

        public static string ConvertToUtf8(this string response)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(response));
        }

        public static string ConvertTo(this string response, string encode)
        {
            return Encoding.UTF8.GetString(Encoding.GetEncoding(encode).GetBytes(response));
        }

        public static string FormatCpfCnpj(this string documento)
        {
            documento = documento.TrimMask().TrimStart('0');
            if (documento.Length >= 12)
                return Convert.ToUInt64(documento).ToString("00\\.000\\.000/0000\\-00");

            return Convert.ToUInt64(documento).ToString("000\\.000\\.000\\-00");
        }
    }
}

