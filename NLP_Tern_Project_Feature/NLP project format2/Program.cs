using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace lexical2
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(@"D:\Class\NLP\reace\docs\me2_file(1+1).txt");
            string[] xmlpath = File.ReadAllLines(@"D:\Class\NLP\reace\docs\xml_file.tbl");

            foreach (string s in xmlpath)
            {
                Console.WriteLine(s);
                XmlDocument xd = new XmlDocument();
                xd.Load(@"D:\Class\NLP\reace" + s.Replace("./", "/"));

                XmlNodeList xdlist = xd.SelectNodes("//s");
                XmlNodeList xdlist_w_corpus = xd.SelectNodes("//w");
                XmlNodeList xdlist_ne = xd.SelectNodes("//ne");
                XmlNodeList xdlist_rel = xd.SelectNodes("//rel");

                List<List<string>> relList = new List<List<string>>();
                List<List<string>> corpus_nelist = new List<List<string>>();
                List<List<string>> corpus_wList = new List<List<string>>();

                prepareRelationCorpus(xdlist_rel, relList);
                prepareNeCorpus(xdlist_ne, corpus_nelist);
                prepareWordCorpus(xdlist_w_corpus, corpus_wList);

                foreach (XmlNode tern in xdlist)//each sentence
                {
                    List<string[]> nelist = new List<string[]>();
                    XmlNodeList xdlist_w = tern.ChildNodes;

                    /***********************/
                    emo(xd, xdlist_w, nelist);
                    /***********************/

                    for (int i = 0; i < nelist.Count(); i++)//read every mention
                        for (int j = i + 1; j < nelist.Count(); j++)
                        {
                            //do your code
                            //0 = ML12, 1 = EN TYPE, 2 = FR, 3 = TO,
                            //4 = head word, 5 = id, 6 = extent word
                            bool hasRelFlag = false;
                            hasRelFlag = whetherRelation(xdlist_rel, nelist[i][5], nelist[j][5]);
                            //Console.WriteLine(hasRelFlag);
                            generateFeature(sw, corpus_nelist, relList, corpus_wList, hasRelFlag, nelist[i], nelist[j]);
                        }
                }
            }
            sw.Close();
        }
        static void prepareWordCorpus(XmlNodeList xdlist_w_corpus, List<List<string>> corpus_wList)
        {
            foreach (XmlNode wItem in xdlist_w_corpus)//each word
            {
                List<string> wAttr = new List<string>();
                string wid = wItem.Attributes.GetNamedItem("id").Value;
                wAttr.Add(wid.Replace("w", " "));
                wAttr.Add(wItem.InnerText.Replace("\n", " ").Replace(" ", "_"));
                corpus_wList.Add(wAttr);
            }
        }
        static void prepareNeCorpus(XmlNodeList xdlist_ne, List<List<string>> corpus_nelist)
        {
            foreach (XmlNode neItem in xdlist_ne)
            {
                List<string> ne = new List<string>();
                string extent = null, head = null;
                ne.Add(neItem.Attributes.GetNamedItem("id").Value);
                ne.Add(neItem.Attributes.GetNamedItem("fr").Value.Replace("w", ""));// fr
                ne.Add(neItem.Attributes.GetNamedItem("to").Value.Replace("w", ""));// to
                XmlNodeList temp = neItem.ChildNodes;
                foreach (XmlNode terrrrn in temp)
                {
                    if (terrrrn.Attributes.GetNamedItem("type").Value == "extent")
                        extent = terrrrn.InnerText.Replace("\n", " ").Replace(" ", "_");
                    else if (terrrrn.Attributes.GetNamedItem("type").Value == "head")
                    {
                        head = terrrrn.InnerText.Replace("\n", " ").Replace(" ", "_");
                        break;
                    }
                }
                ne.Add(extent);//mention
                ne.Add(head);// head
                corpus_nelist.Add(ne);//�摮葉���� ne   
            }
        }
        static void prepareRelationCorpus(XmlNodeList xdlist_rel, List<List<string>> relList)
        {
            /*store corpus relation*/
            foreach (XmlNode relTerm in xdlist_rel)
            {
                List<string> rel = new List<string>();
                rel.Add(relTerm.Attributes.GetNamedItem("e1").Value);
                rel.Add(relTerm.Attributes.GetNamedItem("e2").Value);
                rel.Add(relTerm.Attributes.GetNamedItem("t").Value);
                relList.Add(rel);
            }
        }
        static void generateFeature(StreamWriter sw, List<List<string>> corpus_nelist, List<List<string>> relList, List<List<string>> corpus_wList, bool hasRelFlag, string[] M1, string[] M2)
        {
            char[] exSeperator = { ' ' };
            int e1Id = Convert.ToInt32(M1[5]);
            int e2Id = Convert.ToInt32(M2[5]);

            string[] exToken = M1[6].Split(exSeperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ex in exToken)
                sw.Write("WM1=" + ex.Replace("\n", " ").Replace(" ", "_") + " ");
            sw.Write("HM1=" + M1[4].Replace("\n", " ").Replace(" ", "_") +
                " HM12=" + M1[4].Replace("\n", " ").Replace(" ", "_") + "_" + M2[4].Replace("\n", " ").Replace(" ", "_") + " ");

            exToken = M2[6].Split(exSeperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ex in exToken)
                sw.Write("WM2=" + ex.Replace("\n", " ").Replace(" ", "_") + " ");
            sw.Write("HM2=" + M2[4].Replace("\n", " ").Replace(" ", "_") + " ");

            int between_word = Convert.ToInt32(M2[2].Replace("w", "")) - Convert.ToInt32(M1[3].Replace("w", ""));
            if (between_word > 0)
            {
                sw.Write("WBNULL=false ");// have word
                if (between_word == 1)
                {
                    foreach (List<string> w in corpus_wList)
                    {
                        if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M1[3].Replace("w", "")) + 1))
                        {
                            sw.Write("WBFL=" + w[1] + " ");
                            break;
                        }
                    }
                }
                else if (between_word >= 2)
                {
                    foreach (List<string> w in corpus_wList)
                    {
                        if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M1[3].Replace("w", "")) + 1))
                            sw.Write("WBF=" + w[1] + " ");
                        if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M2[2].Replace("w", "")) - 1))
                            sw.Write("WBL=" + w[1] + " ");
                    }
                    if (between_word >= 3)
                    {
                        foreach (List<string> w in corpus_wList)
                        {
                            if (Convert.ToInt32(w[0]) > (Convert.ToInt32(M1[3].Replace("w", "")) + 1) && Convert.ToInt32(w[0]) < (Convert.ToInt32(M2[2].Replace("w", "")) - 1))
                                sw.Write("WBO=" + w[1] + " ");
                        }
                    }
                }
            }
            else
                sw.Write("WBNULL=true ");

            foreach (List<string> w in corpus_wList)
            {
                if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M1[2].Replace("w", "")) - 1) && w[1] != ".")
                    sw.Write("BM1F=" + w[1] + " ");
                else if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M1[2].Replace("w", "")) - 2) && w[1] != ".")
                    sw.Write("BM1L=" + w[1] + " ");
                if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M2[3].Replace("w", "")) + 1) && w[1] != ".")
                    sw.Write("AM2F=" + w[1] + " ");
                else if (Convert.ToInt32(w[0]) == (Convert.ToInt32(M2[3].Replace("w", "")) + 2) && w[1] != ".")
                    sw.Write("AM2L=" + w[1] + " ");
            }
            if (hasRelFlag == true)
            {
                sw.Write("relation");
                /*foreach (List<string> item in relList)
                {
                    if (Convert.ToInt32(item[0]) == e1Id && Convert.ToInt32(item[1]) == e2Id)
                    {
                        sw.Write(item[2]);
                        break;
                    }
                }*/
            }
            else
                sw.Write("no_relation");
            sw.WriteLine();
        }
        static bool whetherRelation(XmlNodeList xdlist_rel, string e1_id, string e2_id)
        {
            foreach (XmlNode relTerm in xdlist_rel)
            {
                if (relTerm.Attributes.GetNamedItem("e1").Value == e1_id && relTerm.Attributes.GetNamedItem("e2").Value == e2_id)
                    return true;
            }
            return false;
        }
        static void emo(XmlDocument xd, XmlNodeList xdlist_w, List<string[]> nelist)
        {
            foreach (XmlNode terrn in xdlist_w)//each word
            {
                string wid = terrn.Attributes.GetNamedItem("id").Value;
                XmlNodeList xdlist_ne = xd.SelectNodes("//ne");
                foreach (XmlNode terrrn in xdlist_ne)//each ne
                {
                    if (wid == terrrn.Attributes.GetNamedItem("fr").Value)
                    {
                        string pptype = "";
                        XmlNodeList temp = terrrn["exattrs"].ChildNodes;
                        foreach (XmlNode terrrrn in temp)
                            if (terrrrn.Attributes.GetNamedItem("n").Value == "TYPE")
                            {
                                pptype = terrrrn.Attributes.GetNamedItem("v").Value;
                                break;
                            }
                        string[] ne = new string[7];
                        ne[0] = pptype;//ML12
                        ne[1] = terrrn.Attributes.GetNamedItem("t").Value;//EN TYPE
                        ne[2] = wid;//FR
                        ne[3] = terrrn.Attributes.GetNamedItem("to").Value;//TO
                        temp = terrrn.SelectNodes("textspan");
                        ne[4] = temp[1].InnerText;//head word
                        ne[5] = terrrn.Attributes.GetNamedItem("id").Value;//id
                        ne[6] = temp[0].InnerText;//extent word
                        nelist.Add(ne);
                        break;
                    }
                }
            }
        }
    }
}