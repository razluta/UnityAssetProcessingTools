using System;
using UnityAssetProcessingTools.SystemUtilities;

namespace UnityAssetProcessingTools
{
    public static class AssetProcessingTools
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
                return GetDefaultFilter();
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

        public static ActiveFilter GetDefaultFilter()
        {
            var filter = new ActiveFilter()
            {
                BrowsePath = "",
                IsRecursive = true,
                NameStartsWith = "",
                NameContains = "",
                NameEndsWith = "",
                DiskSize = 0,
                ExcludedExtensions = null
            };

            return filter;
        }
    }
}