using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace VPG.Editor.TestTools
{
    /// <summary>
    /// Utility window which sends given sequence of events to another window.
    /// </summary>
    internal static class EditorWindowTestPlayer
    {
        /// <summary>
        /// Start sending <paramref name="recordedActions"/> to the <see cref="window"/> and invoke <paramref name="finishedCallback"/> when done.
        /// </summary>
        public static void StartPlayback(EditorWindow window, IList<UserAction> recordedActions)
        {
            foreach (UserAction action in recordedActions)
            {
                TestableEditorElements.StartPlayback(action.PrepickedSelections);
                window.RepaintImmediately();
                window.SendEvent(action.Event);
                TestableEditorElements.StopPlayback();
            }
        }
    }
}
