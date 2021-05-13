using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.CreatorMenu
{
    internal static class WebinarMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to webinar.
        /// </summary>
        [MenuItem("VR Process Gizmo/Innoactive Help/Webinar", false, 80)]
        private static void OpenWebinar()
        {
            Application.OpenURL("https://vimeo.com/417328541/93a752e72c");
        }
    }
}
