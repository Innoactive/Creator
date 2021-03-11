﻿using System;
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
                Debug.LogError($"There was an error trying to retrieve a package list from the Package Manager - Error Code: [{listRequest2.Error.errorCode}] .\n{listRequest2.Error.message}");
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
        /// <param name="version">If provided, the package will be loaded with this specific version.</param>
        public static async void LoadPackage(string package, string version = null)
        {
            if (string.IsNullOrEmpty(package) || IsPackageLoaded(package, version) || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (IsPackageLoaded(package) && string.IsNullOrEmpty(version) == false)
            {
                PackageInfo installedPackage = Packages.First(packageInfo => packageInfo.name == package);
                EditorUtility.DisplayDialog($"{installedPackage.displayName} Upgrade", $"{installedPackage.displayName} will be upgraded from v{installedPackage.version} to v{version}." , "Continue");
            }

            if (package.Contains("@") == false && string.IsNullOrEmpty(version) == false)
            {
                package = $"{package}@{version}";
            }

            AddRequest addRequest = Client.Add(package);
            Debug.Log($"Enabling package: {package.Split('@').First()}, Version: {(string.IsNullOrEmpty(version) ? "latest" : version)}.");

            while (addRequest.IsCompleted == false)
            {
                await Task.Delay(100);
            }

            if (addRequest.Status == StatusCode.Failure)
            {
                Debug.LogError($"There was an error trying to enable '{package}' - Error Code: [{addRequest.Error.errorCode}] .\n{addRequest.Error.message}");
            }
            else
            {
                OnPackageEnabled?.Invoke(null, new PackageEnabledEventArgs(addRequest.Result));
                Debug.Log($"The package '{addRequest.Result.displayName}' version '{addRequest.Result.version}' has been automatically added");
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
                    Debug.Log($"Removing {package}.");

                    while (removeRequest.IsCompleted == false)
                    {
                        await Task.Delay(100);
                    }

                    if (removeRequest.Status >= StatusCode.Failure)
                    {
                        Debug.LogError($"There was an error trying to enable '{package}' - Error Code: [{removeRequest.Error.errorCode}] .\n{removeRequest.Error.message}");
                    }
                    else
                    {
                        OnPackageDisabled?.Invoke(null, new PackageDisabledEventArgs(removeRequest.PackageIdOrName));
                        Debug.Log($"The package '{removeRequest.PackageIdOrName} has been removed");
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the <see cref="PackageOperationsManager"/> has already collected a list of currently available packages and
        /// given <paramref name="package"/> is already on that list with given <paramref name="version"/>.
        /// </summary>
        /// <remarks>If <paramref name="package"/> already contains an embedded version, <paramref name="version"/> will be ignored.</remarks>
        public static bool IsPackageLoaded(string package, string version = null)
        {
            if (string.IsNullOrEmpty(package))
            {
                throw new ArgumentException($"Parameter '{nameof(package)}' is null or empty.");
            }

            if (package.Contains('@'))
            {
                string[] packageData = package.Split('@');
                return Packages.Any(packageInfo => packageInfo.name == packageData.First() && packageInfo.version == packageData.Last());
            }

            if (string.IsNullOrEmpty(version))
            {
                return Packages.Any(packageInfo => packageInfo.name == package);
            }

            return Packages.Any(packageInfo => packageInfo.name == package && packageInfo.version == version);
        }

        /// <summary>
        /// Returns the version corresponding to the provided <paramref name="package"/> if this is installed, otherwise it returns null.
        /// </summary>
        public static string GetInstalledPackageVersion(string package)
        {
            return Packages.First(packageInfo => package.Contains(packageInfo.name))?.version;
        }
    }
}
