using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fold
{
    class Program
    {
        static int nfold = 10;
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\shown\\Desktop\\Task2_Opinion Word Identification\\traing_output.txt");
            int countPrefold = lines.Count() / nfold;

            for (int i = 1; i <= nfold; i++)
            {
                string path_train     = "C:\\Users\\shown\\Desktop\\Task2_Opinion Word Identification\\fold\\" + i + "_training.txt";
                string path_test      = "C:\\Users\\shown\\Desktop\\Task2_Opinion Word Identification\\fold\\" + i + "_test.txt";
                string path_test_gold = "C:\\Users\\shown\\Desktop\\Task2_Opinion Word Identification\\fold\\" + i + "_test_gold.txt";

                StreamWriter swTrain     = new StreamWriter(path_train);
                StreamWriter swTest      = new StreamWriter(path_test);
                StreamWriter swTest_gold = new StreamWriter(path_test_gold);

                for (int k = 0; k < lines.Count(); k++)
                {
                    if (k >= countPrefold * (i - 1) && k < countPrefold * i)
                    {
                        swTest_gold.WriteLine(lines[k]);
                        string[] temp = lines[k].Split(new char[] { '\t' });

                        if (temp.Count()>2)
                            swTest.WriteLine(temp[0] + "\t" + temp[1] + "\t" + temp[2] );
                        else
                            swTest.WriteLine(lines[k]);
                    }
                    else
		    {
			swTrain.WriteLine(lines[k]);
		    }
                }

                swTest.WriteLine();
                swTest.Close();
                swTest_gold.Close();
                swTrain.Close();
            }
        }
    }
}
