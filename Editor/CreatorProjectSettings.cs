using System.Collections;
using System.Collections.Generic;
using System.IO;
using Innoactive.CreatorEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreatorProjectSettings : ScriptableObject
{
    [SerializeField]
    public bool IsFirstTimeStarted = true;

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

    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
