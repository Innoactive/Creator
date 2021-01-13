using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class SupportMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Innoactive's Jira Servicedesk.
        /// </summary>
        [MenuItem("Innoactive/Help/Support", false, 80)]
        private static void OpenSupportPage()
        {
            Application.OpenURL("https://innoactive.io/creator/support");
        }

    }
}
