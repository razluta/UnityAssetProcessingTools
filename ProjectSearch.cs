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

        public static bool IsAssetValidForFilter(
            string assetRelativePath, 
            ActiveFilter filter)
        {
            Debug.Log("Asset relative path: " + assetRelativePath);
            
            var assetAbsolutePath = Path.Combine(
                System.IO.Directory.GetParent(GetApplicationDataPath()).ToString(), 
                assetRelativePath);
            
            // Make slashes consistent
            assetRelativePath = Path.GetFullPath(assetRelativePath);
            assetAbsolutePath = Path.GetFullPath(assetAbsolutePath);

            // Check file exists
            if (!File.Exists(assetAbsolutePath))
            {
                Debug.Log("- error: file does not exist at absolute path: " + assetAbsolutePath);
                return false;
            }
            else
            {
                Debug.Log("- asset absolute path: " + assetAbsolutePath);
            }
               
            var assetNameWithExtension = Path.GetFileName(assetRelativePath);
            var assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetRelativePath);
            var assetNameExtension = Path.GetExtension(assetRelativePath);
            
            var assetDiskSize = new System.IO.FileInfo(assetAbsolutePath).Length / BytesInKiloBytes;

            if (String.IsNullOrWhiteSpace(assetNameWithExtension))
            {
                Debug.Log("- error: file name is empty " + assetNameWithExtension);
                return false;
            }
               
            // BrowsePath
            if(!String.IsNullOrWhiteSpace(filter.BrowsePath))
            {
                if (!assetRelativePath.StartsWith(filter.BrowsePath))
                {
                    Debug.Log("- error: file is not in folder: " + filter.BrowsePath);
                    return false;
                }
            }
               
            // IsRecursive
            // ???
               
            // NameStartsWith
            if (!assetNameWithoutExtension.StartsWith(filter.NameStartsWith))
            {
                Debug.Log("- error: file name does not start with: " + filter.NameStartsWith);
                return false;
            }
               
            // NameContains
            if (!assetNameWithoutExtension.Contains(filter.NameContains))
            {
                Debug.Log("- error: file name does not contain: " + filter.NameContains);
                return false;
            }
               
            // NameEndsWith
            if (!assetNameWithoutExtension.EndsWith(filter.NameEndsWith))
            {
                Debug.Log("- error: file name does not end with: " + filter.NameEndsWith);
                return false;
            }
               
            // DiskSize
            if (assetDiskSize < filter.DiskSize)
            {
                Debug.Log("- error: file disk size is not larger than: " + filter.DiskSize);
                return false;
            }
            
            // Check extensions
            if (filter.ExcludedExtensions != null)
            {
                if (filter.ExcludedExtensions.Count != 0)
                {
                    foreach (var extension in filter.ExcludedExtensions)
                    {
                        if (assetNameWithExtension.EndsWith(extension))
                        {
                            return false;
                        }
                    }
                }
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
            return Application.dataPath.ToString();
        }

        private static string[] GetAllFileAssetGuids()
        {
            return AssetDatabase.FindAssets(null, new [] {AllAssetsFolderRelativePath});
        }
    }
}