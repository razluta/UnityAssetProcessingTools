using System;
using System.IO;
using UnityEngine;

namespace UnityAssetProcessingTools.AssetUtilities
{
    public static class PathUtilities
    {
        public static string GetApplicationDataPath()
        {
            return Application.dataPath.ToString();
        }

        public static bool IsPathInProject(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            
            var uniformPath = Path.GetFullPath(path);
            var uniformProjectPath = Path.GetFullPath(GetApplicationDataPath());

            return uniformPath.Contains(uniformProjectPath);
        }

        public static string GetRelativeAssetPath(string absoluteAssetPath)
        {
            var relativeAssetPath = string.Empty;

            if (!File.Exists(absoluteAssetPath))
            {
                return relativeAssetPath;
            }

            var uniformAbsolutePath = Path.GetFullPath(absoluteAssetPath);
            var uniformProjectPath = Path.GetFullPath(GetApplicationDataPath());

            if (!uniformAbsolutePath.Contains(uniformProjectPath))
            {
                return relativeAssetPath;
            }
            
            var stringSplit = uniformProjectPath.Split(
                new string[] {uniformAbsolutePath}, 
                StringSplitOptions.None);
            relativeAssetPath = stringSplit[1].Substring(1);

            return relativeAssetPath;
        }
    }
}