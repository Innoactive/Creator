/* Copyright (C) Innoactive GmbH - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Innoactive GmbH, November 2020
 */

using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class DocumentationMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Creator Documentation.
        /// </summary>
        [MenuItem("Innoactive/Help/Documentation", false, 80)]
        private static void OpenDocumentation()
        {
            Application.OpenURL("https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/index.html");
        }
    }
}
