using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// This base class is supposed to be implemented by classes which will be called to setup the scene.
    /// Can be used to e.g. setup training classes or interaction frameworks.
    /// </summary>
    /// <remarks>
    /// See <see cref="TrainingConfigurationSetup"/> as a reference.
    /// </remarks>
    public abstract class SceneSetup
    {
        /// <summary>
        /// Identifier key for specific scene setup types,
        /// e.g. for every interaction framework.
        /// </summary>
        public virtual string Key { get; } = null;

        /// <summary>
        /// Priority lets you tweak in which order different <see cref="SceneSetup"/>s will be performed.
        /// The priority is considered from lowest to highest.
        /// </summary>
        public virtual int Priority { get; } = 0;

        /// <summary>
        /// Setup the scene with necessary objects and/or logic.
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Sets up given <paramref name="prefab"/> in current scene.
        /// </summary>
        /// <remarks>Extensions must be omitted. All asset names and paths in Unity use forward slashes, paths using backslashes will not work.</remarks>
        /// <param name="prefab">Name or path to the target resource to setup.</param>
        /// <exception cref="FileNotFoundException">Exception thrown if no prefab can be found in project with given <paramref name="prefab"/>.</exception>
        protected void SetupPrefab(string prefab)
        {
            string prefabName = Path.GetFileName(prefab);

            if (IsPrefabMissingInScene(prefabName))
            {
                string filter = $"{prefab} t:Prefab";
                string[] prefabsGUIDs = AssetDatabase.FindAssets(filter, null);

                if (prefabsGUIDs.Any() == false)
                {
                    throw new FileNotFoundException($"No prefabs found that match \"{prefab}\"." );
                }

                string assetPath = AssetDatabase.GUIDToAssetPath(prefabsGUIDs.First());
                AssetDatabase.ImportAsset(assetPath);

                string[] brokenPaths = Regex.Split(assetPath, "Resources/");
                string relativePath = brokenPaths.Last().Replace(".prefab", string.Empty);

                GameObject instance = Object.Instantiate(Resources.Load(relativePath, typeof(GameObject))) as GameObject;
                instance.name = instance.name.Replace("(Clone)", string.Empty);
            }
        }

        /// <summary>
        /// Returns true if given <paramref name="prefabName"/> is missing in current scene.
        /// </summary>
        protected bool IsPrefabMissingInScene(string prefabName)
        {
            return GameObject.Find(prefabName) == null;
        }
    }
}
