using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VPG.Core.Utils;
using VPG.Core.Serialization;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.VPGMenu
{
    internal static class ImportTrainingMenuEntry
    {
        /// <summary>
        /// Allows to import trainings.
        /// </summary>
        [MenuItem("VR Process Gizmo/Import Training Course", false, 14)]
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
                CourseAssetManager.Import(path, result.First());
            }
            else
            {
                ChooseSerializerPopup.Show(result, (serializer) =>
                {
                    CourseAssetManager.Import(path, serializer);
                });
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
