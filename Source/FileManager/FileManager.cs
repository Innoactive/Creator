using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// Handles runtime operations that allow reading and writing to files in Unity.
    /// </summary>
    public static class FileManager
    {
        private static IPlatformFileSystem platformFileSystem;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            platformFileSystem = new DefaultFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the persistent data folder.</remarks>
        /// <returns>An asynchronous operation that returns a byte array containing the contents of the file.</returns>
        public static async Task<byte[]> Read(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid filePath");
            }

            if (Path.IsPathRooted(filePath))
            {
                string errorMessage = string.Format("Method only accepts relative paths.\nfilePath: {0}", filePath);
                throw new ArgumentException(errorMessage);
            }

            if (Exists(filePath))
            {
                return await platformFileSystem.ReadFromStreamingAssets(filePath);
            }

            return null;
        }

        /// <summary>
        /// Saves given <paramref name="fileData"/> in provided <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>Returns true if <paramref name="fileData"/> could be saved successfully; otherwise, false.</returns>
        public static bool Write(string filePath, byte[] fileData)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid filePath");
            }

            if (Path.IsPathRooted(filePath))
            {
                string errorMessage = string.Format("Method only accepts relative paths.\nfilePath: {0}", filePath);
                throw new ArgumentException(errorMessage);
            }

            if (fileData == null || fileData.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            return platformFileSystem.Write(filePath, fileData);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets or platform persistent data folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the platform persistent data folder.</remarks>
        public static bool Exists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid filePath");
            }

            if (Path.IsPathRooted(filePath))
            {
                string errorMessage = string.Format("Method only accepts relative paths.\nfilePath: {0}", filePath);
                throw new ArgumentException(errorMessage);
            }

            return platformFileSystem.Exists(filePath);
        }
    }
}
