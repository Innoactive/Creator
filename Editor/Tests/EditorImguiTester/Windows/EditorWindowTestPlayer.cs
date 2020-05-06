using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.ImguiTester;

namespace Innoactive.CreatorEditor.TestTools
{
    /// <summary>
    /// Utility window which sends given sequence of events to another window.
    /// </summary>
    public class EditorWindowTestPlayer : EditorWindow
    {
        public class FinishedEventArgs : EventArgs
        {
            public EditorWindow Result { get; private set; }

            public FinishedEventArgs(EditorWindow result)
            {
                Result = result;
            }
        }

        public delegate void FinishedHandler(object sender, FinishedEventArgs e);

        public event FinishedHandler Finished;

        public static bool IsPlaying { get; private set; }

        private EditorWindow window;

        /// <summary>
        /// Start sending <paramref name="recordedActions"/> to the <see cref="window"/> and invoke <paramref name="finishedCallback"/> when done.
        /// </summary>
        public static void StartPlayback(EditorWindow window, IList<UserAction> recordedActions, FinishedHandler finishedCallback)
        {
            foreach (EditorWindowTestPlayer testPlayer in Resources.FindObjectsOfTypeAll<EditorWindowTestPlayer>())
            {
                testPlayer.Close();
            }

            IsPlaying = true;

            EditorWindowTestPlayer player = CreateInstance<EditorWindowTestPlayer>();
            player.ShowUtility();
            player.Finished += finishedCallback;
            player.window = window;

            foreach (UserAction action in recordedActions)
            {
                TestableEditorElements.StartPlayback(action.PrepickedSelections);
                player.window.SendEvent(action.Event);
                player.SendEvent(EditorGUIUtility.CommandEvent("Wait"));
            }

            player.SendEvent(EditorGUIUtility.CommandEvent("Finished"));
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Wait")
            {
                TestableEditorElements.StopPlayback();
                Event.current.Use();
            }

            if (Event.current.type != EventType.ExecuteCommand)
            {
                return;
            }

            if (Event.current.commandName != "Finished")
            {
                return;
            }

            if (Finished != null)
            {
                Finished(this, new FinishedEventArgs(window));
            }

            window.Close();
            Close();
        }
    }
}
