using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class CommunityMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Innoactive community
        /// </summary>
        [MenuItem("Innoactive/Help/Community", false, 80)]
        private static void OpenCommunityPage()
        {
            Application.OpenURL("https://innoactive.io/creator/community");
        }
    }
}
