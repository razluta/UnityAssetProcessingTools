using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityAssetProcessingTools.Editor
{
    public class AssetProcessingToolsEditor : EditorWindow
    {
        private const int WindowWidth = 350;
        private const int WindowHeight = 650;
        
        // private string _version = "v.0.0.1.20200710";

        private bool _isFilterTab = true;
        private bool _isTexturesSubtab = false;
        private bool _isToolsTab = false;
        private bool _isRenamingSubtab = false;
        
        private VisualElement _root;
        private VisualTreeAsset _mainVisualTree;

        private VisualTreeAsset _wideButtonVisualTreeAsset;
        private Button _loadFilterButton;
        private Button _saveFilterButton;
        private Button _clearFilterButton;
        private Button _confirmFilterButton;
            
        private VisualTreeAsset _activeFilterVisualTreeAsset;
        private VisualElement _toolsTabsVisualElement;
        private VisualTreeAsset _tabButtonVisualTreeAsset;
        private VisualTreeAsset _filterTabVisualTreeAsset;
        private VisualTreeAsset _renamingTabVisualTreeAsset;
        private Button _filterTabButton;
        private Button _renamingTabButton;
        private VisualElement _assetTypesVisualElement;
        private Button _allAssetTypeTabButton;
        private Button _textureTypeTabButton;
        private VisualTreeAsset _filterAllAssetsVisualTreeAsset;
        private VisualTreeAsset _filterTexturesTreeAsset;
        private VisualElement _filterAllAssetsTabContentsVisualElement;
        private VisualElement _filterTexturesTabContentsVisualElement;
        private Button _browse;
        private Label _pathLabel;
        private VisualTreeAsset _versionInfoVisualTreeAsset;
        private VisualElement _versionInfoVisualElement;
        private Button _undoOrEditFilterButton;
        private Button _renameButton;

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
            window.minSize = new Vector2(WindowWidth, WindowHeight);                                                                                          
        }                                                                                                                                    
                                                                                                                                             
        private void OnEnable()                                                                                                              
        {                                                                                                                                    
            // Reference to the root of the window.                                                                                          
            _root = rootVisualElement;

            // Initiate the GUI
            InitUi();
        }

        private void ResetFilter()
        {
            _isFilterTab = true;
            _isToolsTab = false;
            InitUi();
        }

        private void InitUi()
        {
            // Clear Ui
            _root.Clear();
            
            if (_isFilterTab)
            {
                InitFilterUi();
                // Initiate default state
                ShowFilterTab();
            }
            else if (_isToolsTab)
            {
                InitToolsUi();
            }
            
            _root.MarkDirtyRepaint();
        }

        private void InitFilterUi()
        {
            _tabButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_Tab");

            // Load Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _loadFilterButton = _root.Q<Button>("BT_WideButton");
            _loadFilterButton.name = "BT_LoadFilter";
            _loadFilterButton.text = "LOAD FILTER";
            _loadFilterButton.clickable.clicked += LoadFilter;
            
            // Save Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _saveFilterButton = _root.Q<Button>("BT_WideButton");
            _saveFilterButton.name = "BT_SaveFilter";
            _saveFilterButton.text = "SAVE FILTER";
            _saveFilterButton.clickable.clicked += () => SaveFilter(AssetProcessingTools.AssetTypes.AllAssetTypes);

            // Clear Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _clearFilterButton = _root.Q<Button>("BT_WideButton");
            _clearFilterButton.name = "BT_ClearFilter";
            _clearFilterButton.text = "CLEAR FILTER";
            _clearFilterButton = _root.Q<Button>("BT_ClearFilter");
            _clearFilterButton.clickable.clicked += ClearFilter;
            
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
            _allAssetTypeTabButton = _root.Q<Button>("BT_Tab");
            _allAssetTypeTabButton.name = "BT_AllAssetTypes";
            _allAssetTypeTabButton.text = "All Asset Types";
            _allAssetTypeTabButton.clickable.clicked += HideAllFilters;
            
            _tabButtonVisualTreeAsset.CloneTree(_assetTypesVisualElement);
            _textureTypeTabButton = _root.Q<Button>("BT_Tab");
            _textureTypeTabButton.name = "BT_TextureType";
            _textureTypeTabButton.text = "Textures";
            _textureTypeTabButton.clickable.clicked += ShowTextureFilters;
            
            // Filter All Assets Tab Contents
            _filterAllAssetsVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterAllAssetsTab");
            _filterAllAssetsVisualTreeAsset.CloneTree(_root);
            _filterAllAssetsTabContentsVisualElement = _root.Q<VisualElement>("VE_FilterAllAssetsTabContents");
            _filterAllAssetsTabContentsVisualElement.style.flexShrink = 0;
            _filterAllAssetsTabContentsVisualElement.style.flexGrow = 1;
            
            // Filter Textures Tab Contents
            if (_isTexturesSubtab)
            {
                _filterTexturesTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterTexturesTab");
                _filterTexturesTreeAsset.CloneTree(_root);
                _filterTexturesTabContentsVisualElement = _root.Q<VisualElement>("VE_FilterTexturesTabContents");
                _filterTexturesTabContentsVisualElement.style.flexShrink = 0;
                _filterTexturesTabContentsVisualElement.style.flexGrow = 1;
            }

            // Browse Button
            _browse = _root.Q<Button>("BT_Browse");
            _browse.clickable.clicked += Browse;
            _pathLabel = _root.Q<Label>("LB_BrowsePath");

            // Confirm Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _confirmFilterButton = _root.Q<Button>("BT_WideButton");
            _confirmFilterButton.name = "BT_ConfirmFilter";
            _confirmFilterButton.text = "CONFIRM FILTER";
            _confirmFilterButton.clickable.clicked += ConfirmFilter;

            AddVersionInfoVisualElement();
            
            // Bind Ui to Data
            BindFilterToUi(_filter);
        }

        private void HideAllFilters()
        {
            _isTexturesSubtab = false;
            InitUi();
        }
        
        private void ShowTextureFilters()
        {
            _isTexturesSubtab = true;
            InitUi();
        }

        private void AddVersionInfoVisualElement()
        {
            // Version info
            _versionInfoVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_Version");
            _versionInfoVisualTreeAsset.CloneTree(_root);
            _versionInfoVisualElement = _root.Q<VisualElement>("CS_Version");
            _versionInfoVisualElement.style.flexShrink = 0;
            _versionInfoVisualElement.style.flexGrow = 0;
        }

        private void InitToolsUi()
        {
            // Active filter
            _activeFilterVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterActive");
            _activeFilterVisualTreeAsset.CloneTree(_root);
            _filter = AssetProcessingTools.GetFilter();
            
            // Undo (Edit Filter) Button
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _undoOrEditFilterButton = _root.Q<Button>("BT_WideButton");
            _undoOrEditFilterButton.name = "BT_UndoOrEditFilter";
            _undoOrEditFilterButton.text = "RESET FILTER";
            _undoOrEditFilterButton.clickable.clicked += ResetFilter;
            
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

            _tabButtonVisualTreeAsset.CloneTree(_toolsTabsVisualElement);
            _renamingTabButton = _root.Q<Button>("BT_Tab");
            _renamingTabButton.name = "BT_RenamingTab";
            _renamingTabButton.text = "Renaming";
            _renamingTabButton.clickable.clicked += ShowRenamingTab;
            
            // Renaming tab
            _renamingTabVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_RenamingTab");
            _renamingTabVisualTreeAsset.CloneTree(_root);

            // Rename Button
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _renameButton = _root.Q<Button>("BT_WideButton");
            _renameButton.name = "BT_Rename";
            _renameButton.text = "RENAME";
            // _renameButton.clickable.clicked += Rename;

            AddVersionInfoVisualElement();
        }

        private void BindFilterToUi(ActiveFilter filter)
        {
            // _pathLabel.text = filter.BrowsePath;
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
            if (String.IsNullOrWhiteSpace(_filterPath))
            {
                return;
            }
            
            _filter = AssetProcessingTools.GetFilter(_filterPath);
            BindFilterToUi(_filter);
        }

        private void ClearFilter()
        {
            _filter = AssetProcessingTools.GetDefaultFilter();
            BindFilterToUi(_filter);
        }

        private void ConfirmFilter()
        {
            _isFilterTab = false;
            _isToolsTab = true;
            InitUi();
        }
        
    }                                                                                                                                        
}
