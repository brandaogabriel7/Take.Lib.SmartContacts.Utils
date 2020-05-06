using System;
using System.Threading;
using System.Threading.Tasks;
using SmallTalks.Core.Models;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.EventTracker;
using Take.SmartContacts.Utils.Services.Smalltalks;
using Takenet.Iris.Messaging.Resources;

namespace Take.SmartContacts.Utils
{
    public interface ISmalltalksAnalyzer
    {
        Task TrackAsync(IEventTrackExtension eventTrackExtension, SmallTalks.Core.Models.Analysis analysis, Func<string, EventTrack> eventTrackFactory, bool fireAndForget = default, CancellationToken cancellationToken = default);

        Task TrackAsync(ISender sender, SmallTalks.Core.Models.Analysis analysis, Func<string, EventTrack> eventTrackFactory, bool fireAndForget = default, CancellationToken cancellationToken = default);

        Task<SmallTalks.Core.Models.Analysis> AnalyzeAsync(string input, InformationLevel informationLevel = default);

        Task<SimplifiedSmalltalks> SimplifiedAnalyzeAsync(string input);
    }
}