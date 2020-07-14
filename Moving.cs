using System;
using UnityEditor;

namespace UnityAssetProcessingTools
{
    public class Moving
    {
        public string AssetNewAbsolutePath { get; set; }
    }
    
    public static class MovingUtilities
    {
        public static void MoveAsset(string assetRelativePath, Moving movingConditions)
        {
            if (movingConditions == null)
            {
                return;
            }
            
            // Move asset to
            if (!String.IsNullOrWhiteSpace(movingConditions.AssetNewAbsolutePath))
            {
                if (ProjectSearch.IsPathInProject(movingConditions.AssetNewAbsolutePath))
                {
                    var assetNewRelativePath = ProjectSearch.GetRelativeAssetPath(
                        movingConditions.AssetNewAbsolutePath);

                    if (String.IsNullOrWhiteSpace(assetNewRelativePath))
                    {
                        return;
                    }
                    
                    // Move the asset
                    AssetDatabase.MoveAsset(assetRelativePath, assetNewRelativePath);
                }
            }
            
        }
    }
}