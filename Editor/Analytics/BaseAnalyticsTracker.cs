using System;
using System.Globalization;
using UnityEditor;

namespace VPG.Editor.Analytics
{
    /// <summary>
    /// Abstract analytics handler, which handles the SessionId
    /// </summary>
    internal abstract class BaseAnalyticsTracker : IAnalyticsTracker
    {
        public const string KeySessionId = "Innoactive.Creator.Analytics.SessionID";

        public string SessionId { get; }

        internal BaseAnalyticsTracker()
        {
            if (EditorPrefs.HasKey(KeySessionId))
            {
                SessionId = EditorPrefs.GetString(KeySessionId);
            }
            else
            {
                SessionId = Guid.NewGuid().ToString();
            }
        }

        public abstract void Send(AnalyticsEvent data);

        public abstract void SendSessionStart();

        protected string GetLanguage()
        {
            return CultureInfo.InstalledUICulture.Name;
        }
    }
}
