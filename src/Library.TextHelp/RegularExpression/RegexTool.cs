using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.TextHelp.RegularExpression
{
    public static class RegexTool
    {
        public static bool ValidEmailAddress(this string emailAddress)
        {
            return IsMatch(emailAddress, @"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
        }

        public static bool ValidCellPhoneNumber(this string cellPhoneNumber)
        {
            return IsMatch(cellPhoneNumber, @"/^\([0-9]{2}\) [0-9]?[0-9]{4}-[0-9]{4}$/");
        }

        public static bool ValidUrl(this string url)
        {
            return IsMatch(url, @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
        }

        public static List<string> GetEmailAddress(this string emailAddress)
        {
            return Matches(emailAddress, @"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
        }

        public static List<string> GetCellPhoneNumber(this string cellPhoneNumber)
        {
            return Matches(cellPhoneNumber, @"/^\([0-9]{2}\) [0-9]?[0-9]{4}-[0-9]{4}$/");
        }

        public static List<string> GetUrl(this string url)
        {
            return Matches(url, @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
        }

        public static string GetChassi(this string text)
        {
            string chassi = string.Empty;

            if (text.Length == 17)
            {
                chassi = Regex.Match(Regex.Match(text, "(?s)(?i)(?<=Chassi)(?-i).+?(?<=[A-HJ-NPR-Z0-9]{17})", RegexOptions.IgnoreCase).ToString(), "[A-HJ-NPR-Z0-9]{17}", RegexOptions.IgnoreCase).ToString();
                if (chassi.Length < 1)
                {
                    chassi = Regex.Match(text, "(?s)(?i)(?<=Chassi)(?-i).+?(?<=[A-HJ-NPR-Z0-9]{17})", RegexOptions.IgnoreCase).ToString();
                    if (chassi.Length < 1)
                        chassi = Regex.Match(text, "[A-HJ-NPR-Z0-9]{17}", RegexOptions.IgnoreCase).ToString();
                }
            }
            return chassi;
        }

        public static string RemoveAccents(this string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc  ";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
        #region RemoveAccents
        //public string RemoveAccents(string text)
        //{
        //    StringBuilder sbReturn = new StringBuilder();
        //    var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        //    foreach (char letter in arrayText)
        //    {
        //        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
        //            sbReturn.Append(letter);
        //    }
        //    return sbReturn.ToString();
        //}
        #endregion

        public static string RemoveSpecialCharacters(this string texto)
        {
            string specialCharacters = "-/!@#$%¨&*()_+¹²³£¢¬§[]{}ªº'\"=|\\:;.,?°^~´´";

            for (int i = 0; i < specialCharacters.Length; i++)
            {
                texto = texto.Replace(specialCharacters[i].ToString(), string.Empty);
            }
            return texto;
        }

        private static bool IsMatch(string value, string @regex)
        {
            Regex regularExpression = new Regex(@regex);

            if (regularExpression.IsMatch(value))
                return true;
            else
                return false;
        }

        private static List<string> Matches(string value, string @regex)
        {
            List<string> listFound = new List<string>();
            MatchCollection match = Regex.Matches(value, @regex);

            for (int i = 0; i < match.Count; i++)
            {
                listFound.Add(match[i].Value);
            }

            return listFound;
        }

    }
}
