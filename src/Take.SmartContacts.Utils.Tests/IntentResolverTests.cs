using Xunit;

namespace Take.SmartContacts.Utils.Tests
{
    public class IntentResolverTests
    {
        private readonly IIntentAnalyzer _intentAnalyzer;

        public IntentResolverTests()
        {
            _intentAnalyzer = new IntentAnalyzer();
        }

        [Fact]
        public void IntentAnalysis_ReturnCorrect()
        {
            var hasFound = _intentAnalyzer.TryGetIntent("quero a segunda.", new Intent[]
            {
                new Intent(1, "quero fazer a"),
                new Intent(2, "quero fazer b"),
                new Intent(3, "quero fazer c"),
                new Intent(4, "quero fazer d"),
            }, out var intent);

            Assert.True(hasFound);
            Assert.Equal(2, intent.Index);
        }

        [Fact]
        public void IntentAnalysis_ReturnIncorrect()
        {
            var hasFound = _intentAnalyzer.TryGetIntent("nao pode retornar nada", new Intent[]
            {
                new Intent(1, "quero fazer a"),
                new Intent(2, "quero fazer b"),
                new Intent(3, "quero fazer c"),
                new Intent(4, "quero fazer d"),
            }, out _);

            Assert.False(hasFound);
        }

        [Theory]
        [InlineData("quero pegar a segunda opcao", 2)]
        [InlineData("quero a opcao tres", 3)]
        [InlineData("quero a 2", 2)]
        [InlineData("quero pegar segunda via de um boleto", 1)]
        [InlineData("gostaria de reservar um quarto por favor", 3)]
        [InlineData("4", 4)]
        public void IntentAnalysis_ShouldReturnCorrect(string input, int expectedIndex)
        {
            var hasFound = _intentAnalyzer.TryGetIntent(input, new[]
            {
                new Intent(1, "segunda via do boleto"),
                new Intent(2, "consultar meu saldo"),
                new Intent(3, "reservar um quarto"),
                new Intent(4, "pegar um cafe para mim"),
            }, out var intent);

            Assert.True(hasFound);
            Assert.Equal(expectedIndex, intent.Index);
        }

        [Theory]
        [InlineData("nao quero nenhuma opcao")]
        [InlineData("nao vou querer nada")]
        [InlineData("batata")]
        [InlineData("nao me ajudou")]
        public void IntentAnalysis_ShouldReturnIncorrect(string input)
        {
            var hasFound = _intentAnalyzer.TryGetIntent(input, new[]
            {
                new Intent(1, "segunda via do boleto"),
                new Intent(2, "consultar meu saldo"),
                new Intent(3, "reservar um quarto"),
                new Intent(4, "pegar um cafe para mim"),
            }, out _);

            Assert.False(hasFound);
        }

        [Theory]
        [InlineData("Estou satisfeito", 4)]
        [InlineData("To insatisfeito", 2)]
        [InlineData("Achei master!", 5)]
        public void IntentAnalysis_Complex_ShouldReturnCorrect(string input, int expectedIndex)
        {
            var hasFound = _intentAnalyzer.TryGetIntent(input, new[]
            {
                new Intent(1, "😡 Totalmente insatisfeito", "pessimo", "horrivel", "porcaria", "muito ruim", "uma merda", "lixo"),
                new Intent(2, "😟 Insatisfeito", "ruim", "nao gostei"),
                new Intent(3, "😐 Adequado", "mais ou menos", "razoavel", "legalzinho"),
                new Intent(4, "🙂 Satisfeito", "bom", "gostei", "legal"),
                new Intent(5, "😁 Totalmente satisfeito", "otimo", "excelente", "super gostei", "master")
            }, out var intent);

            Assert.True(hasFound);
            Assert.Equal(expectedIndex, intent.Index);
        }
    }
}