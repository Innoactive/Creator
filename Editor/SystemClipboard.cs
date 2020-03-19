using System.Text;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.Configuration;
using UnityEditor;

namespace Innoactive.CreatorEditor.Utils
{
    /// <summary>
    /// A utility class that converts a step from/to UTF-8 string using the serializer from the current <see cref="IEditorConfiguration"/>
    /// and then copies or pastes it from the system's copy buffer.
    /// </summary>
    public static class SystemClipboard
    {
        /// <summary>
        /// Tries to deserialize step from the system's copy buffer. Does not handle exceptions.
        /// </summary>
        /// <returns>A new instance of step.</returns>
        public static IStep PasteStep()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(EditorGUIUtility.systemCopyBuffer);
            return EditorConfigurator.Instance.Serializer.StepFromByteArray(bytes);
        }

        /// <summary>
        /// Serializes the <paramref name="step"/> to a byte array, converts this array into UTF-8 string, and saves it to the system's copy buffer.
        /// </summary>
        /// <param name="step">A step to serialize.</param>
        public static void CopyStep(IStep step)
        {
            byte[] serialized = EditorConfigurator.Instance.Serializer.StepToByteArray(step);
            EditorGUIUtility.systemCopyBuffer = Encoding.UTF8.GetString(serialized);
        }

        /// <summary>
        /// Checks if there is a valid serialized step in the system's copy buffer.
        /// </summary>
        public static bool IsStepInClipboard()
        {
            try
            {
                return (PasteStep() != null);
            }
            catch
            {
                return false;
            }
        }
    }
}
