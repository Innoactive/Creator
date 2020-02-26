#if UNITY_EDITOR

using System.Text;
using System.Collections;
using System.Threading.Tasks;
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

            // When
            TaskCompletionSource<byte[]> completionSource = new TaskCompletionSource<byte[]>();
            ReadAsync(completionSource);
            yield return new WaitUntil(()=> completionSource.Task.IsCompleted);

            // Then
            byte[] fileData = completionSource.Task.Result;
            string message = Encoding.Default.GetString(fileData);

            Assert.That(fileData != null && fileData.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(message));
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
    }
}

#endif
