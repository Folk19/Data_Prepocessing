using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace temp
{
    class findcategory
    {
        Boolean pre = false, now = false, begin = false;
        int length = 0;
        string category="",temp = "",lengest = "";
        Dictionary<int, int> total = new Dictionary<int, int>();
        StreamWriter sw;

        public findcategory(string c)
        {
            category = c;
            sw = new StreamWriter(@"E:\CityU_Training_forNER_"+ c +".txt");
        }
        public void find_category(string word,string categ)
        {
            pre = now;
            if (categ.EndsWith(category))
            {
                if (now && categ.StartsWith("B-"))
                {
                    sw.WriteLine(temp);
                    lengest = length < temp.Length ? temp : lengest;
                    length = length < temp.Length ? temp.Length : length;
                    if (total.ContainsKey(temp.Length))
                        total[temp.Length] += 1;
                    else
                        total.Add(temp.Length, 1);
                    temp = "";
                }
                temp += word;
                now = true;
            }
            else
                now = false;
            if(!now && pre)
            {
                sw.WriteLine(temp);
                lengest = length < temp.Length ? temp : lengest;
                length = length < temp.Length ? temp.Length : length;
                if (total.ContainsKey(temp.Length))
                    total[temp.Length] += 1;
                else
                    total.Add(temp.Length, 1);
                temp = "";
            }
        }
        public int getlength()
        {
            sw.Close();
            return length;
        }
        public string getlengest()
        {
            return lengest;
        }
        public Dictionary<int,int> getlengdic()
        {
            return total;
        }
    }
}
