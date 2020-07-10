using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityAssetProcessingTools.Editor
{
    public class AssetProcessingToolsEditor : EditorWindow                                                                                  
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
        private Button _browse;
        private Button _save;
        private Button _load;
        private Label _pathLabel;
        private VisualTreeAsset _versionInfoVisualTreeAsset;
        private VisualElement _versionInfoVisualElement;

        private static ActiveFilter _filter;
        private static string _filterPath = string.Empty;
        
        private static string _browsePath = string.Empty;
        private static string _isRecursive = Convert.ToString((bool) true);
        private static string _nameStartsWith = string.Empty;
        private static string _nameContains = string.Empty;
        private static string _nameEndsWith = string.Empty;
        private static string _diskSize = Convert.ToString((int)0);
        
        private Dictionary<string, string> _filterData;

        [MenuItem("Art Tools/Launch Asset Processing Tools")]                                                                                     
        public static void ShowWindow()                                                                                                      
        {                                                                                                                                    
            // Opens the window, otherwise focuses it if it’s already open.                                                                  
            var window = GetWindow<AssetProcessingToolsEditor>();    
            
            // Setup the filter
            _filter = AssetProcessingTools.GetFilter(_filterPath);
                
                                                                                                                                             
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
            _filter = AssetProcessingTools.GetFilter();
            
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
            _filterTabButton.clickable.clicked += ShowFilterTab;
            
            _tabButtonVisualTreeAsset.CloneTree(_toolsTabsVisualElement);
            _renamingTabButton = _root.Q<Button>("BT_Tab");
            _renamingTabButton.name = "BT_RenamingTab";
            _renamingTabButton.text = "Renaming";
            _renamingTabButton.clickable.clicked += ShowRenamingTab;
            
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
            
            // Browse Button
            _browse = _root.Q<Button>("BT_Browse");
            _browse.clickable.clicked += Browse;
            _pathLabel = _root.Q<Label>("LB_BrowsePath");
            
            // Save Button
            _save = _root.Q<Button>("BT_Save");
            _save.clickable.clicked += () => SaveFilter(AssetProcessingTools.AssetTypes.AllAssetTypes);
            
            // Save Button
            _load = _root.Q<Button>("BT_Load");
            _load.clickable.clicked += LoadFilter;

            // Version info
            _versionInfoVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_Version");
            _versionInfoVisualTreeAsset.CloneTree(_root);
            _versionInfoVisualElement = _root.Q<VisualElement>("CS_Version");
            _versionInfoVisualElement.style.flexShrink = 0;
            _versionInfoVisualElement.style.flexGrow = 0;
            
            // Bind Ui to Data
            
        }

        private void BindFilterToUi(ActiveFilter filter)
        {
            
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

        private void Browse()
        {
            var path = EditorUtility.OpenFolderPanel("", "", "");
            if (path.Length == 0)
            {
                return;
            }
            _browsePath = path;
            _pathLabel.text = _browsePath;
        }

        private void SaveFilter(AssetProcessingTools.AssetTypes assetType)
        {
            _filterPath = EditorUtility.SaveFilePanel(
                "", "", "filter_data", "json");

            // TODO convert IF to a SWITCH and add the other types
            if (assetType == AssetProcessingTools.AssetTypes.AllAssetTypes)
            {
                _filterData = new Dictionary<string, string>()
                {
                    {AssetProcessingTools.BrowsePath, _browsePath},
                    {AssetProcessingTools.IsRecursive, _isRecursive},
                    {AssetProcessingTools.NameStartsWith, _nameStartsWith},
                    {AssetProcessingTools.NameContains, _nameContains},
                    {AssetProcessingTools.NameEndsWith, _nameEndsWith},
                    {AssetProcessingTools.DiskSize, _diskSize}
                };
            }

            _filter = CreateCurrentFilter(_filterData);
            AssetProcessingTools.SetFilter(_filter, _filterPath);
            AssetDatabase.Refresh();
        }

        private ActiveFilter CreateCurrentFilter(Dictionary<string, string> data)
        {
            var activeFilter = new ActiveFilter();

            if(data.ContainsKey(AssetProcessingTools.BrowsePath))
            {
                activeFilter.BrowsePath = data[AssetProcessingTools.BrowsePath];
            }
            if(data.ContainsKey(AssetProcessingTools.IsRecursive))
            {
                activeFilter.IsRecursive = Convert.ToBoolean(data[AssetProcessingTools.IsRecursive]);
            }
            if(data.ContainsKey(AssetProcessingTools.NameStartsWith))
            {
                activeFilter.NameStartsWith = data[AssetProcessingTools.NameStartsWith];
            }
            if(data.ContainsKey(AssetProcessingTools.NameContains))
            {
                activeFilter.NameContains = data[AssetProcessingTools.NameContains];
            }
            if (data.ContainsKey(AssetProcessingTools.NameEndsWith))
            {
                activeFilter.NameEndsWith = data[AssetProcessingTools.NameEndsWith];
            }
            if (data.ContainsKey(AssetProcessingTools.DiskSize))
            {
                activeFilter.DiskSize = Convert.ToInt32(data[AssetProcessingTools.DiskSize]);
            }

            return activeFilter;
        }

        private void LoadFilter()
        {
            _filterPath = EditorUtility.OpenFilePanel("", "", "json");
            _filter = AssetProcessingTools.GetFilter(_filterPath);

            Debug.Log(_filter);
        }
    }                                                                                                                                        
}
