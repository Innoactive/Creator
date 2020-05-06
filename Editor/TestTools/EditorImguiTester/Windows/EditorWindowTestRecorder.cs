using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Innoactive.CreatorEditor.ImguiTester;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.TestTools
{
    /// <summary>
    /// Utility window which draws itself on top of a given window and intercepts the events.
    /// </summary>
    internal class EditorWindowTestRecorder : EditorWindow
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings {Converters = new List<JsonConverter> {new ImguiEventConverter()}};
        private readonly IList<UserAction> userActions = new List<UserAction>();
        private IEditorImguiTest test;
        private EditorWindow recordedWindow;

        public static bool IsRecording { get; private set; }

        public void StartRecording(IEditorImguiTest test)
        {
            IsRecording = true;

            recordedWindow = test.BaseGiven();
            this.test = test;

            userActions.Clear();

            TestableEditorElements.StartRecording();
        }

        private void OnDestroy()
        {
            if (IsRecording)
            {
                TestableEditorElements.Panic();
                if (recordedWindow != null)
                {
                    recordedWindow.Close();
                }
            }
        }

        private void Abort()
        {
            IsRecording = false;
            TestableEditorElements.Panic();
            if (recordedWindow != null)
            {
                recordedWindow.Close();
            }

            Close();
        }

        private void SaveAndTerminate()
        {
            JsonSerializer serializer = JsonSerializer.Create(serializerSettings);

            List<string> lastPrepickedSelections = TestableEditorElements.StopRecording();

            if (userActions.Any())
            {
                userActions.Last().PrepickedSelections = lastPrepickedSelections;
            }

            string serialized = JArray.FromObject(userActions, serializer).ToString();
            Directory.CreateDirectory(Path.GetDirectoryName(test.PathToRecordedActions));

            StreamWriter file = null;
            try
            {
                file = File.CreateText(test.PathToRecordedActions);
                file.Write(serialized);
            }
            finally
            {
                IsRecording = false;

                if (file != null)
                {
                    file.Close();
                }

                if (recordedWindow != null)
                {
                    recordedWindow.Close();
                }

                AssetDatabase.ImportAsset(test.PathToRecordedActions);

                Close();
            }
        }

        private void OnGUI()
        {
            try
            {
                if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Abort")
                {
                    Abort();
                    return;
                }

                if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SaveAndTerminate")
                {
                    SaveAndTerminate();
                    return;
                }

                Rect newPos = recordedWindow.position;

                minSize = newPos.size;
                maxSize = newPos.size;
                position = newPos;

                MethodInfo onGui = recordedWindow.GetType().GetMethod("OnGUI", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (Event.current.type != EventType.Used)
                {
                    if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
                    {
                        onGui.Invoke(recordedWindow, new object[0]);
                    }
                    else if (Event.current.isMouse == false || recordedWindow.position.Contains(Event.current.mousePosition + recordedWindow.position.position))
                    {
                        if (userActions.Any())
                        {
                            userActions.Last().PrepickedSelections = TestableEditorElements.StopRecording();
                            TestableEditorElements.StartRecording();
                        }

                        Event toRecord = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(Event.current, serializerSettings), serializerSettings);
                        userActions.Add(new UserAction {Event = toRecord});

                        onGui.Invoke(recordedWindow, new object[0]);
                    }
                }

                Focus();
            }
            catch
            {
                IsRecording = false;
                TestableEditorElements.Panic();
                if (recordedWindow != null)
                {
                    recordedWindow.Close();
                }

                Close();
                throw;
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
