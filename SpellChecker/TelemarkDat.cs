using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    static class TelemarkDat
    {
        public static void LoadTelemarkDat(Dictionary<string,Word> dict)
        {
            var texts = File.ReadLines("TELEMARKDAT.643");
            char[] dontwant = { '_', '-' };
            foreach(var line in texts)
            {
                if (line.IndexOfAny(dontwant) > -1) continue;
                var temp = line.Split(' ');
                string wrong = temp[0];
                string right = temp[1];

                if (!dict.ContainsKey(wrong))
                {
                    dict.Add(wrong, new Word(wrong, false, right));
                }
                else
                {
                    if (!dict[wrong].Correct)
                    {
                        dict[wrong].AddCorrectWord(right);
                    }
                }
            }
        }

        public static Dictionary<char, List<char>> GetFreqWrongTypeChar()
        {
            var freqWrongTypeDict = new Dictionary<char, List<char>>();
            var texts = File.ReadLines("TELEMARKDAT.643");
            char[] dontwant = { '_', '-' };
            foreach (var line in texts)
            {
                if (line.IndexOfAny(dontwant) > -1) continue;
                var temp = line.Split(' ');
                string wrong = temp[0];
                string right = temp[1];
                if(wrong.Length == right.Length && EditDistance(wrong,right)==1)
                {
                    for(int i = 0; i < wrong.Length; i++)
                    {
                        if (wrong[i] == right[i]) continue;
                        if (!freqWrongTypeDict.ContainsKey(right[i]))
                        {
                            var t = new List<char>();
                            t.Add(wrong[i]);
                            freqWrongTypeDict.Add(right[i], t);
                            Console.WriteLine($"{wrong[i]} to {right[i]}");
                        }
                        else if(!freqWrongTypeDict[right[i]].Contains(wrong[i]))
                        {
                            freqWrongTypeDict[right[i]].Add(wrong[i]);
                            Console.WriteLine($"{wrong[i]} to {right[i]}");
                        }
                    }
                   
                }
            }
            return freqWrongTypeDict;
        }

         static int EditDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
