using System.IO;
using Innoactive.Creator.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Innoactive.CreatorEditor.Configuration
{
    /// <summary>
    /// Checks on editor initialization if there is a logging config. Will add one if it's missing.
    /// </summary>
    [InitializeOnLoad]
    public class LoggingConfigCreationTrigger
    {
        static LoggingConfigCreationTrigger()
        {
            LifeCycleLoggingConfig instance = Resources.Load<LifeCycleLoggingConfig>("LifeCycleLoggingConfig");
            if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<LifeCycleLoggingConfig>();
                if (Directory.Exists("Assets/Resources") == false)
                {
                    Directory.CreateDirectory("Assets/Resources");
                }
                
                AssetDatabase.CreateAsset(instance, "Assets/Resources/LifeCycleLoggingConfig.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
