using System.Collections.Generic;
using System.Linq;
using Take.SmartContacts.Utils.Extensions;

namespace Take.SmartContacts.Utils
{
    public class IntentAnalyzer : IIntentAnalyzer
    {
        private readonly IIntentResolver[] _intentResolvers = new IIntentResolver[]
        {
            new ExactMatchIntentResolver(),
            new IndexIntentResolver(),
            new SimilarityIntentResolver(),
            new NumberIntentResolver(),
        };

        public IntentAnalyzer()
        {
        }

        public IntentAnalyzer(IEnumerable<IIntentResolver> intentResolvers)
        {
            _intentResolvers = _intentResolvers.Concat(intentResolvers).ToArray();
        }

        public bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent)
        {
            input = SanitizeText(input);
            possibleIntents = possibleIntents.Select(SanitizeIntent);

            Intent tempIntent = default;
            var hasIntent = _intentResolvers.Any(r => r.TryGetIntent(input, possibleIntents, out tempIntent));

            intent = tempIntent;
            return hasIntent;
        }

        private static Intent SanitizeIntent(Intent intent)
        {
            return new Intent(intent.Index, SanitizeText(intent.Text), intent.Synonyms.Select(SanitizeText));
        }

        private static string SanitizeText(string target)
        {
            return target.ToLowerInvariant().RemoveEmojis().RemoveAccentuation().TrimBetween().Trim();
        }
    }
}