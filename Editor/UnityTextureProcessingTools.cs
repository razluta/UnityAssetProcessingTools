using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityTextureProcessingTools.Editor
{
    public class UnityTextureProcessingTools : EditorWindow                                                                                  
    {                                                                                                                                        
        private VisualElement _root;                                                                                                         
                                                                                                                                             
        [MenuItem("Art Tools/Texture Processing Tools")]                                                                                     
        public static void ShowWindow()                                                                                                      
        {                                                                                                                                    
            // Opens the window, otherwise focuses it if it’s already open.                                                                  
            var window = GetWindow<UnityTextureProcessingTools>();                                                                           
                                                                                                                                             
            // Adds a title to the window.                                                                                                   
            window.titleContent = new GUIContent("Texture Processing Tools");                                                                
                                                                                                                                             
            // Sets a minimum size to the window.                                                                                            
            window.minSize = new Vector2(350, 600);                                                                                          
        }                                                                                                                                    
                                                                                                                                             
        private void OnEnable()                                                                                                              
        {                                                                                                                                    
            // Reference to the root of the window.                                                                                          
            _root = rootVisualElement;                                                                                                       
                                                                                                                                             
            // Associates a stylesheet to our root. Thanks to inheritance, all root’s                                                        
            // children will have access to it.                                                                                              
            _root.styleSheets.Add(Resources.Load<StyleSheet>(
                "UnityTextureProcessingToolsEditorWindow"));                                    
                                                                                                                                             
            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.                                                     
            var mainVisualTree = Resources.Load<VisualTreeAsset>(
                "UnityTextureProcessingToolsEditorWindow");                                 
            mainVisualTree.CloneTree(_root);                                                                                                 
        }                                                                                                                                    
    }                                                                                                                                        
}
