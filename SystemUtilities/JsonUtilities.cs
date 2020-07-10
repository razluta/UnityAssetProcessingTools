using System.Collections.Generic;
using UnityEngine;

namespace UnityAssetProcessingTools.SystemUtilities
{
    public static class JsonUtilities
    {
        public static Dictionary<string, string> GetData(string path)
        {
            var data = new Dictionary<string, string>();
            return data;
        }
        
        public static void SetData(Dictionary<string, string> data, string path)
        {
            var jsonData = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(path, jsonData);
        }
    }
}