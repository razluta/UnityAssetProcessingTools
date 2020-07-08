using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityAssetProcessingTools.Editor
{
    public class UnityAssetProcessingTools : EditorWindow                                                                                  
    {                                                                                                                                        
        private VisualElement _root;   

        private TemplateContainer _filterTabTemplateContainer;
        private TemplateContainer _renamingTabTemplateContainer;
        private TemplateContainer _resizingTabTemplateContainer;

        private VisualElement _filterTabVisualElement;
        private VisualElement _renamingTabVisualElement;
        private VisualElement _resizingTabVisualElement;
                                                                                                                                             
        [MenuItem("Art Tools/Launch Asset Processing Tools")]                                                                                     
        public static void ShowWindow()                                                                                                      
        {                                                                                                                                    
            // Opens the window, otherwise focuses it if it’s already open.                                                                  
            var window = GetWindow<UnityAssetProcessingTools>();                                                                           
                                                                                                                                             
            // Adds a title to the window.                                                                                                   
            window.titleContent = new GUIContent("Asset Processing Tools");                                                                
                                                                                                                                             
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
                "UnityAssetProcessingToolsEditorWindow"));                                    
                                                                                                                                             
            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.                                                     
            var mainVisualTree = Resources.Load<VisualTreeAsset>(
                "UnityAssetProcessingToolsEditorWindow");                                 
            mainVisualTree.CloneTree(_root);
            
            // Get references to the tab buttons
            _filterTabTemplateContainer = _root.Q<TemplateContainer>("BT_FilterTab");
            var filterTabButton = (Button) _filterTabTemplateContainer.Children().ToArray()[0];
            filterTabButton.text = "Filter";
            filterTabButton.clickable.clicked += () => ShowFilterTab();
            
            _renamingTabTemplateContainer = _root.Q<TemplateContainer>("BT_RenamingTab");
            var renamingTabButton = (Button) _renamingTabTemplateContainer.Children().ToArray()[0];
            renamingTabButton.text = "Renaming";
            renamingTabButton.clickable.clicked += () => ShowFilterTab();

            // Get references to the tab contents
            _filterTabVisualElement = _root.Q<VisualElement>("VE_FilterTab");

            // Initiate default state
            ShowFilterTab();
        }

        private void HideAllTabs()
        {
            // _filterTabVisualElement.Clear();
        }
        
        private void ShowFilterTab()
        {
            HideAllTabs();
        }
        
    }                                                                                                                                        
}
