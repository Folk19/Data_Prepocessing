using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace XML讀取
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string[] path = File.ReadAllLines("D:\\Class\\NLP\\reace\\docs\\xml_file.tbl");
            StreamWriter hwsw = new StreamWriter("D:\\Class\\NLP\\reace\\docs\\headword.txt");
            foreach (string s in path)
            {
                Console.WriteLine(s);
                XmlDocument xd = new XmlDocument();
                xd.Load("D://Class//NLP//reace"+s.Replace("./","/"));
                XmlNodeList xdlist_ne = xd.SelectNodes("//ne");
                foreach (XmlNode terrrn in xdlist_ne)//each ne
                {
                        string pptype = "";
                        XmlNodeList temp = terrrn.SelectNodes("textspan");
                        foreach (XmlNode terrrrn in temp)
                            if (terrrrn.Attributes.GetNamedItem("type").Value == "head")
                            {
                                pptype = terrrrn.InnerText;
                                break;
                            }
                        hwsw.WriteLine(pptype);//head word
                        
                }
                /*foreach (XmlNode tern in xdlist)
                {
                    List<string> sen = new List<string>();
                    XmlNodeList child = tern.SelectNodes("w");
                    foreach(XmlNode terrn in child)
                    {
                        wsw.WriteLine(terrn.InnerText);
                        sen.Add(terrn.InnerText);
                    }

                    wsw.WriteLine();
                    if (tern.Attributes.GetNamedItem("phr") != null)
                    {
                        
                        string type = "";
                        type = tern.Attributes.GetNamedItem("phr").Value;
                        /*foreach(XmlNode temp in child)
                        {
                            //Console.WriteLine(temp.InnerText);
                            if(temp.Attributes.GetNamedItem("n").Value=="TYPE")
                            {
                                type = temp.Attributes.GetNamedItem("v").Value;
                                break;
                            }
                        }
                        if (type != "" && !dic.ContainsKey(type))
                            dic.Add(type, 1);
                    }
                }*/
            }
            hwsw.Close();
        }
    }
}
