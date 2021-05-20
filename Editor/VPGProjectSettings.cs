using System.IO;
using VPG.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Settings for a VR Process Gizmo Unity project.
/// </summary>
public partial class VPGProjectSettings : ScriptableObject
{
    /// <summary>
    /// Was the VR Process Gizmo imported and therefore started for the first time.
    /// </summary>
    [HideInInspector]
    public bool IsFirstTimeStarted = true;

    /// <summary>
    /// VPG version used last time this was checked.
    /// </summary>
    [HideInInspector]
    public string ProjectVPGVersion = null;

    /// <summary>
    /// Loads the VR Process Gizmo settings for this Unity project from Resources.
    /// </summary>
    /// <returns>Creator Settings</returns>
    public static VPGProjectSettings Load()
    {
        VPGProjectSettings settings = Resources.Load<VPGProjectSettings>("CreatorProjectSettings");
        if (settings == null)
        {
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }
            // Create an instance
            settings = CreateInstance<VPGProjectSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Resources/CreatorProjectSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return settings;
        }
        return settings;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ProjectVPGVersion))
        {
            ProjectVPGVersion = EditorUtils.GetCoreVersion();
        }
    }

    /// <summary>
    /// Saves the VR Process Gizmo settings.
    /// </summary>
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
