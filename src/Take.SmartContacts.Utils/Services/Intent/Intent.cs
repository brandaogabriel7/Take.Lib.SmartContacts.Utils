
using System;
using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    public class Intent
    {
        public int Index { get; set; }

        public string Text { get; set; }

        public IEnumerable<string> Synonyms { get; set; }

        public Intent(int index, string text, IEnumerable<string> synonyms)
        {
            Index = index;
            Text = text;
            Synonyms = synonyms;
        }

        public Intent(int index, string text, params string[] synonyms) : this(index, text, synonyms.AsEnumerable())
        {
        }

        public Intent(int index, string text) : this(index, text, Array.Empty<string>())
        {
        }

        public Intent() : this(default, default)
        {
        }
    }
}