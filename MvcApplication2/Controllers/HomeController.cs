﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcApplication2.Models;
using HtmlAgilityPack;

namespace MvcApplication2.Controllers
{
    public static class HtmlSanitizer
    {
        private static readonly IDictionary<string, string[]> Whitelist;
        private static List<string> DeletableNodesXpath = new List<string>();

        static HtmlSanitizer()
        {
            Whitelist = new Dictionary<string, string[]> {
                { "a", new[] { "href" } },
                { "strong", null },
                { "em", null },
                { "blockquote", null },
                { "b", null},
                { "p", null},
                { "ul", null},
                { "ol", null},
                { "li", null},
                { "strike", null},
                { "u", null},                
                { "sub", null},
                { "sup", null},
                { "table", null },
                { "tr", null },
                { "td", null },
                { "th", null }
                };
        }

        public static string Sanitize(string input)
        {
            if (input.Trim().Length < 1)
                return string.Empty;
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(input);
            SanitizeNode(htmlDocument.DocumentNode);
            string xPath = HtmlSanitizer.CreateXPath();

            return StripHtml(htmlDocument.DocumentNode.WriteTo().Trim(), xPath);
        }

        private static void SanitizeChildren(HtmlNode parentNode)
        {
            for (int i = parentNode.ChildNodes.Count - 1; i >= 0; i--)
            {
                SanitizeNode(parentNode.ChildNodes[i]);
            }
        }

        private static void SanitizeNode(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Element)
            {
                if (!Whitelist.ContainsKey(node.Name))
                {
                    if (!DeletableNodesXpath.Contains(node.Name))
                    {
                        //DeletableNodesXpath.Add(node.Name.Replace("?",""));
                        node.Name = "removeableNode";
                        DeletableNodesXpath.Add(node.Name);
                    }
                    if (node.HasChildNodes)
                    {
                        SanitizeChildren(node);
                    }

                    return;
                }

                if (node.HasAttributes)
                {
                    for (int i = node.Attributes.Count - 1; i >= 0; i--)
                    {
                        HtmlAttribute currentAttribute = node.Attributes[i];
                        string[] allowedAttributes = Whitelist[node.Name];
                        if (allowedAttributes != null)
                        {
                            if (!allowedAttributes.Contains(currentAttribute.Name))
                            {
                                node.Attributes.Remove(currentAttribute);
                            }
                        }
                        else
                        {
                            node.Attributes.Remove(currentAttribute);
                        }
                    }
                }
            }

            if (node.HasChildNodes)
            {
                SanitizeChildren(node);
            }
        }

