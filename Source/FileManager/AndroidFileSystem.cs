﻿#if !UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// Android implementation of <see cref="IPlatformFileSystem"/>.
    /// </summary>
    public class AndroidFileSystem : DefaultFileSystem, IPlatformFileSystem
    {
        private readonly string rootFolder;
        private readonly string[] streamingAssetsFilesPath;

        private const string StreamingAssetsArchivePath = "assets/";
        private const string ExcludedArchivePath = "assets/bin/";

        public AndroidFileSystem(string streamingAssetsPath, string persistentDataPath) : base(streamingAssetsPath, persistentDataPath)
        {
            rootFolder = Application.dataPath;

            using (ZipArchive archive = ZipFile.OpenRead(rootFolder))
            {
                List<string> filesPath = new List<string>();

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.StartsWith(StreamingAssetsArchivePath) && entry.FullName.StartsWith(ExcludedArchivePath) == false)
                    {
                        filesPath.Add(entry.FullName);
                    }
                }

                streamingAssetsFilesPath = filesPath.ToArray();
            }
        }

        /// <inheritdoc />
        /// <remarks>This method uses additional functionality of 'System.IO.Compression' that are not bundled with Unity.
        /// The MSBuild response file 'Assets/csc.rsp' is required for this.
        /// See more: https://docs.unity3d.com/Manual/dotnetProfileAssemblies.html</remarks>
        protected override byte[] ReadFromStreamingAssets(string filePath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(rootFolder))
            {
                ZipArchiveEntry file = archive.Entries.First(entry => entry.FullName == filePath);

                if (file == null)
                {
                    throw new FileNotFoundException(filePath);
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Stream fileStream = file.Open();
                    fileStream.CopyTo(memoryStream);

                    return memoryStream.ToArray();
                }
            }
        }

        /// <inheritdoc />
        protected override bool FileExistsInStreamingAssets(string filePath)
        {
            return streamingAssetsFilesPath.Any(path => path == filePath);
        }

        /// <inheritdoc />
        protected override string NormalizePath(string filePath)
        {
            filePath = Path.Combine(StreamingAssetsArchivePath, filePath);
            return base.NormalizePath(filePath);
        }
    }
}
#endif
