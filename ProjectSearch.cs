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
        
        public static string[] FindAssetsWithFilter(ActiveFilter filter)
        {
            var assetCount = GetAllFileAssetRelativePaths().Count;
            var assetGuidArray = new string[assetCount];
            return assetGuidArray;
        }

        public static List<string> GetAllFileAssetRelativePaths()
        {
            var allAssetGuids = GetAllFileAssetGuids();
            var allAssetsRelativePaths = new List<string>();
            
            foreach (var assetGuid in allAssetGuids)
            {
                var relativeFilePath = AssetDatabase.GUIDToAssetPath(assetGuid);
                
                var unityProjectRootPath = System.IO.Directory.GetParent(Application.dataPath).ToString();
                var absoluteFilePath = Path.Combine(unityProjectRootPath, relativeFilePath);

                if (!System.IO.File.Exists(absoluteFilePath))
                {
                    continue;
                }

                allAssetsRelativePaths.Add(relativeFilePath);
            }

            return allAssetsRelativePaths;
        }

        private static string[] GetAllFileAssetGuids()
        {
            return AssetDatabase.FindAssets(null, new [] {AllAssetsFolderRelativePath});
        }
    }
}