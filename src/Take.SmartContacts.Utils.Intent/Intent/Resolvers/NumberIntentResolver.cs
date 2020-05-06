using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    internal class NumberIntentResolver : IIntentResolver
    {
        private const string RecognitionCulture = Culture.Portuguese;

        public bool TryGetIntent(string input, IEnumerable<Intent> possibleIntents, out Intent intent)
        {
            var result = new List<ModelResult>();

            result.AddRange(NumberRecognizer.RecognizeNumber(input, RecognitionCulture));
            result.AddRange(NumberRecognizer.RecognizeOrdinal(input, RecognitionCulture));

            intent = default;

            if (result.Count != 1)
            {
                return false;
            }

            var recognizedResolution = result[0].Resolution;

            if (recognizedResolution.TryGetValue("value", out var value) && int.TryParse(value?.ToString(), out var possibleIntentIndex))
            {
                intent = possibleIntents.FirstOrDefault(p => p.Index != default && p.Index == possibleIntentIndex);
            }

            return intent != default;
        }
    }
}