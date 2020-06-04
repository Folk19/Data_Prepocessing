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
        static void Main(string[] args)
        {
            string r_path = "./input_hotel_review.txt";
            string w_path = "./output.txt";
            StreamWriter sw = new StreamWriter(w_path);
            StreamReader sr = new StreamReader(r_path);            
            
            NLP nlp = new NLP();
            
            while (!sr.EndOfStream)
            {
                int index = 0,count = 0;
                List<indexcoll> option_index = new List<indexcoll>();
                string str = sr.ReadLine();
                do
                {
                    index = str.IndexOf("<opinion>", index);
                    if (index != -1)
                    {
                        int end_index = str.IndexOf("</opinion>", index);
                        indexcoll temp = new indexcoll();
                        temp.start = index - (19 * count);
                        index += 9;
                        temp.end = end_index - index;
                        option_index.Add(temp);
                        count++;
                    }
                } while (index != -1);
                Console.WriteLine(str);

                index = count = 0;
                //chinese word segmentation
                string[] result = nlp.CWS(str);
                foreach (string s in result)
                {
                    string[] tokens = s.Split('　');

                    foreach (string t in tokens)
                    {
                        char[] separators = {'(',')'};
                        string[] temp = t.Split(separators);
                        for (int i = 0,flag=0; i < temp[0].Length; i++,count++)
                        {
                            //[char][\t][POS][\t][斷詞後的長度][\t][斷詞的B,I][\t][Ans]
                            sw.Write(temp[0][i] + "\t" + temp[1] + "\t" + temp[0].Length + "\t");
                            if (i == 0)
                            {
                                sw.Write("B\t");
                                if (index<option_index.Count()&&option_index[index].start.Equals(count))
                                {
                                    sw.Write("B-OPINION\t");
                                    flag = 1;
                                }
                                else
                                {
                                    sw.Write("0\t");
                                    if (flag == 1)
                                    {
                                        flag = 0;
                                        index++;
                                    }
                                }
                            }
                            else
                            {
                                sw.Write("I\t");
                                if (flag == 1 && (option_index[index].start + option_index[index].end) > count)
                                    sw.Write("I-OPINION\t");
                                else
                                    sw.Write("0\t");
                            }
                            sw.WriteLine();
                        }
                    }
                }
                sw.WriteLine();
            }
        }
        class indexcoll
        {
            public int start;
            public int end;
        }
    }
}
