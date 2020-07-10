using System.Collections.Generic;

namespace UnityAssetProcessingTools
{
    public class ActiveFilter
    {
        public string BrowsePath { get; set; }
        public bool IsRecursive { get; set; }
        public string NameStartsWith  { get; set; }
        public string NameContains  { get; set; }
        public string NameEndsWith  { get; set; }
        public int DiskSize  { get; set; }

        public enum FilterType
        {
            NoActiveFilter,
            UntitledFilter,
            LoadedFilter
        }
    }
}
