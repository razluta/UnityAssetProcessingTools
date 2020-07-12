using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityAssetProcessingTools
{
    public static class ProjectSearch
    {
        private const string AllAssetsFolderRelativePath = "Assets";
        private const int BytesInKiloBytes = 1024;

        public static bool IsAssetValidForFilter(string assetRelativePath, ActiveFilter filter)
        {
            var assetAbsolutePath = Path.Combine(
                System.IO.Directory.GetParent(GetApplicationDataPath()).ToString(), 
                assetRelativePath);
            
            // Check file exists
            if (!File.Exists(assetAbsolutePath))
            {
                return false;
            }
               
            var assetName = Path.GetFileName(assetRelativePath);
            var assetDiskSize = new System.IO.FileInfo(assetAbsolutePath).Length / BytesInKiloBytes;
               
            // BrowsePath
            if(!String.IsNullOrWhiteSpace(filter.BrowsePath))
            {
                if (!assetRelativePath.StartsWith(filter.BrowsePath))
                {
                    return false;
                }
            }
               
            // IsRecursive
            // ???
               
            // NameStartsWith
            if (!assetName.StartsWith(filter.NameStartsWith))
            {
                return false;
            }
               
            // NameContains
            if (!assetName.Contains(filter.NameContains))
            {
                return false;
            }
               
            // NameEndsWith
            if (!assetName.EndsWith(filter.NameEndsWith))
            {
                return false;
            }
               
            // DiskSize
            if (assetDiskSize > filter.DiskSize)
            {
                return false;
            }

            return true;
        }
        
        public static List<string> GetAllFileAssetRelativePaths()
        {
            var allAssetGuids = GetAllFileAssetGuids();
            var allAssetsRelativePaths = new List<string>();
            
            foreach (var assetGuid in allAssetGuids)
            {
                var relativeFilePath = AssetDatabase.GUIDToAssetPath(assetGuid);
                
                var unityProjectRootPath = System.IO.Directory.GetParent(GetApplicationDataPath()).ToString();
                var absoluteFilePath = Path.Combine(unityProjectRootPath, relativeFilePath);

                if (!System.IO.File.Exists(absoluteFilePath))
                {
                    continue;
                }

                allAssetsRelativePaths.Add(relativeFilePath);
            }

            return allAssetsRelativePaths;
        }
        
        public static string GetApplicationDataPath()
        {
            return Application.dataPath;
        }

        private static string[] GetAllFileAssetGuids()
        {
            return AssetDatabase.FindAssets(null, new [] {AllAssetsFolderRelativePath});
        }
    }
}