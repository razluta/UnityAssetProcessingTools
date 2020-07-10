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
            Debug.Log("In SetData(), BrowsePath is " + data[AssetProcessingTools.BrowsePath]);

            foreach (var line in data)
            {
                Debug.Log(line);
            }
            
            var jsonData = JsonUtility.ToJson(data, true);
            Debug.Log(jsonData);
            System.IO.File.WriteAllText(path, jsonData);
        }
    }
}