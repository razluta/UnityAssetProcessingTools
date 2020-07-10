using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace UnityAssetProcessingTools.SystemUtilities
{
    public static class JsonUtilities
    {
        public static ActiveFilter GetData(string path)
        {
            var file = File.OpenText(path);
            var serializers = new JsonSerializer();
            var filter = (ActiveFilter) serializers.Deserialize(file, typeof(ActiveFilter));
            return filter;
        }
        
        public static void SetData(ActiveFilter data, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
    }
}