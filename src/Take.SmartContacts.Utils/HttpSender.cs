using Lime.Protocol;
using Lime.Protocol.Serialization;
using Lime.Protocol.Serialization.Newtonsoft;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;
using Take.Blip.Client.Extensions;
using Take.SmartContacts.Utils.Helpers;

namespace Take.SmartContacts.Utils
{
    public class HttpSender : ISender, IDisposable
    {
        private const string AuthenticationPrefix = "Key ";
        private const string MsgingBaseUri = "https://msging.net";

        private readonly IEnvelopeSerializer _envelopeSerializer;
        private readonly HttpClient _httpClient;

        public HttpSender(string authorizationKey, HttpClient httpClient)
        {
            authorizationKey = authorizationKey.Replace(AuthenticationPrefix, string.Empty);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationPrefix.Trim(), authorizationKey);

            var documentResolver = new DocumentTypeResolver().WithBlipDocuments();
            _envelopeSerializer = new EnvelopeSerializer(documentResolver);

            _httpClient = httpClient;
        }

        public HttpSender(string authorizationKey) : this(authorizationKey, new HttpClient())
        {
        }

        public HttpSender(string identifier, string accessKey, HttpClient httpClient) : this(BlipHelper.GetBotAuthorization(identifier, accessKey), httpClient)
        {
        }

        public HttpSender(string identifier, string accessKey) : this(identifier, accessKey, new HttpClient())
        {
        }

        public Task<Command> ProcessCommandAsync(Command requestCommand, CancellationToken cancellationToken)
        {
            return CommandRequestAsync("/commands", requestCommand, cancellationToken);
        }

        public Task SendCommandAsync(Command command, CancellationToken cancellationToken)
        {
            return ProcessCommandAsync(command, cancellationToken);
        }

        public Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            return RequestAsync("/messages", message, cancellationToken);
        }

        public Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken)
        {
            return RequestAsync("/notifications", notification, cancellationToken);
        }

        private async Task<Command> CommandRequestAsync(string endpoint, Envelope request, CancellationToken cancellationToken)
        {
            var response = await RequestAsync(endpoint, request, cancellationToken);
            return string.IsNullOrEmpty(response) ? default : _envelopeSerializer.Deserialize(response) as Command;
        }

        private async Task<string> RequestAsync(string endpoint, Envelope request, CancellationToken cancellationToken)
        {
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{MsgingBaseUri}{endpoint}"))
            {
                httpRequestMessage.Content = new StringContent(_envelopeSerializer.Serialize(request), Encoding.UTF8, "application/json");

                using (var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false))
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                    return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }

        private bool _disposedValue; // To detect redundant calls

        public void Dispose()
        {
            if (!_disposedValue)
            {
                _disposedValue = true;

                _httpClient?.Dispose();
            }
        }
    }
}