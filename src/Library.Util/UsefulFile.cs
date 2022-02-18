using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Useful
{
    public class UsefulFile
    {
        public string RemodelFileName(string fileName, ref Dictionary<string, int> filesNameDic)
        {
            string empty = string.Empty;
            int num;
            string str1;
            if (filesNameDic.TryGetValue(fileName, out num))
            {
                filesNameDic.Remove(fileName);
                string str2 = ((IEnumerable<string>)fileName.Split('.')).Last<string>();
                str1 = fileName.Remove(fileName.Length - str2.Length) + "(" + (object)num + ")." + str2;
            }
            else
                str1 = fileName;
            filesNameDic.Add(fileName, num + 1);
            return str1;
        }
    }
}
