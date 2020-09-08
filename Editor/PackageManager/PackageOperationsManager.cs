using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Innoactive.CreatorEditor.PackageManager
{
    /// <summary>
    /// Handles different Unity's Package Manager requests.
    /// </summary>
    [InitializeOnLoad]
    internal class PackageOperationsManager
    {
        public class PackageEnabledEventArgs : EventArgs
        {
            public readonly PackageInfo PackageInfo;

            public PackageEnabledEventArgs(PackageInfo packageInfo)
            {
                PackageInfo = packageInfo;
            }
        }

        public class PackageDisabledEventArgs : EventArgs
        {
            public readonly string Package;

            public PackageDisabledEventArgs(string package)
            {
                Package = package;
            }
        }

        /// <summary>
        /// Emitted when a package was successfully installed.
        /// </summary>
        public static event EventHandler<PackageEnabledEventArgs> OnPackageEnabled;

        /// <summary>
        /// Emitted when a package was successfully removed.
        /// </summary>
        public static event EventHandler<PackageDisabledEventArgs> OnPackageDisabled;

        /// <summary>
        /// List of currently loaded packages in the Package Manager.
        /// </summary>
        public static PackageCollection Packages { get; private set; }

        static PackageOperationsManager()
        {
            FetchPackageList();
        }

        private static async void FetchPackageList()
        {
            ListRequest listRequest2 = Client.List();

            while (listRequest2.IsCompleted == false)
            {
                await Task.Delay(100);
            }

            if (listRequest2.Status == StatusCode.Failure)
            {
                Debug.LogErrorFormat("There was an error trying to retrieve a package list from the Package Manager - Error Code: [{0}] .\n{1}", listRequest2.Error.errorCode, listRequest2.Error.message);
            }
            else
            {
                Packages = listRequest2.Result;
            }
        }

        /// <summary>
        /// Adds a package to the Package Manager.
        /// </summary>
        /// <param name="package">A string representing the package to be added.</param>
        public static async void LoadPackage(string package)
        {
            if (string.IsNullOrEmpty(package) || IsPackageLoaded(package) || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            AddRequest addRequest = Client.Add(package);
            Debug.LogFormat("Enabling {0}.", package);

            while (addRequest.IsCompleted == false)
            {
                await Task.Delay(100);
            }

            if (addRequest.Status == StatusCode.Failure)
            {
                Debug.LogErrorFormat("There was an error trying to enable '{0}' - Error Code: [{1}] .\n{2}", package, addRequest.Error.errorCode, addRequest.Error.message);
            }
            else
            {
                OnPackageEnabled?.Invoke(null, new PackageEnabledEventArgs(addRequest.Result));
                Debug.LogFormat("The package '{0} version {1}' has been automatically added", addRequest.Result.displayName, addRequest.Result.version);
            }
        }

        /// <summary>
        /// Removes a package from the Package Manager.
        /// </summary>
        /// <param name="package">A string representing the package to be removed.</param>
        public static async void RemovePackage(string package)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode == false)
            {
                if (IsPackageLoaded(package))
                {
                    RemoveRequest removeRequest = Client.Remove(package);
                    Debug.LogFormat("Removing {0}.", package);

                    while (removeRequest.IsCompleted == false)
                    {
                        await Task.Delay(100);
                    }

                    if (removeRequest.Status >= StatusCode.Failure)
                    {
                        Debug.LogErrorFormat("There was an error trying to enable '{0}' - Error Code: [{1}] .\n{2}",
                            package, removeRequest.Error.errorCode, removeRequest.Error.message);
                    }
                    else
                    {
                        OnPackageDisabled?.Invoke(null, new PackageDisabledEventArgs(removeRequest.PackageIdOrName));
                        Debug.LogFormat("The package '{0} has been removed", removeRequest.PackageIdOrName);
                    }
                }
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
                return Packages != null && Packages.Any(packageInfo => packageInfo.name == packageData.First() && packageInfo.version == packageData.Last());
            }

            return Packages != null && Packages.Any(packageInfo => packageInfo.name == package);
        }
    }
}
