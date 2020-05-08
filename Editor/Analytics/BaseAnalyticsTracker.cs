using System;
using System.Globalization;
using UnityEditor;

namespace Innoactive.CreatorEditor.Analytics
{
    /// <summary>
    /// Abstract analytics handler, which handles the SessionId
    /// </summary>
    internal abstract class BaseAnalyticsTracker : IAnalyticsTracker
    {
        public const string KeySessionId = "Innoactive.Creator.Analytics.SessionID";

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
            sessionId = EditorPrefs.GetString(KeySessionId, null);
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                EditorPrefs.SetString(KeySessionId, sessionId);
            }

            return sessionId;
        }
    }
}
