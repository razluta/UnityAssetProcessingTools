using System;
using System.Collections.Generic;
using UnityAssetProcessingTools.SystemUtilities;
using UnityEngine;

namespace UnityAssetProcessingTools
{
    public class AssetProcessingTools
    {
        public enum AssetTypes
        {
            AllAssetTypes,
            Textures,
            Models
        }
        
        public const string BrowsePath = "browse_path";
        public const string IsRecursive = "is_recursive";
        public const string NameStartsWith = "name_starts_with";
        public const string NameContains = "name_contains";
        public const string NameEndsWith = "name_ends_with";
        public const string DiskSize = "disk_size";

        public static ActiveFilter GetFilter(string path=null)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return new ActiveFilter();
            }
            
            var activeFilter = JsonUtilities.GetData(path);
            return activeFilter;
        }

        public static void SetFilter(ActiveFilter filter, string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }
            
            JsonUtilities.SetData(filter, path);
        }
    }
}