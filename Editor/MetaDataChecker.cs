using Innoactive.CreatorEditor;
using UnityEditor;

namespace Innoactive.Creator.Core.Editor
{
    [InitializeOnLoad]
    internal static class MetaDataChecker
    {
        static MetaDataChecker()
        {
            OnBuildMetaDataRemoval.CleanUpTempFiles();
        }
    }
}
