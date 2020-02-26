using UnityEngine;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using Innoactive.Hub.Threading;
using UnityEngine.Networking;

namespace Innoactive.Creator.IO
{
    /// <summary>
    /// Android implementation of <see cref="IPlatformFileSystem"/>.
    /// </summary>
    /// <remarks>Due to the Android architecture, <see cref="Read"/> is the only possible operation over the Streaming Assets folder.
    /// The rest of the <see cref="IPlatformFileSystem"/> implementations still apply for persistent data.</remarks>
    public class AndroidFileSystem : DefaultFileSystem, IPlatformFileSystem
    {
        public AndroidFileSystem(string streamingAssetsPath, string persistentDataPath) : base(streamingAssetsPath, persistentDataPath) {}

        /// <inheritdoc />
        public override async Task<byte[]> Read(string filePath)
        {
            filePath = NormalizePath(filePath);
            byte[] fileData = await ReadFromStreamingAssets(filePath);

            if (fileData != null)
            {
                return fileData;
            }

            if (FileExistsInPersistentData(filePath))
            {
                return await ReadFromPersistentData(filePath);
            }

            throw new FileNotFoundException(filePath);
        }

        /// <summary>
        /// Returns true if given <paramref name="filePath"/> contains the name of an existing file under the persistent data folder; otherwise, false.
        /// </summary>
        /// <remarks>Due to the Android architecture, it it not possible to validate if files exits in Streaming Assets.</remarks>
        public override bool Exists(string filePath)
        {
            return FileExistsInPersistentData(filePath);
        }

        /// <inheritdoc />
        protected override async Task<byte[]> ReadFromStreamingAssets(string filePath)
        {
            string absolutePath = Path.Combine(StreamingAssetsPath, filePath);

            TaskCompletionSource<byte[]> completionSource = new TaskCompletionSource<byte[]>();
            CoroutineDispatcher.Instance.StartCoroutine(LoadFile(absolutePath, completionSource));

            return await completionSource.Task;
        }

        private IEnumerator LoadFile(string absolutePath, TaskCompletionSource<byte[]> completionSource)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(absolutePath))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError == false && webRequest.isHttpError == false)
                {
                    DownloadHandler downloadHandler = webRequest.downloadHandler;
                    yield return new WaitUntil(() => downloadHandler.isDone);

                    completionSource.SetResult(downloadHandler.data);
                }
                else
                {
                    completionSource.SetResult(null);
                }
            }
        }
    }
}
