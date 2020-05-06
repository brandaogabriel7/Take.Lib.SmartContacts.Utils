using F23.StringSimilarity;
using F23.StringSimilarity.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    internal class SimilarityIntentResolver : IIntentResolver
    {
        private const double MinimumSimilarityScore = 0.6;

        private readonly IStringSimilarity[] _stringSimilarities = {
            new Cosine()
        };

        public bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent)
        {
            var similarities = possibleIntents.Select(p =>
            {
                var compareTexts = p.Synonyms.Concat(new[] { p.Text });
                var similarity = compareTexts.Max(t => _stringSimilarities.Average(s => s.Similarity(input, t)));

                return (intent: p, similarity);
            });

            var mostSimilar = similarities.OrderByDescending(s => s.similarity).First();

            intent = mostSimilar.intent;

            return mostSimilar.similarity > MinimumSimilarityScore;
        }
    }
}