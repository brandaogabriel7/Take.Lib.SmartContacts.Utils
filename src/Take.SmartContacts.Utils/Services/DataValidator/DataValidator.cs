using System.Collections.Generic;
using System.Linq;

namespace Take.SmartContacts.Utils
{
    public class DataValidator : IDataValidator
    {
        private readonly IDataTypeValidator[] _dataTypeValidators = new IDataTypeValidator[]
        {
            new CnpjValidator(),
            new CpfValidator(),
            new CarPlateValidator(),
            new MercosurCarPlateValidator()
        };

        public DataValidator()
        {
        }

        public DataValidator(IEnumerable<IDataTypeValidator> dataTypeValidators)
        {
            _dataTypeValidators = _dataTypeValidators.Concat(dataTypeValidators).ToArray();
        }

        public bool IsValid(string input, DataType types, out ValidatedData validatedData)
        {
            var validators = _dataTypeValidators.Where(v => (types & v.DataType) != 0);

            ValidatedData tempValidatedData = default;
            var isValid = validators.Any(v =>
            {
                var validWithCurrentValidator = v.TryValidate(input, out var tempCleannedInput);
                tempValidatedData = new ValidatedData
                {
                    CleanedData = tempCleannedInput,
                    UsedDataType = v.DataType
                };

                return validWithCurrentValidator;
            });

            if (!isValid && string.IsNullOrEmpty(tempValidatedData.CleanedData))
            {
                tempValidatedData.UsedDataType = DataType.None;
            }

            validatedData = tempValidatedData;

            return isValid;
        }
    }
}