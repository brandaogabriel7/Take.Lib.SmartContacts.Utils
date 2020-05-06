using Lime.Protocol;

namespace Take.SmartContacts.Utils.Helpers
{
    internal static class BlipHelper
    {
        internal static string GetBotAuthorization(string identifier, string accessKey)
        {
            // Check if is a full identity
            if (Identity.TryParse(identifier, out var parsedIdentity))
            {
                // Get name if is a full identity
                identifier = parsedIdentity.Name;
            }

            return $"{identifier}:{accessKey.FromBase64()}".ToBase64();
        }
    }
}