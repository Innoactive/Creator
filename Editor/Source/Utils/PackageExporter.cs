using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using Innoactive.Hub.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Utils
{
    public static class PackageExporter
    {
        private class PackageExporterArguments
        {
            [Option("export-config", MetaValue = "STRING", HelpText = "Path to the exporter config.", Required = true)]
            public string Config { get; set; }
        }

        internal class ExportConfig// : ConfigBase
        {
            public string AssetDirectory = "Assets";
            public string Version = "v0.0.0";
            public string[] Includes = { "*" };
            public string[] Excludes = { };
            public string OutputPath = ".\\Builds\\v0.0.0.unitypackage";
            public string VersionFilename = "version.txt";
        }

        public static void Export()
        {
            PackageExporterArguments args = ParseCommandLineArguments();
            Export(args.Config);
        }

        public static void Export(string configPath)
        {
            if (File.Exists(configPath) == false)
            {
                string msg = string.Format("config in path '{0}' is not found!", configPath);
                throw new ArgumentException(msg);
            }

            ExportConfig config = new ExportConfig();
            //config = JsonConfigFileManager.Load(config, configPath);

            if (string.IsNullOrEmpty(config.VersionFilename) == false)
            {
                UpdateVersionFile(config.AssetDirectory + "/" + config.VersionFilename, config.Version);
            }

            // Create the output directory if it doesn't exist yet.
            string outputDirectory = Path.GetDirectoryName(config.OutputPath.Replace('/', '\\'));
            if (string.IsNullOrEmpty(outputDirectory) == false && Directory.Exists(outputDirectory) == false)
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string[] exportedPaths = GetAssetPathsToExport(config);

            AssetDatabase.ExportPackage(exportedPaths, config.OutputPath.Replace('/', '\\'), ExportPackageOptions.Default);
        }

        private static string[] GetAssetPathsToExport(ExportConfig config)
        {
            string root = config.AssetDirectory;
            if (root.Last() != '/')
            {
                root = root + '/';
            }

            string[] assetPathsInRootDirectory = AssetDatabase.GetAllAssetPaths().Where(assetPath => assetPath.StartsWith(root)).ToArray();
            string[] assetPathsIncludedOnly = assetPathsInRootDirectory.Where(filePath => config.Includes.Any(includingPattern => Regex.IsMatch(filePath, WildcardToRegular(includingPattern)))).ToArray();
            string[] assetPathsWithoutExcluded = assetPathsIncludedOnly.Where(filePath => config.Excludes.Any(excludingPattern => Regex.IsMatch(filePath, WildcardToRegular(excludingPattern))) == false).ToArray();
            return assetPathsWithoutExcluded;
        }

        private static void UpdateVersionFile(string path, string content)
        {
            File.WriteAllText(UnityAssetPathToAbsoluteWindowsPath(path), content);
            AssetDatabase.ImportAsset(path);
            TextAsset versionFile = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (versionFile != null)
            {
                EditorUtility.SetDirty(versionFile);
            }
        }

        private static string UnityAssetPathToAbsoluteWindowsPath(string unityPath)
        {
            // Unity paths always start with "Assets/"
            if (!unityPath.StartsWith("Assets/"))
            {
                throw new Exception("The specified Unity path is not relative to the Project root directory");
            }
            // prepend the path to the unity assets folder
            // replace forward by backward slashes
            return Path.Combine(Application.dataPath.Replace("/", @"\"), unityPath.Substring("Assets/".Length).Replace("/", @"\"));
        }

        private static string WildcardToRegular(string value)
        {
            return "^" + value.Replace("?", ".").Replace("*", ".*") + "$";
        }

        private static PackageExporterArguments ParseCommandLineArguments()
        {
            PackageExporterArguments arguments = new PackageExporterArguments();
            // Redirect Console.Error output to Unity
            Console.SetError(new UnityDebugLogErrorWriter());
            Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), arguments);
            // Unset console output
            Console.SetError(TextWriter.Null);

            return arguments;
        }
    }
}
