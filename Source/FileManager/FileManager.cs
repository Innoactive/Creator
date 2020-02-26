﻿using UnityEngine;
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
            platformFileSystem = CreatePlatformFileSystem();
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <param name="filePath">Path where the file is located.</param>
        /// <param name="cacheLocation">Cache directory where to lok</param>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the persistent data folder.</remarks>
        /// <returns>An asynchronous operation that returns a byte array containing the contents of the file.</returns>
        public static async Task<byte[]> Read(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid 'filePath'");
            }

            if (Path.IsPathRooted(filePath))
            {
                throw new ArgumentException($"Method only accepts relative paths.\n'filePath': {filePath}");
            }

            return await platformFileSystem.Read(filePath);
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
                throw new ArgumentException("Invalid 'filePath'");
            }

            if (Path.IsPathRooted(filePath))
            {
                throw new ArgumentException($"Method only accepts relative paths.\n'filePath': {filePath}");
            }

            if (fileData == null || fileData.Length == 0)
            {
                throw new ArgumentException("Invalid 'fileData'");
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
                throw new ArgumentException("Invalid 'filePath'");
            }

            if (Path.IsPathRooted(filePath))
            {
                throw new ArgumentException($"Method only accepts relative paths.\n'filePath': {filePath}");
            }

            return platformFileSystem.Exists(filePath);
        }

        private static IPlatformFileSystem CreatePlatformFileSystem()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return new AndroidFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
            }

            return new DefaultFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
        }
    }
}
