namespace Innoactive.Creator.Core.IO
{
    /// <summary>
    /// Interface with basic platform operations for reading and saving files in Unity.
    /// </summary>
    /// <remarks>Operations are done for the StreamingAssets and platform persistent data folders.</remarks>
    public interface IPlatformFileSystem
    {
        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the persistent data folder.</remarks>
        /// <returns>The contents of the file into a byte array.</returns>
        byte[] Read(string filePath);

        /// <summary>
        /// Saves given <paramref name="fileData"/> in provided <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>Returns true if <paramref name="fileData"/> could be saved successfully; otherwise, false.</returns>
        bool Write(string filePath, byte[] fileData);

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets or platform persistent data folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the platform persistent data folder.</remarks>
        bool Exists(string filePath);
    }
}
