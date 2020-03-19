using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Serialization;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    public static class ImportTrainingMenuEntry
    {
        /// <summary>
        /// Allows to import trainings.
        /// </summary>
        [MenuItem("Innoactive/Creator/Import Training Course", false, 14)]
        private static void ImportTraining()
        {
            string path = EditorUtility.OpenFilePanel("Select your training", ".", String.Empty);

            if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            {
                return;
            }

            string format = Path.GetExtension(path).Replace(".", "");
            List<ICourseSerializer> result = GetFittingSerializer(format);

            if (result.Count == 0)
            {
                Debug.LogError("Tried to import, but no Serializer found.");
                return;
            }

            if (result.Count == 1)
            {
                StartImport(path, result.First());
            }
            else
            {
                ChooseSerializerPopup.Show(result, (serializer) => { StartImport(path, serializer); });
            }
        }

        private static void StartImport(string path, ICourseSerializer serializer)
        {
            if (EditorCourseUtils.ImportTrainingCourse(path, serializer))
            {
                EditorUtility.DisplayDialog("Training successfully imported", "Your training course was successfully imported.", "OK");
            }
        }

        private static List<ICourseSerializer> GetFittingSerializer(string format)
        {
            return ReflectionUtils.GetConcreteImplementationsOf<ICourseSerializer>()
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .Select(type => (ICourseSerializer)ReflectionUtils.CreateInstanceOfType(type))
                .Where(s => s.FileFormat.Equals(format))
                .ToList();
        }
    }
}
