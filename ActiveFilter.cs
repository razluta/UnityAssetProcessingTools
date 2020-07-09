namespace UnityAssetProcessingTools
{
    public class ActiveFilter
    {
        private string AssetDirectoryPath { get; set; }
        private bool IsDirectorySearchRecursive { get; set; }
        private string NameStartsWith  { get; set; }
        private string NameContains  { get; set; }
        private string NameEndsWith  { get; set; }
        private int DiskSize  { get; set; }
    }
}



