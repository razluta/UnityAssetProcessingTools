using System;
using UnityEditor;
using UnityAssetProcessingTools.AssetUtilities;

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
                if (PathUtilities.IsPathInProject(movingConditions.AssetNewAbsolutePath))
                {
                    var assetNewRelativePath = PathUtilities.GetRelativeAssetPath(
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