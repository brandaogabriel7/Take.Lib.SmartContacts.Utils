using System.Text.RegularExpressions;

namespace Take.SmartContacts.Utils
{
    public class MercosurCarPlateValidator : CarPlateValidator
    {
        public override DataType DataType => DataType.MercosurCarPlate;

        private readonly Regex _mercosurCarPlateRgx = new Regex(@"\b[A-Z]{3}([.,-]|\s)*(\d(\d[A-Z]|[A-Z]\d)\d)\b", RegexOptions.Compiled);

        public override Regex GetCarPlatePattern()
        {
            return _mercosurCarPlateRgx;
        }
    }
}