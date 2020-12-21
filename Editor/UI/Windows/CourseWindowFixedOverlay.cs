/* Copyright (C) Innoactive GmbH - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Innoactive GmbH, November 2020
 */

using Innoactive.CreatorEditor.UI;
using Innoactive.CreatorPro.Account;
using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor;


namespace Innoactive.Creator.Core
{
    internal class CourseWindowFixedOverlay : IEditorGraphicDrawer
    {
        public int Priority { get; } = 100;

        private Texture2D backgroundTexture;

        public CourseWindowFixedOverlay()
        {
            backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.3f));
            backgroundTexture.Apply();
        }

        public void Draw(Rect windowRect)
        {
            IChapter c = GlobalEditorHandler.GetCurrentChapter();
            if(c != null && (c.Data.FirstStep == null && c.Data.Steps.Count == 0))
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = new Color(1, 1, 1, 0.70f);
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 20;

                GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
                backgroundStyle.normal.background = backgroundTexture;

                GUI.Box(new Rect(windowRect.x + (windowRect.width / 2) - 260, windowRect.y + (windowRect.height / 2) - 20, 420, 40), "", backgroundStyle);
                GUI.Label(new Rect(windowRect.x + (windowRect.width / 2) - 260, windowRect.y + (windowRect.height / 2 - 20), 420, 40), "Right-click to create new step", style);
            }
            
                
            
        }
    }
}
