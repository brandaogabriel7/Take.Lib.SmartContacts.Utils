using System.Linq;
using System.Text.RegularExpressions;

namespace Take.SmartContacts.Utils
{
    public class CnpjValidator : IDataTypeValidator
    {
        private readonly Regex _cnpjRgx = new Regex(@"\b[0-9]{2}([.,-]|\s*)[0-9]{3}([.,-]|\s*)[0-9]{3}([.,-\/]|\s*)[0-9]{4}([.,-]|\s*)[0-9]{2}\b", RegexOptions.Compiled);
        private readonly int[] _validatorMultipliers = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        public DataType DataType => DataType.Cnpj;

        public bool TryValidate(string input, out string cleanedDocument)
        {
            var couldExtract = TryExtractCnpj(input, out cleanedDocument);

            if (!couldExtract)
            {
                return false;
            }

            var digits = cleanedDocument.Select(x => int.Parse(x.ToString())).ToArray();

            int firstVerifierDigit, secondVerifierDigit;

            return digits[12] == ((firstVerifierDigit = digits.Take(12).Select((d, i) => d * _validatorMultipliers[i + 1]).Sum() % 11) < 2 ? 0 : 11 - firstVerifierDigit) && // Check the first verification number
                digits[13] == ((secondVerifierDigit = digits.Take(13).Select((d, i) => d * _validatorMultipliers[i]).Sum() % 11) < 2 ? 0 : 11 - secondVerifierDigit); // Check the second verification number
        }

        public bool TryExtractCnpj(string source, out string cnpj)
        {
            var rgxResult = _cnpjRgx.Match(source);

            cnpj = new string(rgxResult.Value.Where(char.IsDigit).ToArray());

            return rgxResult.Success;
        }
    }
}