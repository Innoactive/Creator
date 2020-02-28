#if UNITY_EDITOR

using System.IO;
using System.Text;
using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using Innoactive.Creator.IO;

namespace Innoactive.Creator.Tests
{
    public class DefaultFileSystemTests : IOTests
    {
        private IPlatformFileSystem defaultFileSystem;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            defaultFileSystem = new DefaultFileSystem(Application.streamingAssetsPath, Application.persistentDataPath);
        }

        [UnityTest]
        public IEnumerator Read()
        {
            // Given
            Assert.IsTrue(defaultFileSystem.Exists(RelativeFilePath));

            // When
            byte[] fileData = defaultFileSystem.Read(RelativeFilePath);
            string message = Encoding.Default.GetString(fileData);

            Assert.That(fileData != null && fileData.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(message));
            yield break;
        }

        [UnityTest]
        public IEnumerator Exists()
        {
            // Given
            // When
            // Then
            Assert.IsFalse(string.IsNullOrEmpty(RelativeFilePath));
            Assert.IsFalse(Path.IsPathRooted(RelativeFilePath));
            Assert.IsTrue(defaultFileSystem.Exists(RelativeFilePath));
            yield break;
        }

        [UnityTest]
        public IEnumerator Write()
        {
            Assert.IsFalse(FileManager.Exists(RelativeFilePath));

            // Given
            byte[] fileData = new UTF8Encoding(true).GetBytes(FileContent);

            // When
            defaultFileSystem.Write(RelativeFilePath, fileData);

            // Then
            Assert.IsTrue(defaultFileSystem.Exists(RelativeFilePath));
            yield break;
        }

        [UnityTest]
        public IEnumerator NotExistingFile()
        {
            // Given
            // When
            // Then
            Assert.IsFalse(defaultFileSystem.Exists(NonExistingFilePath));
            yield break;
        }
    }
}

#endif
