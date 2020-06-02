using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace temp
{
    class Program
    {
        static void Main(string[] args)
        {
            findcategory PER = new findcategory("PER"), ORG = new findcategory("ORG"), LOC = new findcategory("LOC");
            findcategory[] collect = {PER,ORG,LOC};
            string[] path = File.ReadAllLines(@"E:\CityU_Training_forNER.txt");
            foreach (string s in path)
            {
                if (s == "")
                    continue;
                string[] temp = s.Split(' ');
                for (int i = 0; i < 3; i++)
                    collect[i].find_category(temp[0], temp[3]);
            }
            
            for(int i=0 ; i<3 ; i++)
            {
                Console.WriteLine("PER: " + collect[i].getlength() + " " + collect[i].getlengest());
                foreach (KeyValuePair<int, int> item in collect[i].getlengdic())
                    Console.WriteLine(item.Key + " " + item.Value);
            }
            Console.WriteLine("我好了");
            Console.ReadKey();
        }
    }
}