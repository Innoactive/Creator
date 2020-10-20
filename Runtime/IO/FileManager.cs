﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Innoactive.Creator.Core.IO
{
    /// <summary>
    /// Handles runtime operations that allow reading and writing to files in Unity.
    /// </summary>
    public static class FileManager
    {
        private static IPlatformFileSystem platformFileSystem;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            platformFileSystem = CreatePlatformFileSystem();
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the persistent data folder.</remarks>
        /// <returns>The contents of the file into a byte array.</returns>
        /// <exception cref="ArgumentException">Exception thrown if <paramref name="filePath"/> is invalid.</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if the file does not exist.</exception>
        public static byte[] Read(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid 'filePath'");
            }

            if (Path.IsPathRooted(filePath))
            {
                throw new ArgumentException($"Method only accepts relative paths.\n'filePath': {filePath}");
            }

            return platformFileSystem.Read(filePath);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets or the persistent data folder.</remarks>
        /// <returns>Returns a `string` with the content of the file.</returns>
        /// <exception cref="ArgumentException">Exception thrown if <paramref name="filePath"/> is invalid.</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if the file does not exist.</exception>
        public static string ReadAllText(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid 'filePath'");
            }

            if (Path.IsPathRooted(filePath))
            {
                throw new ArgumentException($"Method only accepts relative paths.\n'filePath': {filePath}");
            }

            return platformFileSystem.ReadAllText(filePath);
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

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory relative to the Streaming Assets folder.
        /// </summary>
        /// <param name="path">The relative path to the Streaming Assets folder. This string is not case-sensitive.</param>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in <paramref name="path" />.
        /// Depending on the platform, this parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see implementations of <see cref="IPlatformFileSystem"/>), but doesn't support regular expressions.
        /// </param>
        public static IEnumerable<string> FetchStreamingAssetsFilesAt(string path, string searchPattern)
        {
            return platformFileSystem.FetchStreamingAssetsFilesAt(path, searchPattern);
        }

        private static IPlatformFileSystem CreatePlatformFileSystem()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return new AndroidFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
#else
            return new DefaultFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
#endif
        }
    }
}
