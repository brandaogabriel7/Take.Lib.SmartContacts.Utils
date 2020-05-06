using System.Linq;
using System.Text.RegularExpressions;

namespace Take.SmartContacts.Utils
{
    public class CpfValidator : IDataTypeValidator
    {
        private readonly Regex _cpfRgx = new Regex(@"\b((\d{3}([.,-]|\s*)){3}\d{2})\b", RegexOptions.Compiled);

        public DataType DataType => DataType.Cpf;

        public bool TryValidate(string input, out string cleanedDocument)
        {
            var couldExtract = TryExtractCpf(input, out cleanedDocument);

            if (!couldExtract)
            {
                return false;
            }

            var digits = cleanedDocument.Select(x => int.Parse(x.ToString())).ToArray();

            int firstVerifierDigit, secondVerifierDigit, firstAux = 10, secondAux = 11;

            return digits.Any(x => x != digits[0]) && // Check if all numbers are equal
                digits[9] == ((firstVerifierDigit = digits.Take(9).Sum(x => x * firstAux--) % 11) < 2 ? 0 : 11 - firstVerifierDigit) && // Check the first verification number
                    digits[10] == ((secondVerifierDigit = digits.Take(10).Sum(x => x * secondAux--) % 11) < 2 ? 0 : 11 - secondVerifierDigit); // Check the second verification number
        }

        public bool TryExtractCpf(string source, out string cpf)
        {
            var rgxResult = _cpfRgx.Match(source);

            cpf = new string(rgxResult.Value.Where(char.IsDigit).ToArray());

            return rgxResult.Success;
        }
    }
}