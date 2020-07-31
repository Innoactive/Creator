using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;

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
            if (EditorUtils.GetCurrentCompatibilityLevel() == ApiCompatibilityLevel.NET_4_6)
            {
                return;
            }

            // If the application was built in .Net other than 4.x and the NewtonsoftJsonCourseSerializer class is also packed in the project,
            // then we let the user know that the application might not work and we offer a quick way to fix the problem using the DotNetWizard.
            foreach (PackedAssets packedAsset in report.packedAssets)
            {
                foreach (PackedAssetInfo content in packedAsset.contents)
                {
                    string file = Path.GetFileNameWithoutExtension(content.sourceAssetPath);

                    if (file == nameof(NewtonsoftJsonCourseSerializer))
                    {
                        Debug.LogError("This Unity project is not configured for .Net 4.X, but the Innoactive Creator requires .NET 4.X support. The built application might not work as expected.");
                        DotNetWizard.OpenWizard();
                        return;
                    }
                }
            }
        }
    }
}
