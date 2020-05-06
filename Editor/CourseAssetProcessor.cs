namespace Innoactive.CreatorEditor
{
    internal class CourseAssetProcessor : UnityEditor.AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string[] paths)
        {
            Editors.ProjectIsGoingToSave();
            return paths;
        }
    }
}
