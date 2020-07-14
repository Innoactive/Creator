using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.TestTools
{
    /// <summary>
    /// Utility window which sends given sequence of events to another window.
    /// </summary>
    internal class EditorWindowTestPlayer : EditorWindow
    {
        /// <summary>
        /// Event args for event which is fired when <see cref="EditorWindowTestPlayer"/> finished playing a <see cref="IEditorImguiTest"/>.
        /// </summary>
        public class FinishedEventArgs : EventArgs
        {
            /// <summary>
            /// Reference to the <see cref="UI.Windows.CourseWindow"/> where the <see cref="IEditorImguiTest"/> was executed.
            /// </summary>
            public EditorWindow Result { get; }

            public FinishedEventArgs(EditorWindow result)
            {
                Result = result;
            }
        }

        public delegate void FinishedHandler(object sender, FinishedEventArgs e);

        /// <summary>
        /// Event fired when <see cref="EditorWindowTestPlayer"/> finished playing a <see cref="IEditorImguiTest"/>.
        /// </summary>
        public event FinishedHandler Finished;

        /// <summary>
        /// True if this <see cref="EditorWindowTestRecorder"/> is currently playing a <see cref="IEditorImguiTest"/>.
        /// </summary>
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

            Finished?.Invoke(this, new FinishedEventArgs(window));

            window.Close();
            Close();
        }
    }
}
