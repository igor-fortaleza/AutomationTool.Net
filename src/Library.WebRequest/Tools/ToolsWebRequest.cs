using HtmlAgilityPack;
using Library.TextHelp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.WebRequest.Tools
{
    public static class HelpWebRequest
    {
        public static HtmlDocument ToHtmlDocument(this string stringHtml)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(stringHtml);
            return htmlDocument;
        }

        public static HtmlDocument ToHtmlDocument(this HttpWebResponse httpResponse)
        {
            return httpResponse.ToHtmlDocument(Encoding.Default);
        }

        public static HtmlDocument ToHtmlDocument(
          this HttpWebResponse httpResponse,
          Encoding encoding)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            try
            {
                htmlDocument.LoadHtml(httpResponse.ToHtmlString(encoding));
            }
            catch
            {
                htmlDocument = (HtmlDocument)null;
            }
            finally
            {
                httpResponse.Close();
            }
            return htmlDocument;
        }

        public static string ToHtmlString(this HttpWebResponse httpResponse)
        {
            return httpResponse.ToHtmlString((Encoding)null);
        }

        public static string ToHtmlString(this HttpWebResponse httpResponse, Encoding encoding)
        {
            try
            {
                return (encoding == null ? (TextReader)new StreamReader(httpResponse.GetResponseStream()) : (TextReader)new StreamReader(httpResponse.GetResponseStream(), encoding)).ReadToEnd().Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static HtmlNode GetElementById(this HtmlDocument htmlDoc, string id)
        {
            return htmlDoc.DocumentNode.Descendants().FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Id != null)
                    return x.Id == id;
                return false;
            }));
        }

        public static HtmlNode GetElementById(this HtmlNode node, string id)
        {
            return ((IEnumerable<HtmlNode>)node.ChildNodes).FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Id != null)
                    return x.Id == id;
                return false;
            }));
        }

        public static List<HtmlNode> GetElementsByTag(this HtmlNode node, string tag)
        {
            return node.Descendants(tag).ToList<HtmlNode>();
        }

        public static List<HtmlNode> GetElementsByTag(this HtmlDocument htmlDoc, string tag)
        {
            return htmlDoc.DocumentNode.Descendants(tag).ToList<HtmlNode>();
        }

        public static List<HtmlNode> GetElementsByTag(
          this List<HtmlNode> htmlDoc,
          string tag)
        {
            return ((IEnumerable<HtmlNode>)htmlDoc).Where<HtmlNode>((Func<HtmlNode, bool>)(x => x.Name == tag)).ToList<HtmlNode>();
        }

        public static HtmlNode GetElementByName(
          this HtmlNode node,
          string name,
          string element)
        {
            try
            {
                return node.SelectSingleNode(string.Format("//{1}[@name='{0}']", (object)name, (object)element));
            }
            catch
            {
                return (HtmlNode)null;
            }
        }

        public static List<string> GetTextByCol(this HtmlNode node, int index)
        {
            List<string> stringList = new List<string>();
            using (List<HtmlNode>.Enumerator enumerator = node.GetElementsByTag("tr").GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    List<HtmlNode> elementsByTag = enumerator.Current.GetElementsByTag("td");
                    if (elementsByTag != null && elementsByTag.Count > 0)
                        stringList.Add(elementsByTag[index].InnerText);
                }
            }
            return stringList;
        }

        public static List<string> GetTextByRow(this HtmlNode node, int index)
        {
            List<string> stringList = new List<string>();
            List<HtmlNode> elementsByTag = node.GetElementsByTag("tr");
            if (elementsByTag != null && elementsByTag.Count > index)
            {
                using (List<HtmlNode>.Enumerator enumerator = elementsByTag[index].GetElementsByTag("td").GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        HtmlNode current = enumerator.Current;
                        stringList.Add(current.InnerText);
                    }
                }
            }
            return stringList;
        }

        public static string GetTextByRowCol(this HtmlNode node, int indexRow, int indexCol)
        {
            string str = string.Empty;
            List<HtmlNode> elementsByTag1 = node.GetElementsByTag("tr");
            if (elementsByTag1 != null && elementsByTag1.Count > indexRow)
            {
                List<HtmlNode> elementsByTag2 = elementsByTag1[indexRow].GetElementsByTag("td");
                if (elementsByTag2 != null && elementsByTag2.Count > indexCol)
                    str = elementsByTag2[indexCol].InnerText;
            }
            return str;
        }

        public static HtmlNode GetNextElement(this HtmlNode node)
        {
            HtmlNode htmlNode = node;
            try
            {
                do
                {
                    htmlNode = htmlNode.NextSibling;
                    if (htmlNode == null)
                        break;
                }
                while (htmlNode.Name == "#text");
            }
            catch
            {
                htmlNode = (HtmlNode)null;
            }
            return htmlNode;
        }

        public static HtmlNode GetNextElement(this HtmlNode node, string tag)
        {
            HtmlNode node1 = node;
            do
            {
                node1 = node1.GetNextElement();
            }
            while (node1 != null && node1.Name != tag);
            return node1;
        }

        public static HtmlNode GetPrevElement(this HtmlNode node)
        {
            HtmlNode htmlNode = node;
            do
            {
                htmlNode = htmlNode.PreviousSibling;
            }
            while (htmlNode != null && htmlNode.Name == "#text");
            return htmlNode;
        }

        public static HtmlNode GetPrevElement(this HtmlNode node, string tag)
        {
            HtmlNode node1 = node;
            do
            {
                node1 = node1.GetPrevElement();
            }
            while (node1 != null && node1.Name != tag);
            return node1;
        }

        public static Match GetByRegex(this HtmlNode node, string regex)
        {
            try
            {
                return new Regex(regex).Match(node.OuterHtml);
            }
            catch
            {
                return (Match)null;
            }
        }

        public static string TrimHtml(this string html)
        {
            html = WebUtility.HtmlDecode(html).ReplaceEscapeCode();
            try
            {
                for (int startIndex = html.IndexOf("<!--"); startIndex > -1; startIndex = html.IndexOf("<!--"))
                {
                    int num = html.IndexOf("-->") - startIndex;
                    if (num < 0)
                        num = html.Length - startIndex - 3;
                    html = html.Replace(html.Substring(startIndex, num + 3), string.Empty);
                }
            }
            catch
            {
                html = string.Empty;
            }
            return html;
        }

        public static List<HtmlNode> GetElementsByClass(
          this HtmlDocument htmlDoc,
          string value)
        {
            return htmlDoc.GetElementsByAttribute("class", value);
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this HtmlDocument htmlDoc,
          string attribute)
        {
            return htmlDoc.DocumentNode.Descendants().Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this HtmlDocument htmlDoc,
          string attribute,
          string value)
        {
            return htmlDoc.DocumentNode.Descendants().Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name == null || !(y.Name == attribute) || y.Value == null)
                            return false;
                        return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w =>
                        {
                            if (w != null)
                                return w == value;
                            return false;
                        }));
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static HtmlNode GetElementByAttribute(
          this HtmlDocument htmlDoc,
          string attribute)
        {
            return htmlDoc.DocumentNode.Descendants().FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            }));
        }

        public static HtmlNode GetElementByAttribute(
          this HtmlDocument htmlDoc,
          string attribute,
          string value)
        {
            return htmlDoc.DocumentNode.Descendants().FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x => ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
            {
                if (!(y.Name == attribute))
                    return false;
                return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w => w == value));
            }))));
        }

        public static List<HtmlNode> GetElementsByClass(this HtmlNode htmlDoc, string value)
        {
            return htmlDoc.GetElementsByAttribute("class", value);
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this HtmlNode htmlDoc,
          string attribute)
        {
            return htmlDoc.Descendants().Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this HtmlNode htmlDoc,
          string attribute,
          string value)
        {
            return htmlDoc.Descendants().Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name == null || !(y.Name == attribute) || y.Value == null)
                            return false;
                        return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w =>
                        {
                            if (w != null)
                                return w == value;
                            return false;
                        }));
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static HtmlNode GetElementByAttribute(this HtmlNode htmlDoc, string attribute)
        {
            return htmlDoc.Descendants().FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            }));
        }

        public static HtmlNode GetElementByAttribute(
          this HtmlNode htmlDoc,
          string attribute,
          string value)
        {
            return htmlDoc.Descendants().FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x => ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
            {
                if (!(y.Name == attribute))
                    return false;
                return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w => w == value));
            }))));
        }

        public static List<HtmlNode> GetElementsByClass(
          this List<HtmlNode> htmlDoc,
          string value)
        {
            return htmlDoc.GetElementsByAttribute("class", value);
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this List<HtmlNode> htmlDoc,
          string attribute)
        {
            return ((IEnumerable<HtmlNode>)htmlDoc).Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static List<HtmlNode> GetElementsByAttribute(
          this List<HtmlNode> htmlDoc,
          string attribute,
          string value)
        {
            return ((IEnumerable<HtmlNode>)htmlDoc).Where<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name == null || !(y.Name == attribute) || y.Value == null)
                            return false;
                        return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w =>
                        {
                            if (w != null)
                                return w == value;
                            return false;
                        }));
                    }));
                return false;
            })).ToList<HtmlNode>();
        }

        public static HtmlNode GetElementByAttribute(
          this List<HtmlNode> htmlDoc,
          string attribute)
        {
            return ((IEnumerable<HtmlNode>)htmlDoc).FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x =>
            {
                if (x.Attributes != null)
                    return ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
                    {
                        if (y.Name != null)
                            return y.Name == attribute;
                        return false;
                    }));
                return false;
            }));
        }

        public static HtmlNode GetElementByAttribute(
          this List<HtmlNode> htmlDoc,
          string attribute,
          string value)
        {
            return ((IEnumerable<HtmlNode>)htmlDoc).FirstOrDefault<HtmlNode>((Func<HtmlNode, bool>)(x => ((IEnumerable<HtmlAttribute>)x.Attributes).Any<HtmlAttribute>((Func<HtmlAttribute, bool>)(y =>
            {
                if (!(y.Name == attribute))
                    return false;
                return ((IEnumerable<string>)y.Value.Split(' ')).Any<string>((Func<string, bool>)(w => w == value));
            }))));
        }

        public static HtmlNode RemoveChildByName(this HtmlDocument htmlDoc, string name)
        {
            return htmlDoc.DocumentNode.RemoveChildByName(name, false);
        }

        public static HtmlNode RemoveChildByName(
          this HtmlDocument htmlDoc,
          string name,
          bool allChild)
        {
            return htmlDoc.DocumentNode.RemoveChildByName(name, allChild);
        }

        public static List<HtmlNode> RemoveChildByName(
          this List<HtmlNode> htmlDoc,
          string name)
        {
            return htmlDoc.RemoveChildByName(name, false);
        }

        public static List<HtmlNode> RemoveChildByName(
          this List<HtmlNode> htmlDoc,
          string name,
          bool allChild)
        {
            using (List<HtmlNode>.Enumerator enumerator = htmlDoc.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    enumerator.Current.RemoveChildByName(name, allChild);
            }
            return htmlDoc;
        }

        public static HtmlNodeCollection RemoveChildByName(
          this HtmlNodeCollection htmlDoc,
          string name)
        {
            return htmlDoc.RemoveChildByName(name, false);
        }

        public static HtmlNodeCollection RemoveChildByName(
          this HtmlNodeCollection htmlDoc,
          string name,
          bool allChild)
        {
            using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>)htmlDoc).GetEnumerator())
            {
                while (((IEnumerator)enumerator).MoveNext())
                    enumerator.Current.RemoveChildByName(name, allChild);
            }
            return htmlDoc;
        }

        public static HtmlNode RemoveChildByName(this HtmlNode htmlDoc, string name)
        {
            return htmlDoc.RemoveChildByName(name, false);
        }

        public static HtmlNode RemoveChildByName(
          this HtmlNode htmlDoc,
          string name,
          bool allChild)
        {
            int count = htmlDoc.ChildNodes.Count;
            for (int index = 0; index < count; ++index)
            {
                if (htmlDoc.ChildNodes[index].Name == name)
                {
                    htmlDoc.ChildNodes.Remove(index);
                    --index;
                    count = htmlDoc.ChildNodes.Count;
                }
                else if (allChild)
                    htmlDoc.ChildNodes[index].RemoveChildByName(name, true);
            }
            return htmlDoc;
        }

        public static HtmlNode GetElementContainsTagAndText(
              this List<HtmlNode> nodes,
              string tag,
              string contains)
        {
            HtmlNode htmlNode = (HtmlNode)null;
            using (List<HtmlNode>.Enumerator enumerator = nodes.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    HtmlNode current = enumerator.Current;
                    List<HtmlNode> elementsByTag = current.GetElementsByTag(tag);
                    if (elementsByTag != null && ((IEnumerable<HtmlNode>)elementsByTag).Any<HtmlNode>((Func<HtmlNode, bool>)(x => x.InnerHtml.ToUpperInvariant().Contains(contains))))
                    {
                        htmlNode = current;
                        break;
                    }
                }
            }
            return htmlNode;
        }
    }
}
