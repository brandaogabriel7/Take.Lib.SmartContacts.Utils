using Microsoft.Extensions.DependencyInjection;
using SmallTalks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmallTalks.Core.Models;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.EventTracker;
using Take.SmartContacts.Utils.Services.Smalltalks;
using Takenet.Iris.Messaging.Resources;

namespace Take.SmartContacts.Utils
{
    public class SmalltalksAnalyzer : ISmalltalksAnalyzer
    {
        private readonly ISmallTalksDetector _smallTalksDetector;

        public SmalltalksAnalyzer()
        {
            var services = ContainerProvider.RegisterTypes(new ServiceCollection());

            _smallTalksDetector = services.BuildServiceProvider().GetService<ISmallTalksDetector>();
        }

        public Task TrackAsync(IEventTrackExtension eventTrackExtension, SmallTalks.Core.Models.Analysis analysis, Func<string, EventTrack> eventTrackFactory, bool fireAndForget = default, CancellationToken cancellationToken = default)
        {
            var trackings = analysis.Matches.Select(m => eventTrackFactory.Invoke(m.SmallTalk));

            var trackingTasks = trackings.Select(t => eventTrackExtension.AddAsync(t.Category.Trim(), t.Action, extras: t.Extras,
                    contactIdentity: t.Contact?.Identity, fireAndForget: fireAndForget, cancellationToken: cancellationToken));

            return Task.WhenAll(trackingTasks);
        }

        public Task TrackAsync(ISender sender, SmallTalks.Core.Models.Analysis analysis, Func<string, EventTrack> eventTrackFactory, bool fireAndForget = default, CancellationToken cancellationToken = default)
        {
            return TrackAsync(new EventTrackExtension(sender), analysis, eventTrackFactory, fireAndForget, cancellationToken);
        }

        public Task<SmallTalks.Core.Models.Analysis> AnalyzeAsync(string input, InformationLevel informationLevel = default)
        {
            return _smallTalksDetector.DetectAsync(input, new SmallTalksPreProcessingConfiguration
            {
                ToLower = true,
                UnicodeNormalization = true,
                InformationLevel = informationLevel
            });
        }

        public async Task<SimplifiedSmalltalks> SimplifiedAnalyzeAsync(string input)
        {
            var analysis = await AnalyzeAsync(input, InformationLevel.NORMAL);
            return new SimplifiedSmalltalks
            {
                HasRelevantInput = analysis.UseCleanedInput == true,
                Smalltalks = analysis.Matches.Select(m => m.SmallTalk).ToDictionary(s => s, _ => true)
            };
        }
    }
}