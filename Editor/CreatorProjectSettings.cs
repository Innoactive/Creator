﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Innoactive.CreatorEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
            AssetDatabase.CreateAsset (settings, "Assets/Resources/CreatorProjectSettings.asset");
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
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
