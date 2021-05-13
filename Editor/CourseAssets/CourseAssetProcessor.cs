namespace VPG.CreatorEditor
{
    /// <summary>
    /// A class which detects if the project is going to be saved and informs the <seealso cref="GlobalEditorHandler"/> about it.
    /// </summary>
    internal class CourseAssetProcessor : UnityEditor.AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string[] paths)
        {
            GlobalEditorHandler.ProjectIsGoingToSave();
            return paths;
        }
    }
}
