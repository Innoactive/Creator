namespace VPG.Editor.Analytics
{
    internal interface IAnalyticsTracker
    {
        /// <summary>
        /// Session id in use.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// Sends given data.
        /// </summary>
        void Send(AnalyticsEvent data);

        /// <summary>
        /// Send a start event.
        /// </summary>
        void SendSessionStart();
    }
}
