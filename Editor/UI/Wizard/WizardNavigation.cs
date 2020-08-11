using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    internal class WizardNavigation
    {
        private const float PaddingTop = 4f;

        private List<IWizardNavigationEntry> Entries { get; }

        public WizardNavigation(List<IWizardNavigationEntry> entries)
        {
            Entries = entries;
        }

        protected float EntryHeight { get; set; } = 32f;

        public void SetSelected(int position)
        {
            Entries.ForEach(entry => entry.Selected = false);
            Entries[position].Selected = true;
        }

        public void Draw(Rect window)
        {
            // Draw darker area at the bottom
            EditorGUI.DrawRect(new Rect(window.position, new Vector2(window.width, PaddingTop)), WizardWindow.LineColor);

            for (int position = 0; position < Entries.Count; position++)
            {
                IWizardNavigationEntry entry = Entries[position];
                entry.Draw(GetEntryRect(position, window.width));
            }

            // Draw darker area at the bottom
            EditorGUI.DrawRect(new Rect(0, Entries.Count * EntryHeight + 1 + PaddingTop, window.width, window.height - 1 - Entries.Count * EntryHeight), WizardWindow.LineColor);
        }

        protected Rect GetEntryRect(int position, float width)
        {
            return new Rect(0, PaddingTop + (position * EntryHeight), width, EntryHeight);
        }

        internal class Entry : IWizardNavigationEntry
        {
            public string Name { get; }

            public bool Selected { get; set; } = false;

            public Entry(string name)
            {
                Name = name;
            }

            public void Draw(Rect window)
            {
                if (!Selected)
                {
                    EditorGUI.DrawRect(new Rect(0, window.y + 1, window.width, window.height - 1), WizardWindow.LineColor);
                }
                EditorGUI.LabelField(new Rect(4, window.y + 4, window.width - 8, window.height - 8), Name);
            }
        }
    }
}
