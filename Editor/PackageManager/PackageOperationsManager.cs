using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using Debug = UnityEngine.Debug;

namespace Innoactive.CreatorEditor.PackageManager
{
    /// <summary>
    /// Handles differnt Unity's Package Manager requests.
    /// </summary>
    [InitializeOnLoad]
    internal static class PackageOperationsManager
    {
        private enum PackageRequestStatus
        {
            None,
            Init,
            Listing,
            Adding,
            WaitingForPackage,
            Failure
        }

        private static string currentPackage;
        private static ListRequest listRequest;
        private static AddRequest addRequest;
        private static PackageCollection packageCollection;

        /// <summary>
        /// List of currently loaded packages in the Unity's Package Manager.
        /// </summary>
        public static PackageCollection Packages { get { return packageCollection; } }

        private static PackageRequestStatus packageRequestStatus;

        static PackageOperationsManager()
        {
            packageRequestStatus = PackageRequestStatus.Init;
            EditorApplication.update += EditorUpdate;
        }

        /// <summary>
        /// Adds a package dependency to the Project.
        /// </summary>
        /// <param name="package">A string representing the package to be added.</param>
        public static void LoadPackage(string package)
        {
            if (string.IsNullOrEmpty(currentPackage))
            {
                currentPackage = package;
                packageRequestStatus = PackageRequestStatus.Adding;

                Debug.LogFormat("Enabling {0}.", package);
                EditorApplication.update += EditorUpdate;
            }
            else
            {
                Debug.LogErrorFormat("Package {0} cant be loaded while {1} is being loaded.", package, currentPackage);
            }
        }

        /// <summary>
        /// Returns true if the <see cref="PackageOperationsManager"/> has already collected a list of currently available packages and
        /// given <paramref name="package"/> is already on that list.
        /// </summary>
        public static bool IsPackageLoaded(string package)
        {
            if (package.Contains('@'))
            {
                string[] packageData = package.Split('@');
                return packageCollection != null && packageCollection.Any(packageInfo => packageInfo.name == packageData.First() && packageInfo.version == packageData.Last());
            }

            return packageCollection != null && packageCollection.Any(packageInfo => packageInfo.name == package);
        }

        private static void EditorUpdate()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlaying)
            {
                return;
            }

            switch (packageRequestStatus)
            {
                case PackageRequestStatus.Init:
                    RequestPackageList();
                    break;
                case PackageRequestStatus.Listing:
                    RetrievePackageListResult();
                    break;
                case PackageRequestStatus.Adding:
                    RequestAddPackage();
                    break;
                case PackageRequestStatus.WaitingForPackage:
                    WaitForPackageStatus();
                    break;
                default:
                    StopListeningEditorUpdate();
                    break;
            }
        }

        private static void RequestPackageList()
        {
            listRequest = Client.List();
            packageRequestStatus = PackageRequestStatus.Listing;
        }

        private static void RetrievePackageListResult()
        {
            if (listRequest == null || listRequest.IsCompleted == false)
            {
                return;
            }

            if (listRequest.Status == StatusCode.Failure)
            {
                packageRequestStatus = PackageRequestStatus.Failure;
                Debug.LogErrorFormat("There was an error trying to retrieve a package list from the Package Manager - Error Code: [{0}] .\n{1}", listRequest.Error.errorCode, listRequest.Error.message);
            }
            else
            {
                packageRequestStatus = PackageRequestStatus.None;
                packageCollection = listRequest.Result;
            }

            listRequest = null;
        }

        private static void RequestAddPackage()
        {
            packageRequestStatus = PackageRequestStatus.WaitingForPackage;
            addRequest = Client.Add(currentPackage);
        }

        private static void WaitForPackageStatus()
        {
            if (addRequest == null || addRequest.IsCompleted == false)
            {
                return;
            }

            if (addRequest.Status >= StatusCode.Failure)
            {
                packageRequestStatus = PackageRequestStatus.Failure;
                Debug.LogErrorFormat("There was an error trying to enable '{0}' - Error Code: [{1}] .\n{2}", currentPackage, addRequest.Error.errorCode, addRequest.Error.message);
            }
            else
            {
                packageRequestStatus = PackageRequestStatus.None;
                Debug.LogFormat("The package '{0} version {1}' has been automatically added", addRequest.Result.displayName, addRequest.Result.version);
            }

            addRequest = null;
            currentPackage = null;
        }

        private static void StopListeningEditorUpdate()
        {
            packageRequestStatus = PackageRequestStatus.None;
            EditorApplication.update -= EditorUpdate;
        }
    }
}
