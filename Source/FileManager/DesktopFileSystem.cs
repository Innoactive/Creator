﻿using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// Desktop implementation of <see cref="IPlatformFileSystem"/>.
    /// </summary>
    public class DesktopFileSystem : IPlatformFileSystem
    {
        private readonly string streamingAssetsPath;
        private readonly string persistentDataPath;

        /// <inheritdoc />
        public string StreamingAssetsPath { get; }

        /// <inheritdoc />
        public string PersistentDataPath { get; }

        public DesktopFileSystem(string streamingAssetsPath, string persistentDataPath)
        {
            this.streamingAssetsPath = streamingAssetsPath;
            this.persistentDataPath = persistentDataPath;
        }

        /// <inheritdoc />
        public async Task<byte[]> GetFileFromStreamingAssets(string filePath)
        {
            string absolutePath = Path.Combine(streamingAssetsPath, filePath);

            if (File.Exists(absolutePath) == false)
            {
                string errorMessage = string.Format("File at path '{0}' could not be found.", filePath);
                throw new FileNotFoundException(errorMessage);
            }

            byte[] bytes = File.ReadAllBytes(absolutePath);

            return bytes;
        }

        /// <inheritdoc />
        public bool StreamingAssetsFileExists(string filePath)
        {
            string absolutePath = Path.Combine(streamingAssetsPath, filePath);
            return File.Exists(absolutePath);
        }

        /// <inheritdoc />
        public byte[] GetFileFromPersistentData(string filePath)
        {
            string absolutePath = Path.Combine(persistentDataPath, filePath);

            if (PersistentDataFileExists(filePath))
            {
                string errorMessage = string.Format("File at path '{0}' could not be found.", absolutePath);
                throw new FileNotFoundException(errorMessage);
            }

            byte[] bytes = File.ReadAllBytes(absolutePath);

            return bytes;
        }

        /// <inheritdoc />
        public bool SaveFileInPersistentData(string filePath, byte[] file)
        {
            bool savedSuccessfully = false;

            try
            {
                string absoluteFilePath = BuildPersistentDataPath(filePath);
                File.WriteAllBytes(absoluteFilePath, file);
                savedSuccessfully = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return savedSuccessfully;
        }

        /// <inheritdoc />
        public bool PersistentDataFileExists(string filePath)
        {
            string absolutePath = Path.Combine(persistentDataPath, filePath);
            return File.Exists(absolutePath);
        }

        /// <inheritdoc />
        public string BuildPersistentDataPath(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string relativePath = Path.GetDirectoryName(filePath);

            string absolutePath = Path.Combine(persistentDataPath, relativePath);

            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
                Debug.LogWarningFormat("Directory '{0}' was created.\n{1}", absolutePath);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return absolutePath;
            }

            string absoluteFilePath = Path.Combine(persistentDataPath, filePath);
            return absoluteFilePath;
        }
    }
}
