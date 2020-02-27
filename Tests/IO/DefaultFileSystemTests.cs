#if UNITY_EDITOR

using System.IO;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
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

        protected override async void ReadAsync(TaskCompletionSource<byte[]> completionSource)
        {
            Assert.IsTrue(defaultFileSystem.Exists(RelativeFilePath));
            byte[] fileData = defaultFileSystem.Read(RelativeFilePath);
            completionSource.SetResult(fileData);
        }
    }
}

#endif
