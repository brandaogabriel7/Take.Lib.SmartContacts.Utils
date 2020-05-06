using System.Linq;
using System.Text.RegularExpressions;

namespace Take.SmartContacts.Utils
{
    public class CarPlateValidator : IDataTypeValidator
    {
        private readonly Regex _carPlateRgx = new Regex(@"\b[A-Z]{3}([.,-]|\s)*\d{4}\b", RegexOptions.Compiled);

        public virtual DataType DataType => DataType.CarPlate;

        public bool TryValidate(string input, out string cleanedData)
        {
            var rgxResult = GetCarPlatePattern().Match(input.ToUpperInvariant());

            cleanedData = rgxResult.Value.Trim();
            cleanedData = new string(cleanedData.Where(char.IsLetterOrDigit).ToArray());

            return rgxResult.Success;
        }

        public virtual Regex GetCarPlatePattern()
        {
            return _carPlateRgx;
        }
    }
}