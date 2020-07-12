using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityAssetProcessingTools
{
    public class ActiveFilter
    {
        public enum FilterAssetType
        {
            AllAssetsFilter,
            TexturesFilter
        }
        
        public string BrowsePath { get; set; }
        public bool IsRecursive { get; set; }
        public string NameStartsWith  { get; set; }
        public string NameContains  { get; set; }
        public string NameEndsWith  { get; set; }
        public int DiskSize  { get; set; }
        public List<string> ExcludedExtensions { get; set; }
    }
}
