using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    internal class ChangeNamePopup : EditorWindow
    {
        private static ChangeNamePopup instance;

        private readonly GUID textFieldIdentifier = new GUID();

        private INamedData nameable;
        private string newName;

        private bool isFocusSet;

        public bool IsClosed { get; protected set; }

        public static ChangeNamePopup Open(INamedData nameable, Rect labelPosition, Vector2 offset)
        {
            if (instance != null)
            {
                instance.Close();
            }

            instance = CreateInstance<ChangeNamePopup>();

            instance.nameable = nameable;
            instance.newName = nameable.Name;

            instance.position = new Rect(labelPosition.x - offset.x, labelPosition.y - offset.y, labelPosition.width, labelPosition.height);
            instance.ShowPopup();
            instance.Focus();

            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                instance.Close();
                instance.IsClosed = true;
            };

            return instance;
        }

        private void OnGUI()
        {
            if (nameable == null || focusedWindow != this)
            {
                Close();
                instance.IsClosed = true;
            }

            GUI.SetNextControlName(textFieldIdentifier.ToString());
            newName = EditorGUILayout.TextField(newName);

            if (isFocusSet == false)
            {
                isFocusSet = true;
                EditorGUI.FocusTextInControl(textFieldIdentifier.ToString());
            }

            if (focusedWindow != this)
            {
                return;
            }

            if (Event.current.isKey == false)
            {
                return;
            }

            if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
            {
                if (string.IsNullOrEmpty(newName))
                {
                    return;
                }

                string oldName = nameable.Name;
                RevertableChangesHandler.Do(new CourseCommand(
                    // ReSharper disable once ImplicitlyCapturedClosure
                    () =>
                    {
                        nameable.Name = newName;
                    },
                    // ReSharper disable once ImplicitlyCapturedClosure
                    () =>
                    {
                        nameable.Name = oldName;
                    }
                ));
                Close();
                instance.IsClosed = true;
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                instance.IsClosed = true;
                Event.current.Use();
            }
        }
    }
}
