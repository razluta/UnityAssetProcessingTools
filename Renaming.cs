using System;

namespace UnityAssetProcessingTools
{
    public class Renaming
    {
        public string ReplacementSource { get; set; }
        public string ReplacementTarget { get; set; }
        
        public string Prefix { get; set; }
        
        public string Suffix { get; set; }
        
        public string CapitalizeAfter { get; set; }
        
        public string LowercaseAfter { get; set; }
        
        public string Remove { get; set; }
        
        public string InsertSource { get; set; }
        public string InsertTarget { get; set; }
        
        public bool UppercaseAll { get; set; }
        public bool LowercaseAll { get; set; }
    }

    public class RenamingUtilities
    {
        public static string RenameAsset(string assetName, Renaming renamingConditions)
        {
            if (renamingConditions == null)
            {
                return assetName;
            }
            
            var renamedAssetName = assetName;

            // replace ... with ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.ReplacementSource) && 
                !String.IsNullOrWhiteSpace(renamingConditions.ReplacementTarget))
            {
                if (renamedAssetName.Contains(renamingConditions.ReplacementSource))
                {
                    renamedAssetName = renamedAssetName.Replace(
                        renamingConditions.ReplacementSource,
                        renamingConditions.ReplacementTarget);
                }
            }
            
            // prefix with ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.Prefix))
            {
                renamedAssetName = renamingConditions.Prefix + renamedAssetName;
            }
            
            // suffix with ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.Suffix))
            {
                renamedAssetName += renamingConditions.Suffix;
            }
            
            // capitalize the letter after ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.CapitalizeAfter))
            {
                if (renamedAssetName.Contains(renamingConditions.CapitalizeAfter))
                {
                    // TODO
                }
            }
            
            // lowercase the letter after ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.LowercaseAfter))
            {
                if (renamedAssetName.Contains(renamingConditions.LowercaseAfter))
                {
                    // TODO
                }
            }
            
            // remove ...
            if (!String.IsNullOrWhiteSpace(renamingConditions.Remove))
            {
                if (renamedAssetName.Contains(renamingConditions.Remove))
                {
                    renamedAssetName = renamedAssetName.Replace(
                        renamingConditions.Remove, 
                        String.Empty);
                }
            }
            
            // insert ... after ...
            if (true)
            {
                //  TODO
            }
            
            // uppercase all
            if (renamingConditions.UppercaseAll)
            {
                renamedAssetName = renamedAssetName.ToUpper();
            }
            
            // lowercase all
            if (renamingConditions.LowercaseAll)
            {
                renamedAssetName = renamedAssetName.ToLower();
            }
            
            return renamedAssetName;
        }
    }

}