using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication4
{
    class Program
    {
        static string read_path = "C:\\Users\\x\\Desktop\\Task2_Opinion Word Identification\\traing_output.txt";
        static string marge_path = "C:\\Users\\x\\Desktop\\Task2_Opinion Word Identification\\fold\\out\\";
        static string output_path = "C:\\Users\\x\\Desktop\\Task2_Opinion Word Identification\\fold\\out\\out_all.txt";

        static void Main(string[] args)
        {
            int count = 0;
            double GTP=0,TP= 0,TTALL=0;
            for (int i = 1; i < 11; i++)
            {
                string[] temp = File.ReadAllLines(marge_path + i +"_out.txt");
                File.AppendAllLines(output_path, temp);
            }
            string[] gold_stander = File.ReadAllLines(read_path);
            string[] predict_data = File.ReadAllLines(output_path);
            for (int i = 0,j=0; i < gold_stander.Count() && j < predict_data.Count();)
            {
                if (gold_stander[i] == "")
                {
                    i++;
                    continue;
                }
                if (predict_data[j] == "")
                {
                    j++;
                    continue;
                }
                string[] gold_temp = gold_stander[i++].Split(new char[] { '\t' });
                string[] pred_temp = predict_data[j++].Split(new char[] { '\t' });
                Console.WriteLine(gold_temp[0] +"&&" + pred_temp[0]);
                if (gold_temp[0].Equals(pred_temp[0]))
                {
                    if (gold_temp[3].Equals("OPINION"))
                    {
                        GTP++;
                    }
                    if (pred_temp[3].Equals("OPINION"))
                    {
                        if(gold_temp[3].Equals("OPINION"))
                        {
                            TP++;
                        }
                        TTALL++;
                    }
                }
                else
                    Console.ReadKey();
                count++;
            }
            double precision = (TP*100) / TTALL, recall = (TP*100) / GTP, f_score = (2 * precision * recall) / (precision + recall);
            Console.WriteLine("precision = " + precision + "\nrecall = " + recall + "\nf_score = " + f_score);
            Console.ReadKey();
        }
    }
}
