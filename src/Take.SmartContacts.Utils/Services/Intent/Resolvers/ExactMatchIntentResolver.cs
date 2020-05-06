using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    internal class ExactMatchIntentResolver : IIntentResolver
    {
        public bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent)
        {
            input = Sanitize(input);
            intent = possibleIntents.FirstOrDefault(r => input.Contains(Sanitize(r.Text)) || r.Synonyms.Any(s => input.Contains(Sanitize(s))));
            return intent != default;
        }

        private static string Sanitize(string target)
        {
            return $" {target} ";
        }
    }
}