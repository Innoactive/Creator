using System.Net.Http;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    /// <summary>
    /// Sends data to Google Analytics.
    /// </summary>
    internal class GoogleTracker : BaseAnalyticsTracker
    {
        private readonly HttpClient client = new HttpClient();

        private readonly string agent = $"Unity-{Application.unityVersion}";

        public override void Send(AnalyticsEvent data)
        {
            PostData(BuildEventUri(data));
        }

        private void PostData(string uri)
        {
            if (AnalyticsUtils.GetTrackingState() == AnalyticsState.Enabled)
            {
                client.GetAsync(uri);
            }
        }

        private string BuildEventUri(AnalyticsEvent data)
        {
            string uri = GetBaseUri() + "&t=event&ec={0}&ea={1}&el={2}";
            return string.Format(uri, data.Category, data.Action, data.Label);
        }

        public override void SendSessionStart()
        {
            PostData(GetBaseUri() + "&sc=start");
        }

        private string GetBaseUri()
        {

            string baseUri = "https://www.google-analytics.com/collect?v=1&tid=UA-109665637-9&cid={0}&aip=1&npa=1&ul={1}&an=Creator&av={2}&ua={3}&ds=unity";
#if CREATOR_PRO
            if (CreatorPro.Account.UserAccount.IsAccountLoggedIn())
            {
                baseUri += $"&uid={CreatorPro.Account.UserAccount.GetId()}";
            }
#endif
            return string.Format(baseUri, SessionId, GetLanguage(), EditorUtils.GetCoreVersion(), agent);
        }
    }
}
