using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using opennlp.tools.parser;
using LemmaSharp;


namespace NLP_Project
{
    class Program
    {
        public static Dictionary<string,string> dict = new Dictionary<string,string>();
        public static string path = "C:\\Users\\x\\Desktop\\Task1_Document Classification\\task1_test_svm.txt";
        public static StreamWriter sw = new StreamWriter(path);
        public static List<string> OutOfDictionary = new List<String>();
        
        static void Main(string[] args)
        {
            string[] dictline = System.IO.File.ReadAllLines("C:\\Users\\x\\Documents\\Visual Studio 2013\\Projects\\NLP\\NLP\\bin\\Debug\\dic.txt");
            char[] delimate1 = { ':' };
            foreach(string line in dictline)
            {
                string[] temp = line.Split(delimate1);
                dict.Add(temp[0],temp[1]);
            }

           
            
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\x\\Desktop\\Task1_Document Classification\\task1_test.xml");
            /*XmlNodeList books = doc.SelectNodes("/RDF/Text[@category='book']");
            XmlNodeList dvds = doc.SelectNodes("/RDF/Text[@category='dvd']");
            XmlNodeList healths = doc.SelectNodes("/RDF/Text[@category='health']");
            XmlNodeList musics = doc.SelectNodes("/RDF/Text[@category='music']");
            XmlNodeList toys_games = doc.SelectNodes("/RDF/Text[@category='toys_games']");*/
            XmlNodeList unknow = doc.SelectNodes("/RDF/Text[@category='?']");

            //for (int i = 0; i < books.Count || i < dvds.Count || i < healths.Count || i < musics.Count || i < toys_games.Count; i++)
            for (int i = 0; i < unknow.Count; i++)
            {
                /*if (i < books.Count)
                {
                    Console.WriteLine("RUNNING...1");
                    sw.Write("+1 ");
                    looking_dict(slipt(books[i].InnerText));
                }
                if (i < dvds.Count)
                {
                    Console.WriteLine("RUNNING...2");
                    sw.Write("+2 ");
                    looking_dict(slipt(dvds[i].InnerText));
                }
                if (i < healths.Count)
                {
                    Console.WriteLine("RUNNING...3");
                    sw.Write("+3 ");
                    looking_dict(slipt(healths[i].InnerText));
                }
                if (i < musics.Count)
                {
                    Console.WriteLine("RUNNING...4");
                    sw.Write("+4 ");
                    looking_dict(slipt(musics[i].InnerText));
                }
                if (i < toys_games.Count)
                {
                    Console.WriteLine("RUNNING...5");
                    sw.Write("+5 ");
                    looking_dict(slipt(toys_games[i].InnerText));
                }*/
                Console.WriteLine("RUNNING..."+i);
                sw.Write("0 ");
                looking_dict(slipt(unknow[i].InnerText));
            }
            sw.Close();
        }
        static List<string> slipt(string n)
        {
            NLP nlp = new NLP();
            char[] delimate = new char[] { '\'', ':', ',', '.', '(', ')', '/', '\"', '!', '*', ';', '[', ']', '{', '}' };
            List<string> temp = new List<string>();

            /*sentence detect*/
            string[] sentences = nlp.SentDetect(n);
            foreach (string s in sentences)
            {
                /*tokenization*/
                string[] tokens2 = nlp.Tokenize(s);
                /*Stemming, Lemmatization*/
                for (int i = 0; i < tokens2.Length; i++)
                    tokens2[i] = nlp.Lemmatization(tokens2[i]);
                /*Filter out stopwords*/
                string[] result2 = nlp.FilterOutStopWords(tokens2);
                foreach (string sf in result2)
                {
                    string[] te = sf.Split(delimate);
                    foreach (string t in te)
                        if (t != "")
                            temp.Add(t);
                }
            }
            return temp;
            /*Paser
                Parse p = nlp.Parser(sent);
                p.show();*/
            //Console.ReadKey();
        }
        static void looking_dict(List<string> words)
        {
            Dictionary<int,int> afterlooking = new Dictionary<int,int>();
            foreach (string w in words)
            {
                int index = 0;
                if (dict.ContainsKey(w))
                    index = Convert.ToInt32(dict[w]);
                else
                {
                    if (OutOfDictionary.Contains(w))
                        OutOfDictionary.Add(w);
                    index = dict.Count() + OutOfDictionary.IndexOf(w) + 1;
                }
                if (afterlooking.ContainsKey(index))
                {
                    afterlooking[index]++;
                }
                else
                {
                    afterlooking.Add( index , 1 );
                }
            }
            List<int> sort = afterlooking.Keys.ToList();
            sort.Sort();
            foreach (int s in sort)
            {
                sw.Write(s+":"+afterlooking[s]+" ");
            }
            sw.WriteLine();

        }
    }
}
