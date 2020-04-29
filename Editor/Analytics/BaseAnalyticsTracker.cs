using System;
using System.Globalization;

namespace Innoactive.CreatorEditor.Analytics
{
    /// <summary>
    /// Abstract analytics handler, which handles the SessionId
    /// </summary>
    internal abstract class BaseAnalyticsTracker : IAnalyticsTracker
    {
        public const string KeySessionId = "SessionID";

        private string sessionId;
        public string SessionId
        {
            get
            {
                if (sessionId == null)
                {
                    SetSessionId();
                }

                return sessionId;
            }
        }

        public abstract void Send(AnalyticsEvent data);

        protected string GetLanguage()
        {
            return CultureInfo.InstalledUICulture.Name;
        }

        protected string SetSessionId()
        {
            sessionId = RegistryUtils.GetRegistryEntry<string>(RegistryUtils.AnalyticsEntry, KeySessionId, null);
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                RegistryUtils.SetRegistryEntry(RegistryUtils.AnalyticsEntry,KeySessionId, sessionId);
            }

            return sessionId;
        }
    }
}
