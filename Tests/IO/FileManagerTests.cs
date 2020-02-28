#if UNITY_EDITOR

using System;
using System.Text;
using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using Innoactive.Creator.IO;

namespace Innoactive.Creator.Tests
{
    public class FileManagerTests : IOTests
    {
        [UnityTest]
        public IEnumerator Read()
        {
            // Given
            Assert.IsTrue(FileManager.Exists(RelativeFilePath));

            // When
            byte[] fileData = FileManager.Read(RelativeFilePath);
            string message = Encoding.Default.GetString(fileData);

            // Then
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
            Assert.IsTrue(FileManager.Exists(RelativeFilePath));
            yield break;
        }

        [UnityTest]
        public IEnumerator Write()
        {
            Assert.IsFalse(FileManager.Exists(RelativeFilePath));

            // Given
            byte[] fileData = new UTF8Encoding(true).GetBytes(FileContent);

            // When
            FileManager.Write(RelativeFilePath, fileData);

            // Then
            Assert.IsTrue(FileManager.Exists(RelativeFilePath));
            yield break;
        }

        [UnityTest]
        public IEnumerator ArgumentException()
        {
            // Given
            string nullPath = null;
            string empPath = string.Empty;
            string absolutePath = Application.dataPath;
            byte[] nullData = null;
            byte[] fileData = FileManager.Read(RelativeFilePath);

            // When

            // Then
            Assert.Throws<ArgumentException>(()=> FileManager.Read(nullPath));
            Assert.Throws<ArgumentException>(()=> FileManager.Read(empPath));
            Assert.Throws<ArgumentException>(()=> FileManager.Read(absolutePath));

            Assert.Throws<ArgumentException>(()=> FileManager.Write(nullPath, fileData));
            Assert.Throws<ArgumentException>(()=> FileManager.Write(empPath, fileData));
            Assert.Throws<ArgumentException>(()=> FileManager.Write(absolutePath, fileData));
            Assert.Throws<ArgumentException>(()=> FileManager.Write(RelativeFilePath, nullData));

            Assert.Throws<ArgumentException>(()=> FileManager.Exists(nullPath));
            Assert.Throws<ArgumentException>(()=> FileManager.Exists(empPath));
            Assert.Throws<ArgumentException>(()=> FileManager.Exists(absolutePath));
            yield break;
        }

        [UnityTest]
        public IEnumerator NotExistingFile()
        {
            // Given
            // When
            // Then
            Assert.IsFalse(FileManager.Exists(NonExistingFilePath));
            yield break;
        }
    }
}

#endif
