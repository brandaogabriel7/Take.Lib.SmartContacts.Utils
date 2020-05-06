using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    internal class IndexIntentResolver : IIntentResolver
    {
        public bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent)
        {
            intent = default;

            if (int.TryParse(input, out var parsedInput))
            {
                intent = possibleIntents.SingleOrDefault(p => p.Index != default && p.Index == parsedInput);
                return intent != default;
            }

            return false;
        }
    }
}