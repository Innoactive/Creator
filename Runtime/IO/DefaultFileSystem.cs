﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Innoactive.Creator.Core.IO
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
        public virtual byte[] Read(string filePath)
        {
            filePath = NormalizePath(filePath);

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
            filePath = NormalizePath(filePath);

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
            filePath = NormalizePath(filePath);
            return FileExistsInStreamingAssets(filePath) || FileExistsInPersistentData(filePath);
        }

        /// <inheritdoc />
        /// <remarks>
        /// The following wildcard specifiers are permitted in <paramref name="searchPattern"/>:
        /// Wildcard specifier	    Matches
        /// * (asterisk)	        Zero or more characters in that position.
        /// ? (question mark)	    Zero or one character in that position.
        /// </remarks>
        public virtual IEnumerable<string> FetchStreamingAssetsFilesAt(string path, string searchPattern)
        {
            string relativePath = Path.Combine(StreamingAssetsPath, path);
            return Directory.GetFiles(relativePath, searchPattern);
        }

        /// <summary>
        /// Loads a file stored at <paramref name="filePath"/>.
        /// Returns a `FileNotFoundException` if file does not exist.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the StreamingAssets folder.</remarks>
        /// <returns>The contents of the file into a byte array.</returns>
        protected virtual byte[] ReadFromStreamingAssets(string filePath)
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
        /// <remarks><paramref name="filePath"/> must be relative to the platform persistent data folder.</remarks>
        /// <returns>The contents of the file into a byte array.</returns>
        protected virtual byte[] ReadFromPersistentData(string filePath)
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
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the platform persistent data folder; otherwise, false.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the platform persistent data folder.</remarks>
        protected virtual bool FileExistsInPersistentData(string filePath)
        {
            string absolutePath = Path.Combine(PersistentDataPath, filePath);
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Builds a directory from given <paramref name="filePath"/>.
        /// </summary>
        /// <remarks><paramref name="filePath"/> must be relative to the platform persistent data folder.</remarks>
        /// <returns>The created directory absolute path.</returns>
        protected virtual string BuildPersistentDataPath(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string relativePath = Path.GetDirectoryName(filePath);

            string absolutePath = Path.Combine(PersistentDataPath, relativePath);

            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
                Debug.LogWarningFormat("Directory '{0}' was created.", absolutePath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return absolutePath;
            }

            return Path.Combine(PersistentDataPath, filePath);
        }

        /// <summary>
        /// Normalizes path to platform specific.
        /// </summary>
        protected virtual string NormalizePath(string filePath)
        {
            return filePath.Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
