using System.Threading.Tasks;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPlatformFileSystem
    {
        /// <summary>
        /// The path to the StreamingAssets folder (Read Only).
        /// </summary>
        string StreamingAssetsPath { get; }

        /// <summary>
        /// The path to a persistent data directory (Read Only).
        /// </summary>
        string PersistentDataPath { get; }

        /// <summary>
        /// If exists, this method returns a file from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        Task<byte[]> GetFileFromStreamingAssets(string filePath);

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        bool StreamingAssetsFileExists(string filePath);

        /// <summary>
        /// If exits, this method returns a file from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        byte[] GetFileFromPersistentData(string filePath);

        /// <summary>
        /// Saves given <paramref name="file"/> in provided <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>Returns true if <paramref name="file"/> could be saved successfully; otherwise, false.</returns>
        bool SaveFileInPersistentData(string filePath, byte[] file);

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        bool PersistentDataFileExists(string filePath);

        /// <summary>
        /// Builds a directory from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>The created directory absolute path.</returns>
        string BuildPersistentDataPath(string filePath);
    }
}
