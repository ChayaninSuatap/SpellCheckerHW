using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    class Word
    {
        public string Text{ get; private set; }
        public bool Correct { get; private set; }
        public List<string> CorrectWords;
        public Word(string text, bool correct, string correctWord = null)
        {
            Text = text;
            Correct = correct;
            CorrectWords = new List<string>();
            if(correctWord != null)
            {
                CorrectWords.Add(correctWord);
            }
        }

        public void AddCorrectWord(string word)
        {
            if(!CorrectWords.Contains(word))
                CorrectWords.Add(word);
        }
    }
}
