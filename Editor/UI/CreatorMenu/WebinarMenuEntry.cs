using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class WebinarMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to webinar.
        /// </summary>
        [MenuItem("Innoactive/Help/Webinar", false, 80)]
        private static void OpenWebinar()
        {
            Application.OpenURL("https://vimeo.com/417328541/93a752e72c");
        }
    }
}
