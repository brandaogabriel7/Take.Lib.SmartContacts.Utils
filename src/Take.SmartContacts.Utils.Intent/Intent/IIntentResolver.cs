using System.Collections.Generic;

namespace Take.SmartContacts.Utils
{
    public interface IIntentResolver
    {
        bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent);
    }
}