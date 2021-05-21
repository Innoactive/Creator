using UnityEditor;
using UnityEngine;

namespace VPG.Editor.VPGMenu
{
    internal static class DocumentationMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Creator Documentation.
        /// </summary>
        [MenuItem("VR Process Gizmo/Innoactive Help/Documentation", false, 80)]
        private static void OpenDocumentation()
        {
            Application.OpenURL("https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/index.html");
        }
    }
}
