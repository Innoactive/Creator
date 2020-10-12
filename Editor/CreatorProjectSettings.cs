using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Settings for an Innoactive Creator Unity project.
/// </summary>
public class CreatorProjectSettings : ScriptableObject
{
    /// <summary>
    /// Was the Creator imported and therefore started for the first time.
    /// </summary>
    [SerializeField]
    public bool IsFirstTimeStarted = true;

    /// <summary>
    /// Enables the validation system.
    /// </summary>
    [SerializeField]
    public bool IsValidationEnabled = true;

    /// <summary>
    /// Loads the Creator settings for this Unity project from Resources.
    /// </summary>
    /// <returns>Creator Settings</returns>
    public static CreatorProjectSettings Load()
    {
        CreatorProjectSettings settings = Resources.Load<CreatorProjectSettings>("CreatorProjectSettings");
        if (settings == null)
        {
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }
            // Create an instance
            settings = CreateInstance<CreatorProjectSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Resources/CreatorProjectSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return Resources.Load<CreatorProjectSettings>("CreatorProjectSettings");
        }
        return settings;
    }

    /// <summary>
    /// Saves the Creator settings.
    /// </summary>
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
