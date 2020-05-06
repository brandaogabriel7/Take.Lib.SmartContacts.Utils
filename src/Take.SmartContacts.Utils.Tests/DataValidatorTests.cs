using Xunit;

namespace Take.SmartContacts.Utils.Tests
{
    public class DataValidatorTests
    {
        private readonly IDataValidator _validator;

        public DataValidatorTests()
        {
            _validator = new DataValidator();
        }

        [Fact]
        public void ValidDocument_ReturnsCorrect()
        {
            var isValid = _validator.IsValid("meu cpf e 012.345678-90 ok?", DataType.Cpf, out var document);

            Assert.True(isValid);
            Assert.Equal("01234567890", document.CleanedData);
        }

        [Fact]
        public void ValidDocumentWithMultiplesValidators_ReturnsCorrect()
        {
            var isValid = _validator.IsValid("meu cpf e 012.345678-90 ok?", DataType.Cpf | DataType.Cnpj, out var document);

            Assert.True(isValid);
            Assert.Equal("01234567890", document.CleanedData);
        }

        [Fact]
        public void InvalidDocumentWithMultiplesValidators_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("meu cpf e 012.345678-92 ok?", DataType.Cpf | DataType.Cnpj, out var document);

            Assert.False(isValid);
            Assert.Equal("01234567892", document.CleanedData);
        }

        [Fact]
        public void ValidDocumentWithInvalidValidator_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("meu cpf e 012.345678-90 ok?", DataType.Cnpj, out var document);

            Assert.False(isValid);
        }

        [Fact]
        public void InvalidDocument_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("meu cpf e 012.345678-92 ok?", DataType.Cpf, out var document);

            Assert.False(isValid);
            Assert.Equal(DataType.Cpf, document.UsedDataType);
            Assert.Equal("01234567892", document.CleanedData);
        }

        [Fact]
        public void NoDocument_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("to sem meu cpf", DataType.Cpf, out var document);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidCarPlate_ReturnsCorrect()
        {
            var isValid = _validator.IsValid("minha placa é abc1234", DataType.CarPlate, out var plate);

            Assert.True(isValid);
            Assert.Equal(DataType.CarPlate, plate.UsedDataType);
            Assert.Equal("ABC1234", plate.CleanedData);
        }

        [Fact]
        public void ValidCarPlate_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("minha placa é abc1234", DataType.MercosurCarPlate, out var plate);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidMercosurCarPlate_ReturnsCorrect()
        {
            var isValid = _validator.IsValid("minha placa é PXY6A73", DataType.MercosurCarPlate, out var plate);

            Assert.True(isValid);
            Assert.Equal(DataType.MercosurCarPlate, plate.UsedDataType);
            Assert.Equal("PXY6A73", plate.CleanedData);
        }

        [Fact]
        public void ValidMercosurCarPlate_ReturnsIncorrect()
        {
            var isValid = _validator.IsValid("minha placa é PXY6A73", DataType.CarPlate, out var plate);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("placa é pxt6273", true)]
        [InlineData("essa é a placa pxt 6573", true)]
        [InlineData("minha placa é abc-9874", true)]
        [InlineData("minha placa é pxt6A73", false)]
        [InlineData("placa é pxt6a73", false)]
        [InlineData("essa é minha placa: pxt6aa3", false)]
        [InlineData("p5t6A73", false)]
        public void ValidCarPlate_ShouldReturnIfIsValid(string input, bool expectedIsValid)
        {
            var isValid = _validator.IsValid(input, DataType.CarPlate, out var plate);

            Assert.Equal(expectedIsValid, isValid);
        }

        [Theory]
        [InlineData("minha placa é pxt6A73", true)]
        [InlineData("placa é pxt6a73", true)]
        [InlineData("essa é minha placa: pxt6aa3", false)]
        [InlineData("p5t6A73", false)]
        [InlineData("minha placa é pxt6273", false)]
        [InlineData("minha placa é pxt 6573", false)]
        [InlineData("minha placa é abc 9874", false)]
        public void ValidMercosurCarPlate_ShouldReturnIfIsValid(string input, bool expectedIsValid)
        {
            var isValid = _validator.IsValid(input, DataType.MercosurCarPlate, out var plate);

            Assert.Equal(expectedIsValid, isValid);
        }

        [Theory]
        [InlineData("minha placa é pxt6A73", DataType.MercosurCarPlate, true)]
        [InlineData("placa é pxt6a73", DataType.MercosurCarPlate, true)]
        [InlineData("essa é minha placa: pxt6aa3", DataType.None, false)]
        [InlineData("p5t6A73", DataType.None, false)]
        [InlineData("minha placa é pxt6273", DataType.CarPlate, true)]
        [InlineData("minha placa é pxt 6573", DataType.CarPlate, true)]
        [InlineData("minha placa é abc 9874", DataType.CarPlate, true)]
        [InlineData("meu cpnj é 76236060000170", DataType.Cnpj, true)]
        [InlineData("cpnj é 61.817.061/0001-30", DataType.Cnpj, true)]
        [InlineData("61817061/0001-30", DataType.Cnpj, true)]
        [InlineData("meu cnpj é 7623606000017", DataType.None, false)]
        [InlineData("batata", DataType.None, false)]

        public void ValidCarPlateAndCNPJ_ShouldReturnIfIsValid(string input, DataType expectDataType, bool expectedIsValid)
        {
            var isValid = _validator.IsValid(input, DataType.CarPlate | DataType.MercosurCarPlate | DataType.Cnpj, out var cleanedValidatedData);

            Assert.Equal(expectedIsValid, isValid);
            Assert.Equal(expectDataType, cleanedValidatedData.UsedDataType);
        }
    }
}