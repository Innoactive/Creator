using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// Default implementation of <see cref="IPlatformFileSystem"/> based in 'System.IO'.
    /// </summary>
    /// <remarks>It works out of the box for most of the Unity's supported platforms.</remarks>
    public class DefaultFileSystem : IPlatformFileSystem
    {
        /// <summary>
        /// The path to the platform's StreamingAssets folder (Read Only).
        /// </summary>
        protected readonly string StreamingAssetsPath;

        /// <summary>
        /// The path to the platform's persistent data directory (Read Only).
        /// </summary>
        protected readonly string PersistentDataPath;

        public DefaultFileSystem(string streamingAssetsPath, string persistentDataPath)
        {
            StreamingAssetsPath = streamingAssetsPath;
            PersistentDataPath = persistentDataPath;
        }

        /// <inheritdoc />
        public virtual Task<byte[]> Read(string filePath)
        {

            if (FileExistsInStreamingAssets(filePath))
            {
                return ReadFromStreamingAssets(filePath);
            }

            if (FileExistsInPersistentData(filePath))
            {
                return ReadFromPersistentData(filePath);
            }

            throw new FileNotFoundException(filePath);
        }

        /// <inheritdoc />
        public virtual bool Write(string filePath, byte[] fileData)
        {
            try
            {
                string absoluteFilePath = BuildPersistentDataPath(filePath);
                File.WriteAllBytes(absoluteFilePath, fileData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        /// <inheritdoc />
        public virtual bool Exists(string filePath)
        {
            return FileExistsInStreamingAssets(filePath) || FileExistsInPersistentData(filePath);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        /// <returns>An asynchronous operation that returns a byte array containing the contents of the file.</returns>
        protected virtual async Task<byte[]> ReadFromStreamingAssets(string filePath)
        {
            string absolutePath = Path.Combine(StreamingAssetsPath, filePath);

            if (File.Exists(absolutePath) == false)
            {
                throw new FileNotFoundException($"File at path '{filePath}' could not be found.");
            }

            return File.ReadAllBytes(absolutePath);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// /// <returns>An asynchronous operation that returns a byte array containing the contents of the file.</returns>
        protected virtual async Task<byte[]> ReadFromPersistentData(string filePath)
        {
            string absolutePath = Path.Combine(PersistentDataPath, filePath);

            if (FileExistsInPersistentData(filePath))
            {
                throw new FileNotFoundException($"File at path '{absolutePath}' could not be found.");
            }

            return File.ReadAllBytes(absolutePath);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        protected virtual bool FileExistsInStreamingAssets(string filePath)
        {
            string absolutePath = Path.Combine(StreamingAssetsPath, filePath);
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the StreamingAssets folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        protected virtual bool FileExistsInPersistentData(string filePath)
        {
            string absolutePath = Path.Combine(PersistentDataPath, filePath);
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Builds a directory from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to <see cref="PersistentDataPath"/>.</remarks>
        /// <returns>The created directory absolute path.</returns>
        protected virtual string BuildPersistentDataPath(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string relativePath = Path.GetDirectoryName(filePath);

            string absolutePath = Path.Combine(PersistentDataPath, relativePath);

            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
                Debug.LogWarningFormat("Directory '{0}' was created.\n{1}", absolutePath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return absolutePath;
            }

            return Path.Combine(PersistentDataPath, filePath);
        }
    }
}
