using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpellChecker
{
    public partial class Form1 : Form
    {
        bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }
        Dictionary<string, Word> dict = new Dictionary<string, Word>();
        Dictionary<char, List<char>> freqWrongTypeDict = new Dictionary<char, List<char>>();

        public Form1()
        {
            InitializeComponent();
            var wordInDat = new List<string>();
            var splitList = new List<char>();
            var texts = File.ReadLines("HOLBROOKDAT.643");
            //find split char
            foreach (var x in texts)
            {
                var t = x.ToLower();
                foreach (var c in t)
                {
                    if (!IsEnglishLetter(c))
                    {
                        splitList.Add(c);
                    }
                }
            }
            //tokenize
            foreach (var x in texts)
            {
                var t = x.ToLower().Split(splitList.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                wordInDat.AddRange(t);
            }
            //remove duplicate
            HashSet<string> tt = new HashSet<string>();
            foreach (var x in wordInDat)
            {
                tt.Add(x);
            }
            wordInDat = tt.ToList();
            //save correct word
            foreach (var x in wordInDat)
            {
                dict.Add(x, new Word(x, true));
            }
            //Load telemakrDat
            TelemarkDat.LoadTelemarkDat(dict);
            freqWrongTypeDict= TelemarkDat.GetFreqWrongTypeChar();
            //generate wrong char
            foreach (var right in wordInDat)
            {
                foreach (var wrong in SwapOneChar(right).Concat(DeleteOneChar(right)).Concat(WrongOneChar(right)))
                {
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
            this.Text = "Dict size = " + dict.Count;
        }

        List<string> DeleteOneChar(string a)
        {
            var output = new List<string>();
            for (int i = 0; i < a.Length; i++)
            {
                output.Add(a.Remove(i, 1));
            }
            return output;
        }

        List<string> SwapOneChar(string a)
        {
            var output = new List<string>();
            for (int i = 0; i < a.Length - 1; i++)
            {
                var t = a.ToCharArray();
                char temp = t[i];
                t[i] = t[i + 1];
                t[i + 1] = temp;
                output.Add(new string(t));
            }
            return output;
        }

        List<string> WrongOneChar(string a)
        {
            var output = new List<string>();
            var temp = a.ToCharArray();
            for (int i = 0; i < a.Length; i++)
            {
                char oldchar = temp[i];
                if (freqWrongTypeDict.ContainsKey(oldchar))
                {
                    foreach (char wrongChar in freqWrongTypeDict[oldchar])
                    {
                        temp[i] = wrongChar;
                        output.Add(new string(temp));
                    }
                }

            }
            return output;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            string text = richTextBox2.Text.ToLower();

            if (text == "")
            {
                richTextBox1.Text = "Please type some sentence";
                return;
            }
            else
                GenerateOutput();
        }

        void GenerateOutput()
        {
            richTextBox1.Text = "";
            var text = richTextBox2.Text.ToLower();     
            //split text in input
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var words = lines[i].Split(' ');
                foreach (var wordWithNoneEngChar in words)
                {
                    var word = RemoveNoneEnglishChar(wordWithNoneEngChar);
                    if (word == "" || word == "\n") continue;
                    int wordCol = lines[i].IndexOf(word);
                    if (dict.ContainsKey(word))
                    {
                        if (!dict[word].Correct)
                        {
                            foreach (var x in SuggestCorrectWord(word))
                            {
                                if (x != "\n" && x != "")
                                    richTextBox1.Text += $"Line {i + 1},{wordCol} : {word} maybe {x}\n";
                            }
                        }
                    }
                    else
                    {
                        richTextBox1.Text += $"Line {i + 1},{wordCol} : don't know {word}\n";
                    }
                }

            }

        }

        string[] SuggestCorrectWord(string word)
        {
            var output = new List<string>();
            foreach (var x in dict[word].CorrectWords)
            {
                output.Add(x);
            }
            return output.ToArray();
        }

        string RemoveNoneEnglishChar(string text)
        {
            char[] temp = text.ToCharArray();
            List<char> output = new List<char>();
            for (int i = 0; i < temp.Length; i++)
            {
                if(IsEnglishLetter(temp[i]))
                {
                    output.Add(temp[i]);
                }
            }
            return new string(output.ToArray());
        }
    }
}