        private static string StripHtml(string html, string xPath)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            if (xPath.Length > 0)
            {
                HtmlNodeCollection invalidNodes = htmlDoc.DocumentNode.SelectNodes(@xPath);
                if (invalidNodes != null)
                    foreach (HtmlNode node in invalidNodes)
                    {
                        node.ParentNode.RemoveChild(node, true);
                    }
            }
            return htmlDoc.DocumentNode.WriteContentTo(); ;
        }

        private static string CreateXPath()
        {
            string _xPath = string.Empty;
            for (int i = 0; i < DeletableNodesXpath.Count; i++)
            {
                if (i != DeletableNodesXpath.Count - 1)
                {
                    _xPath += string.Format("//{0}|", DeletableNodesXpath[i].ToString());
                }
                else _xPath += string.Format("//{0}", DeletableNodesXpath[i].ToString());
            }
            return _xPath;
        }
    }

    [ValidateInput(false)]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static Dictionary<string, UserRegistation> ip2UserRegistations = new Dictionary<string, UserRegistation>();
        private static Dictionary<string, FotoDatas> FotoName2Comments = new Dictionary<string, FotoDatas>();

        private Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

        private string CreateStatistics()
        {
            var userID = UserIp();
            var lastVisit = ip2UserRegistations[userID].Visits.Count > 1 ?
                                string.Format("{0} on {1}", ip2UserRegistations[userID].Visits[ip2UserRegistations[userID].Visits.Count - 2].Item1,
                                                            ip2UserRegistations[userID].Visits[ip2UserRegistations[userID].Visits.Count - 2].Item2)
                           : "Это первое посещение!";

            var toDraw = String.Format("Всего посещений: {0}\r\n" +
                          "За сегодня: {1}\r\n" +
                          "Предыдущий визит: {2}", ip2UserRegistations[userID].Visits.Count,
                ip2UserRegistations[userID].Visits.Count(visitTime => visitTime.Item1.Date.Equals(DateTime.Now.Date)), lastVisit);

            var fileName = new Random().Next() + ".jpg";
            DrawText(toDraw, new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold), Color.Brown, Color.Cornsilk).Save(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\info\" + fileName, ImageFormat.Jpeg);
            return fileName;
        }

        private string UserIp()
        {
            return HttpContext.Request.UserHostAddress;
        }

        private void Register(string place)
        {
            var userIp = UserIp();
            if (userIp != null)
            {
                if (!ip2UserRegistations.ContainsKey(userIp))
                {
                    ip2UserRegistations.Add(userIp, new UserRegistation());
                }
                ip2UserRegistations[userIp].Visits.Add(new Tuple<DateTime, string>(DateTime.Now, place));
            }
        }

        private string AddComment(string toFoto, string comment)
        {
            if (!FotoName2Comments.ContainsKey(toFoto))
            {
                FotoName2Comments.Add(toFoto, new FotoDatas());
                FotoName2Comments[toFoto].FotoName = toFoto;
            }
            string ans;
            if (DateTime.Now - ip2UserRegistations[UserIp()].LastPostTime < TimeSpan.FromSeconds(15))
            {
                ans = "Еще не прошло 30 секунд с последней публикации. Ждать еще " + (15 - (DateTime.Now - ip2UserRegistations[UserIp()].LastPostTime).Seconds);
            }
            else
            {
                ans = "Ваш коммент добавлен.";
                FotoName2Comments[toFoto].Comments.Add(HtmlSanitizer.Sanitize(comment));
                ip2UserRegistations[UserIp()].LastPostTime = DateTime.Now;
            }

            return ans;
        }

        private string AddVote(string toFoto, int sign)
        {
            if (!FotoName2Comments.ContainsKey(toFoto))
            {
                FotoName2Comments.Add(toFoto, new FotoDatas());
                FotoName2Comments[toFoto].FotoName = toFoto;
            }

            if (Math.Abs(sign) != 1)
                return "Что-то не так с поставленной оценкой. Может, Вы хакер?";

            string ans;
            if (DateTime.Now - ip2UserRegistations[UserIp()].LastVoteTime < TimeSpan.FromSeconds(10))
            {
                ans = "Еще не прошло 30 секунд с последнего голосования. Ждать еще " + (10 - (DateTime.Now - ip2UserRegistations[UserIp()].LastVoteTime).Seconds);
            }
            else
            {
                ans = "Ваш голос учтён.";
                FotoName2Comments[toFoto].Mark += sign;
                ip2UserRegistations[UserIp()].LastVoteTime = DateTime.Now;
            }
            return ans;
        }

        public ActionResult Index()
        {
            Register("index");
            return View();
        }
        public ActionResult Statictics()
        {
            Register("info");
            return View(new FotoName { name = CreateStatistics() });
        }

        public ActionResult Info()
        {
            Register("info");
            return View();
        }

        [HttpPost]
        public ActionResult AddCommentPage(string fotoName, string review)
        {

            Register("comment added");
            return View(new CommentAddedInfo
            {
                status =
                    AddComment(fotoName, review)
            });
        }
        [HttpGet]
        public ActionResult Galery()
        {
            Register("galery");
            var place = "minis";
            return View(Directory.GetFiles(@"c:\users\sht3ch\documents\visual studio 2013\Projects\MvcApplication2\MvcApplication2\pics\" + place + "\\").
                Where(name => name.EndsWith(".jpg")).Select(str => "/pics/" + place + "/" + str.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).
                    Last()).
                    ToList());
        }


        public ActionResult MyVisits()
        {
            Register("visits");
            if (UserIp() == null)
                return View(new UserRegistation());
            return View(ip2UserRegistations[UserIp()]);
        }

        public ActionResult AjaxGetComments(int id)
        {
            Register("comment getted");
            var requiredName = Request.QueryString.Get("name");

            return View(FotoName2Comments.ContainsKey(requiredName) ? FotoName2Comments[requiredName].Comments : new List<string> { "Nocomments. ur comment will be first!" });
        }

        public ActionResult AjaxVote(int id)
        {
            Register("voting");
            var requiredName = Request.QueryString.Get("name");
            int sign;
            sign = int.TryParse(Request.QueryString.Get("sign"), out sign) ? sign : 0;

            return View(new VoteForFoto { foto = requiredName, sign = sign, status = AddVote(requiredName, sign) });
        }

        public ActionResult AjaxGetVote(int id)
        {
            Register("Get vote");
            var requiredName = Request.QueryString.Get("name");

            var mark = FotoName2Comments.ContainsKey(requiredName) ? FotoName2Comments[requiredName].Mark : 0;

            File.

            return View(new VoteForFoto { foto = requiredName, sign = mark});
        }
        public ActionResult AjaxTestScript()
        {
            return View();
        }


    }
}
