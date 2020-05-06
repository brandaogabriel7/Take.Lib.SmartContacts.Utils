using System;
using Lime.Protocol;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmallTalks.Core.Models;
using Take.Blip.Client.Extensions.EventTracker;
using Takenet.Iris.Messaging.Resources;
using Takenet.Iris.Messaging.Resources.Analytics;
using Xunit;

namespace Take.SmartContacts.Utils.Tests
{
    public class SmalltalksAnalyserTests
    {
        private readonly ISmalltalksAnalyzer _smalltalksAnalyzer;

        public SmalltalksAnalyserTests()
        {
            // Real instances
            _smalltalksAnalyzer = new SmalltalksAnalyzer();
        }

        [Fact]
        public async Task SmalltalksAnalysis_ReturnCorrect()
        {
            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("eai, meu cpf e esse.");

            Assert.Equal(1, analysis.MatchesCount);
            Assert.Equal("Greeting", analysis.Matches[0].SmallTalk);
        }

        [Fact]
        public async Task SmalltalksAnalysis_ReturnIncorrect()
        {
            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("teste retorna nada");

            Assert.Empty(analysis.Matches);
        }

        [Fact]
        public async Task SmalltalksAnalysis_ReturnMultiples()
        {
            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("oi, vlw.");

            Assert.Equal(2, analysis.MatchesCount);
        }

        [Fact]
        public async Task SmalltalksAnalysis_ReturnCorrectCount()
        {
            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("oi, vlw.");

            Assert.Equal(analysis.Matches.Count, analysis.MatchesCount);
        }

        [Fact]
        public async Task SmalltalksAnalysis_WithRelevantInput_ReturnCorrect()
        {
            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("oi, gostaria de pedir uma pizza.", InformationLevel.NORMAL);

            Assert.True(analysis.UseCleanedInput);
        }

        [Fact]
        public async Task SimplifiedSmalltalksAnalysis_WithRelevantInput_ReturnIncorrect()
        {
            var analysis = await _smalltalksAnalyzer.SimplifiedAnalyzeAsync("oi, gostaria de pedir uma pizza.");

            Assert.True(analysis.HasRelevantInput);
            Assert.NotEmpty(analysis.Smalltalks);
        }

        [Fact]
        public async Task SimplifiedSmalltalksAnalysis_WithRelevantInput_ReturnCorrect()
        {
            var analysis = await _smalltalksAnalyzer.SimplifiedAnalyzeAsync("oi, tudo bem?");

            Assert.False(analysis.HasRelevantInput);
            Assert.NotEmpty(analysis.Smalltalks);
        }

        [Fact]
        public async Task SmalltalksTracking_TrackCorrectQuantity()
        {
            // Mocks
            var eventTrackExtension = Substitute.For<IEventTrackExtension>();
            await eventTrackExtension
                .AddAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());

            var analysis = await _smalltalksAnalyzer.AnalyzeAsync("oi, vlw");

            await _smalltalksAnalyzer.TrackAsync(eventTrackExtension, analysis, e => new EventTrack
            {
                Category = "Small talks",
                Action = e,
                Contact = new EventContact
                {
                    Identity = Identity.Parse("user@msging.net")
                }
            });

            var receivedCalls = eventTrackExtension.Received(2).ReceivedCalls();

            Assert.NotEmpty(receivedCalls);
            Assert.Equal(2, receivedCalls.Count());
        }
    }
}