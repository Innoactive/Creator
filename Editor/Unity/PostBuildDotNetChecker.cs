using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Post-process validation to identify if the 'API Compatibility Level' is set to '.Net 4.x'.
    /// </summary>
    internal class PostBuildDotNetChecker : IPostprocessBuildWithReport
    {
        ///<inheritdoc />
        public int callbackOrder => 0;

        ///<inheritdoc />
        public void OnPostprocessBuild(BuildReport report)
        {
            if (EditorUtils.GetCurrentCompatibilityLevel() != ApiCompatibilityLevel.NET_4_6)
            {
                Debug.LogError("This Unity project is not configured for .Net 4.X, some features of the Innoactive Creator requires .NET 4.X support.\nThe built application might not work as expected."
                    + "\nIn order to prevent this, go to Edit > Project Settings > Player Settings > Other Settings and set the Api Compatibility Level to .NET 4.X.");
            }
        }
    }
}
