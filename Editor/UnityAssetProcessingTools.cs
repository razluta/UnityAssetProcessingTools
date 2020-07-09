using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityAssetProcessingTools.Editor
{
    public class UnityAssetProcessingTools : EditorWindow                                                                                  
    {                                                                                                                                        
        private VisualElement _root;
        private VisualTreeAsset _mainVisualTree;

        private VisualTreeAsset _activeFilterVisualTreeAsset;
        private VisualElement _toolsTabsVisualElement;
        private VisualTreeAsset _tabButtonVisualTreeAsset;
        private TemplateContainer _filterTabTemplateContainer;
        private TemplateContainer _renamingTabTemplateContainer;
        private Button _filterTabButton;
        private Button _renamingTabButton;
        private VisualElement _assetTypesVisualElement;
        private Button _allAssetTypeTabButton;
        private VisualTreeAsset _filterAllAssetsVisualTreeAsset;
        private VisualElement _filterAllAssetsTabContentsVisualElement;
        private VisualTreeAsset _versionInfoVisualTreeAsset;
        private VisualElement _versionInfoVisualElement;

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
                                                                                                                                             
            // // Loads and clones our VisualTree (eg. our UXML structure) inside the root.                                                     
            // _mainVisualTree = Resources.Load<VisualTreeAsset>(
            //     "UnityAssetProcessingToolsEditorWindow");                                 
            // _mainVisualTree.CloneTree(_root);

            // Initiate the GUI
            InitUi();

            // Initiate default state
            ShowFilterTab();
        }

        private void InitUi()
        {
            // Active filter
            _activeFilterVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterActive");
            _activeFilterVisualTreeAsset.CloneTree(_root);
            
            // Tools tabs
            _toolsTabsVisualElement = new VisualElement();
            _root.Add(_toolsTabsVisualElement);
            _toolsTabsVisualElement.style.flexDirection = FlexDirection.Row;
            _toolsTabsVisualElement.style.flexShrink = 0;
            _toolsTabsVisualElement.style.flexGrow = 0;
            _toolsTabsVisualElement.style.marginBottom = 8;
            _toolsTabsVisualElement.style.marginTop = 8;
            _toolsTabsVisualElement.style.alignItems = Align.Center;
            _toolsTabsVisualElement.style.justifyContent = Justify.Center;
            
            _tabButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_Tab");
            
            _tabButtonVisualTreeAsset.CloneTree(_toolsTabsVisualElement);
            _filterTabButton = _root.Q<Button>("BT_Tab");
            _filterTabButton.name = "BT_FilterTab";
            _filterTabButton.text = "Filter";
            _filterTabButton.clickable.clicked += () => ShowFilterTab();
            
            _tabButtonVisualTreeAsset.CloneTree(_toolsTabsVisualElement);
            _renamingTabButton = _root.Q<Button>("BT_Tab");
            _renamingTabButton.name = "BT_RenamingTab";
            _renamingTabButton.text = "Renaming";
            _renamingTabButton.clickable.clicked += () => ShowRenamingTab();
            
            // Asset Type Tab
            _assetTypesVisualElement = new VisualElement();
            _root.Add(_assetTypesVisualElement);
            _assetTypesVisualElement.style.flexDirection = FlexDirection.Row;
            _assetTypesVisualElement.style.flexShrink = 0;
            _assetTypesVisualElement.style.flexGrow = 0;
            _assetTypesVisualElement.style.marginBottom = 8;
            _assetTypesVisualElement.style.marginTop = 8;
            _assetTypesVisualElement.style.alignItems = Align.Center;
            _assetTypesVisualElement.style.justifyContent = Justify.Center;
            
            _tabButtonVisualTreeAsset.CloneTree(_assetTypesVisualElement);
            _allAssetTypeTabButton = _filterTabButton = _root.Q<Button>("BT_Tab");
            _allAssetTypeTabButton.name = "BT_AllAssetTypes";
            _allAssetTypeTabButton.text = "All Asset Types";
            
            // Filter All Assets Tab Contents
            _filterAllAssetsVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterAllAssetsTab");
            _filterAllAssetsVisualTreeAsset.CloneTree(_root);
            _filterAllAssetsTabContentsVisualElement = _root.Q<VisualElement>("VE_FilterAllAssetsTabContents");
            _filterAllAssetsTabContentsVisualElement.style.flexShrink = 0;
            _filterAllAssetsTabContentsVisualElement.style.flexGrow = 1;
            
            // Version info
            _versionInfoVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_Version");
            _versionInfoVisualTreeAsset.CloneTree(_root);
            _versionInfoVisualElement = _root.Q<VisualElement>("CS_Version");
            _versionInfoVisualElement.style.flexShrink = 0;
            _versionInfoVisualElement.style.flexGrow = 0;
        }

        private void HideAllTabs()
        {
            // _filterTabVisualElement.Clear();
        }
        
        private void ShowFilterTab()
        {
            HideAllTabs();
        }
        
        private void ShowRenamingTab()
        {
            HideAllTabs();
        }
        
    }                                                                                                                                        
}
