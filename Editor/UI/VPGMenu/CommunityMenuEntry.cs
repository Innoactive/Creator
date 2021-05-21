using UnityEditor;
using UnityEngine;

namespace VPG.Editor.VPGMenu
{
    internal static class CommunityMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Innoactive community.
        /// </summary>
        [MenuItem("VR Process Gizmo/Innoactive Help/Community", false, 80)]
        private static void OpenCommunityPage()
        {
            Application.OpenURL("https://innoactive.io/creator/community");
        }
    }
}