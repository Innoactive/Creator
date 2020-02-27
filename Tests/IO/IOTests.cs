#if UNITY_EDITOR

using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NUnit.Framework;
using Innoactive.Creator.IO;

namespace Innoactive.Creator.Tests
{
    public class IOTests
    {
        protected const string FolderName = "TestFolder";
        protected const string FileName = "TestFile.txt";
        protected const string FileContent = "Hello Test!";

        protected string RelativeFilePath;
        protected string AbsoluteStreamingAssetsFilePath;
        protected string AbsolutePersistentDataFilePath;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            RelativeFilePath = Path.Combine(FolderName, FileName);
            AbsoluteStreamingAssetsFilePath = CreateDirectory(Application.streamingAssetsPath);
            AbsolutePersistentDataFilePath = CreateDirectory(Application.persistentDataPath);
        }

        [SetUp]
        public virtual void SetUp()
        {
            if (TestContext.CurrentContext.Test.MethodName.Contains("Write"))
            {
                return;
            }

            CreateFile(AbsoluteStreamingAssetsFilePath);
            CreateFile(AbsolutePersistentDataFilePath);
        }

        [TearDown]
        public virtual void TearDown()
        {
            DeleteFile(AbsoluteStreamingAssetsFilePath);
            DeleteFile(AbsolutePersistentDataFilePath);
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            DeleteDirectory(Application.streamingAssetsPath);
            DeleteDirectory(Application.persistentDataPath);
        }

        protected virtual void ReadAsync(TaskCompletionSource<byte[]> completionSource)
        {
            Assert.IsTrue(FileManager.Exists(RelativeFilePath));
            byte[] fileData = FileManager.Read(RelativeFilePath);
            completionSource.SetResult(fileData);
        }

        private void CreateFile(string cacheLocation)
        {
            using (FileStream fileStream = File.Create(cacheLocation))
            {
                byte[] fileData = new UTF8Encoding(true).GetBytes(FileContent);
                fileStream.Write(fileData, 0, fileData.Length);
            }
        }

        private void DeleteFile(string absoluteFilePath)
        {
            if (File.Exists(absoluteFilePath))
            {
                File.Delete(absoluteFilePath);
            }
        }

        private string CreateDirectory(string cacheLocation)
        {
            string fileDirectory = Path.Combine(cacheLocation, FolderName);

            if (Directory.Exists(fileDirectory) == false)
            {
                Directory.CreateDirectory(fileDirectory);
            }

            return Path.Combine(fileDirectory, FileName);
        }

        private void DeleteDirectory(string cacheLocation)
        {
            string fileDirectory = Path.Combine(cacheLocation, FolderName);

            if (Directory.Exists(fileDirectory))
            {
                Directory.Delete(fileDirectory);
            }
        }
    }
}

#endif

