using System;
using System.Collections.Generic;
using UnityAssetProcessingTools.SystemUtilities;

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
            var activeFilter = new ActiveFilter();
            
            if (path == null)
            {
                return activeFilter;
            }
            
            var data = JsonUtilities.GetData(path);
            
            if(data.ContainsKey(BrowsePath))
            {
                activeFilter.BrowsePath = data[BrowsePath];
            }
            if(data.ContainsKey(IsRecursive))
            {
                activeFilter.IsRecursive = Convert.ToBoolean(data[IsRecursive]);
            }
            if(data.ContainsKey(NameStartsWith))
            {
                activeFilter.NameStartsWith = data[NameStartsWith];
            }
            if(data.ContainsKey(NameContains))
            {
                activeFilter.NameContains = data[NameContains];
            }
            if (data.ContainsKey(NameEndsWith))
            {
                activeFilter.NameEndsWith = data[NameEndsWith];
            }
            if (data.ContainsKey(DiskSize))
            {
                activeFilter.DiskSize = Convert.ToInt32(data[DiskSize]);
            }

            return activeFilter;
        }

        public static void SetFilter(ActiveFilter filter, string path)
        {
            var data = new Dictionary<string, string>() {};
            
            data[BrowsePath] = filter.BrowsePath;
            data[IsRecursive] = filter.IsRecursive.ToString();
            data[NameStartsWith] = filter.NameStartsWith;
            data[NameContains] = filter.NameContains;
            data[NameEndsWith] = filter.NameEndsWith;
            data[DiskSize] = filter.DiskSize.ToString();
            
            JsonUtilities.SetData(data, path);
        }
    }
}