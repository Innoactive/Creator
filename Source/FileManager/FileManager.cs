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
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        /// <returns>An asynchronous operation that returns a byte array containing the contents of the file.</returns>
        public static async Task<byte[]> RetrieveFileFromStreamingAssets(string filePath)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            return await platformFileSystem.GetFileFromStreamingAssets(filePath);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        public static bool StreamingAssetsFileExists(string filePath)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            return platformFileSystem.StreamingAssetsFileExists(filePath);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        public static byte[] RetrieveFileFromPersistentData(string filePath)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            return platformFileSystem.GetFileFromPersistentData(filePath);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        public static bool PersistentDataFileExists(string filePath)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            return platformFileSystem.PersistentDataFileExists(filePath);
        }

        /// <summary>
        /// Saves given <paramref name="file"/> in provided <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>Returns true if <paramref name="file"/> could be saved successfully; otherwise, false.</returns>
        public static bool SaveFileInPersistentData(string filePath, byte[] file)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            return platformFileSystem.SaveFileInPersistentData(filePath, file);
        }

        /// <summary>
        /// Builds a directory from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>The created directory absolute path.</returns>
        public static string BuildPersistentDataPath(string filePath)
        {
            SystemException exception = ValidatePath(filePath);

            if (exception != null)
            {
                throw exception;
            }

            return platformFileSystem.BuildPersistentDataPath(filePath);
        }

        private static SystemException ValidatePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new ArgumentException("Invalid filePath");
            }

            if (Path.IsPathRooted(filePath))
            {
                string errorMessage = string.Format("Method only accepts relative paths.\nfilePath: {0}", filePath);
                return new ArgumentException(errorMessage);
            }

            return null;
        }
    }
}
