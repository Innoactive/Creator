﻿using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.PackageManager
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    internal static class PrefabsValidator
    {
        static PrefabsValidator()
        {
            DependencyManager.OnPostProcess += OnPostProcess;
        }

        private static void OnPostProcess(object sender, DependenciesEnabledEventArgs e)
        {
            DependencyManager.OnPostProcess -= OnPostProcess;
            ReimportCreatorPrefabs();
        }

        private static void ReimportCreatorPrefabs()
        {
            string filter = "t:Prefab";
            string[] prefabsGUIDs = AssetDatabase.FindAssets(filter, new[] {"Assets/Innoactive"});

            foreach (var prefabGUID in prefabsGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                AssetDatabase.ImportAsset(assetPath);
                Debug.LogFormat("The prefab '{0}' has been automatically reimported", assetPath);
            }
        }
    }
}
