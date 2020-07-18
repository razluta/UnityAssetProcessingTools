using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityAssetProcessingTools.AssetUtilities;

namespace UnityAssetProcessingTools.Editor
{
    public class AssetProcessingToolsEditor : EditorWindow
    {
        // private string _version = "v.0.0.1.20200710";
        
        private const int WindowWidth = 1000;
        private const int WindowHeight = 1000;

        private readonly StyleColor _colorTabActive = new StyleColor(new Color(0.45f, 0.45f, 0.45f));
        private readonly StyleColor _colorTabInactive = new StyleColor(new Color(0.34f, 0.34f, 0.34f));
        private readonly StyleColor _colorGreyLine = new StyleColor(new Color(0.14f, 0.14f, 0.14f));

        private bool _isFilterTab = true;
        private bool _isAllAssetsSubtab = true;
        private bool _isTexturesSubtab = false;
        private bool _isToolsTab = false;
        private bool _isRenamingSubtab = true;
        private bool _isMovingSubtab = false;
        
        private VisualElement _root;
        private VisualTreeAsset _mainVisualTree;

        private VisualTreeAsset _wideButtonVisualTreeAsset;
        private Button _loadFilterButton;
        private Button _saveFilterButton;
        private Button _clearFilterButton;
        private Button _applyFilterButton;
        private Button _confirmFilterButton;

        // Includes
        private Button _includeNameStartsWithButton;
        private Foldout _includeNameStartsWithFields;
        private Button _includeNameContainsButton;
        private Foldout _includeNameContainsFields;
        private Button _includeNameEndsWithButton;
        private Foldout _includeNameEndsWithFields;
        private TextField _includeDiskSizeTextField;
        private Foldout _includeDiskSizeFields;
        
        // Excludes
        private Button _excludeNameStartsWithButton;
        private Foldout _excludeNameStartsWithFields;
        private Button _excludeNameContainsButton;
        private Foldout _excludeNameContainsFields;
        private Button _excludeNameEndsWithButton;
        private Foldout _excludeNameEndsWithFields;
        private TextField _excludeDiskSizeTextField;
        private Foldout _excludeDiskSizeFields;

        private VisualElement _toolsTabsVisualElement;
        private VisualTreeAsset _tabButtonVisualTreeAsset;
        private VisualTreeAsset _filterTabVisualTreeAsset;
        private VisualTreeAsset _renamingTabVisualTreeAsset;
        private VisualTreeAsset _movingTabVisualTreeAsset;
        private Button _filterTabButton;
        private Button _renamingTabButton;
        private Button _movingTabButton;
        private VisualElement _assetTypesVisualElement;
        private Button _allAssetTypeTabButton;
        private Button _textureTypeTabButton;
        private VisualTreeAsset _filterAllAssetsVisualTreeAsset;
        private VisualTreeAsset _filterTexturesTreeAsset;
        private VisualElement _filterAllAssetsTabContentsVisualElement;
        private VisualElement _filterAllAssetsIgnorelistTabContentsVisualElement;
        private VisualElement _filterTexturesTabContentsVisualElement;
        private Button _browse;
        private Label _pathLabel;
        private ScrollView _filterResultsScrollView;
        private VisualTreeAsset _versionInfoVisualTreeAsset;
        private VisualElement _versionInfoVisualElement;
        private Button _undoOrEditFilterButton;
        private Button _renameButton;
        private Button _moveButton;

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
            
            // Establish references

            // Load necessary references for UXML data
            
            // Load necessary references for USS data

            
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
            _allAssetTypeTabButton.clickable.clicked += ShowAllAssetsFilters;

            _tabButtonVisualTreeAsset.CloneTree(_assetTypesVisualElement);
            _textureTypeTabButton = _root.Q<Button>("BT_Tab");
            _textureTypeTabButton.name = "BT_TextureType";
            _textureTypeTabButton.text = "Textures";
            _textureTypeTabButton.clickable.clicked += ShowTextureFilters;
            
            // Filter All Assets Tab Contents
            if (_isAllAssetsSubtab)
            {
                _filterAllAssetsVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterAllAssetsTab");
                _filterAllAssetsVisualTreeAsset.CloneTree(_root);
                
                _filterAllAssetsTabContentsVisualElement = _root.Q<VisualElement>("VE_FilterAllAssetsTabContents");
                _filterAllAssetsTabContentsVisualElement.style.flexShrink = 0;
                _filterAllAssetsTabContentsVisualElement.style.flexGrow = 1;
                
                // Style update
                SetAllFilterTabButtonStylesAsInactive();
                SetTabButtonStyle(_allAssetTypeTabButton, true);
            }
            
            // Filter Textures Tab Contents
            if (_isTexturesSubtab)
            {
                _filterTexturesTreeAsset = Resources.Load<VisualTreeAsset>("CS_FilterTexturesTab");
                _filterTexturesTreeAsset.CloneTree(_root);
                
                _filterTexturesTabContentsVisualElement = _root.Q<VisualElement>("VE_FilterTexturesTabContents");
                _filterTexturesTabContentsVisualElement.style.flexShrink = 0;
                _filterTexturesTabContentsVisualElement.style.flexGrow = 1;

                // Style update
                SetAllFilterTabButtonStylesAsInactive();
                SetTabButtonStyle(_textureTypeTabButton, true);
            }

            // Include Browse Button
            _browse = _root.Q<Button>("BT_Browse");
            _browse.clickable.clicked += Browse;
            _pathLabel = _root.Q<Label>("LB_BrowsePath");
            
            // Include Name Starts With
            _includeNameStartsWithButton = _root.Q<Button>("BT_IncludeNameStartsWith");
            _includeNameStartsWithFields = _root.Q<Foldout>("FO_IncludeNameStartsWith");
            _includeNameStartsWithButton.clickable.clicked += () => AddNewEntryToFoldout(_includeNameStartsWithFields);
            
            // Include Name Contains
            _includeNameContainsButton = _root.Q<Button>("BT_IncludeNameContains");
            _includeNameContainsFields = _root.Q<Foldout>("FO_IncludeNameContains");
            _includeNameContainsButton.clickable.clicked += () => AddNewEntryToFoldout(_includeNameContainsFields);
            
            // Include Name Ends With
            _includeNameEndsWithButton = _root.Q<Button>("BT_IncludeNameEndsWith");
            _includeNameEndsWithFields = _root.Q<Foldout>("FO_IncludeNameEndsWith");
            _includeNameEndsWithButton.clickable.clicked += () => AddNewEntryToFoldout(_includeNameEndsWithFields);
            
            // Include Disk Size
            _includeDiskSizeFields = _root.Q<Foldout>("FO_IncludeDiskSize");
            _includeDiskSizeTextField = _root.Q<TextField>("TF_IncludeDiskSize");

            // Apply Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _applyFilterButton = _root.Q<Button>("BT_WideButton");
            _applyFilterButton.name = "BT_ApplyFilter";
            _applyFilterButton.text = "APPLY FILTER";
            _applyFilterButton.clickable.clicked += ApplyFilter;
            
            // Exclude Name Starts With
            _excludeNameStartsWithButton = _root.Q<Button>("BT_ExcludeNameStartsWith");
            _excludeNameStartsWithFields = _root.Q<Foldout>("FO_ExcludeNameStartsWith");
            _excludeNameStartsWithButton.clickable.clicked += () => AddNewEntryToFoldout(_excludeNameStartsWithFields);
            
            // Exclude Name Contains
            _excludeNameContainsButton = _root.Q<Button>("BT_ExcludeNameContains");
            _excludeNameContainsFields = _root.Q<Foldout>("FO_ExcludeNameContains");
            _excludeNameContainsButton.clickable.clicked += () => AddNewEntryToFoldout(_excludeNameContainsFields);
            
            // Exclude Name Ends With
            _excludeNameEndsWithButton = _root.Q<Button>("BT_ExcludeNameEndsWith");
            _excludeNameEndsWithFields = _root.Q<Foldout>("FO_ExcludeNameEndsWith");
            _excludeNameEndsWithButton.clickable.clicked += () => AddNewEntryToFoldout(_excludeNameEndsWithFields);
            
            // Include Disk Size
            _excludeDiskSizeFields = _root.Q<Foldout>("FO_ExcludeDiskSize");
            _excludeDiskSizeTextField = _root.Q<TextField>("TF_ExcludeDiskSize");
            
            // Filter results
            var filterHolder = new VisualElement();
            filterHolder.style.borderTopColor = _colorGreyLine;
            filterHolder.style.borderTopWidth = 1;
            filterHolder.style.borderBottomColor = _colorGreyLine;
            filterHolder.style.borderBottomWidth = 1;
            
            var filterResultsHolder = new VisualElement();
            filterResultsHolder.style.flexShrink = 1;
            filterResultsHolder.style.flexGrow = 1;
            filterResultsHolder.style.borderLeftColor = new StyleColor(Color.black);
            filterResultsHolder.style.borderTopColor = new StyleColor(Color.black);
            filterResultsHolder.style.borderRightColor = new StyleColor(Color.black);
            filterResultsHolder.style.borderBottomColor = new StyleColor(Color.black);

            filterResultsHolder.style.borderLeftWidth = 1;
            filterResultsHolder.style.borderTopWidth = 1;
            filterResultsHolder.style.borderRightWidth = 1;
            filterResultsHolder.style.borderBottomWidth = 1;

            filterResultsHolder.style.marginLeft = 6;
            filterResultsHolder.style.marginTop = 6;
            filterResultsHolder.style.marginRight = 6;
            filterResultsHolder.style.marginBottom = 6;
            
            filterResultsHolder.style.paddingLeft = 3;
            filterResultsHolder.style.paddingTop = 3;
            filterResultsHolder.style.paddingRight = 3;
            filterResultsHolder.style.paddingBottom = 3;

            var filterResultsLabel = new Label()
            {
                text = "Filter Results"
            };
            filterResultsLabel.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            filterResultsLabel.style.marginTop = 6;

            _filterResultsScrollView = new ScrollView();

            InitializeFilterResults();

            filterResultsHolder.Add(_filterResultsScrollView);
            
            filterHolder.Add(filterResultsLabel);
            filterHolder.Add(filterResultsHolder);
            
            _root.Add(filterHolder);

            // Confirm Filter Button
            _wideButtonVisualTreeAsset = Resources.Load<VisualTreeAsset>("BT_WideButton");
            _wideButtonVisualTreeAsset.CloneTree(_root);
            _confirmFilterButton = _root.Q<Button>("BT_WideButton");
            _confirmFilterButton.name = "BT_ConfirmFilter";
            _confirmFilterButton.text = "CONFIRM FILTER LIST";
            _confirmFilterButton.clickable.clicked += ConfirmFilterList;

            AddVersionInfoVisualElement();
            
            // Bind Ui to Data
            BindFilterToUi(_filter);
        }

        private void SetAllFilterTabButtonStylesAsInactive()
        {
            SetTabButtonStyle(_allAssetTypeTabButton, false);
            SetTabButtonStyle(_textureTypeTabButton, false);
        }

        private void SetTabButtonStyle(Button button, bool active)
        {
            button.style.backgroundColor = active ? _colorTabActive : _colorTabInactive;
        }

        private void HideAllFilters()
        {
            _isAllAssetsSubtab = true;
            _isTexturesSubtab = false;
        }

        private void ShowAllAssetsFilters()
        {
            HideAllFilters();
            _isAllAssetsSubtab = true;
            InitUi();
        }
        
        private void ShowTextureFilters()
        {
            HideAllFilters();
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
            
            _tabButtonVisualTreeAsset.CloneTree(_toolsTabsVisualElement);
            _movingTabButton = _root.Q<Button>("BT_Tab");
            _movingTabButton.name = "BT_MovingTab";
            _movingTabButton.text = "Moving";
            _movingTabButton.clickable.clicked += ShowMovingTab;
            
            // Active Renaming Tab?
            if (_isRenamingSubtab)
            {
                // Renaming tab
                _renamingTabVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_RenamingTab");
                _renamingTabVisualTreeAsset.CloneTree(_root);
                
                // Rename Button
                _wideButtonVisualTreeAsset.CloneTree(_root);
                _renameButton = _root.Q<Button>("BT_WideButton");
                _renameButton.name = "BT_Rename";
                _renameButton.text = "RENAME";
                _renameButton.clickable.clicked += Rename;
                
                // Change styles
                SetAllToolTabButtonStylesAsInactive();
                SetTabButtonStyle(_renamingTabButton, true);
            }
            
            // Active Moving Tab?
            if (_isMovingSubtab)
            {
                // Moving tab
                _movingTabVisualTreeAsset = Resources.Load<VisualTreeAsset>("CS_MovingTab");
                _movingTabVisualTreeAsset.CloneTree(_root);
                
                // Rename Button
                _wideButtonVisualTreeAsset.CloneTree(_root);
                _moveButton = _root.Q<Button>("BT_WideButton");
                _moveButton.name = "BT_Move";
                _moveButton.text = "MOVE";
                _moveButton.clickable.clicked += Move;
                
                // Change styles
                SetAllToolTabButtonStylesAsInactive();
                SetTabButtonStyle(_movingTabButton, true);
            }
            
            AddVersionInfoVisualElement();
        }
        
        private void SetAllToolTabButtonStylesAsInactive()
        {
            SetTabButtonStyle(_renamingTabButton, false);
            SetTabButtonStyle(_movingTabButton, false);
        }

        private void BindFilterToUi(ActiveFilter filter)
        {
            // _pathLabel.text = filter.BrowsePath;
        }
        
        private void HideAllTools()
        {
            _isRenamingSubtab = false;
            _isMovingSubtab = false;
        }

        private void ShowRenamingTab()
        {
            HideAllTools();
            _isRenamingSubtab = true;
            InitUi();
        }
        
        private void ShowMovingTab()
        {
            HideAllTools();
            _isMovingSubtab = true;
            InitUi();
        }

        private void Browse()
        {
            var applicationDataPath = PathUtilities.GetApplicationDataPath();
            
            var path = EditorUtility.OpenFolderPanel("", applicationDataPath, "");
            if (path.Length == 0)
            {
                return;
            }

            if (!PathUtilities.IsPathInProject(path))
            {
                EditorUtility.DisplayDialog(
                    "Error - Invalid path", 
                    "Selected path must be within your project!", 
                    "Ok");
                _browsePath = applicationDataPath;
            }
            else
            {
                _browsePath = path;
            }
            
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
            InitializeFilterResults();
            BindFilterToUi(_filter);
        }

        private void ApplyFilter()
        {
            PopulateFilterResultsScrollView();
        }

        private void InitializeFilterResults()
        {
            _filterResultsScrollView.Clear();
            var foundAsset = new Label();
            foundAsset.text = "0 results found";
            _filterResultsScrollView.Add(foundAsset);
        }

        private void PopulateFilterResultsScrollView()
        {
            _filterResultsScrollView.Clear();

            var allAssetsRelativePaths = ProjectSearch.GetAllFileAssetRelativePaths();
            var totalAssetCount = allAssetsRelativePaths.Count;

            for (var i = 0; i < totalAssetCount; i++)
            {
                var assetRelativePath = allAssetsRelativePaths[i];
                var assetName = Path.GetFileName(assetRelativePath);

                // Show Progress Bar
                var fractionalValue = (float) (i + 1) / totalAssetCount;
                var percentage = fractionalValue * 100;
                EditorUtility.DisplayProgressBar(
                    "Filtering Assets",
                    String.Format("{0}% - Processing - {1}", percentage, assetName), 
                    fractionalValue);

                // Entry Button
                var resultHolder = new VisualElement();
                var filterResultButton = new Button();
                filterResultButton.text = assetName;
                filterResultButton.tooltip = assetRelativePath;
                filterResultButton.clickable.clicked += () => SelectCurrentFoundAsset(assetRelativePath);
                filterResultButton.style.backgroundColor = new StyleColor(Color.clear);
                filterResultButton.style.borderTopLeftRadius = 0;
                filterResultButton.style.borderTopRightRadius = 0;
                filterResultButton.style.borderBottomRightRadius = 0;
                filterResultButton.style.borderBottomLeftRadius = 0;
                filterResultButton.style.paddingTop = 3;
                filterResultButton.style.paddingBottom = 3;
                filterResultButton.style.flexGrow = 1;
                
                // Remove entry button
                var removeEntryButton = new Button();
                removeEntryButton.text = "X";
                removeEntryButton.tooltip = "Exclude from list.";
                removeEntryButton.style.flexShrink = 1;
                removeEntryButton.style.borderTopLeftRadius = 0;
                removeEntryButton.style.borderTopRightRadius = 0;
                removeEntryButton.style.borderBottomRightRadius = 0;
                removeEntryButton.style.borderBottomLeftRadius = 0;

                // Style VE and add buttons to VE
                resultHolder.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                resultHolder.Add(removeEntryButton);
                resultHolder.Add(filterResultButton);
                
                removeEntryButton.clickable.clicked += () => RemoveFilteredListEntryAsset(resultHolder);

                if (ProjectSearch.IsAssetValidForFilter(assetRelativePath, _filter))
                {
                    _filterResultsScrollView.Add(resultHolder);
                }
            }

            // If the list is empty, show 0 results message
            if (_filterResultsScrollView.childCount == 0)
            {
                var foundAsset = new Label();
                foundAsset.text = "0 results found";
                _filterResultsScrollView.Add(foundAsset);
            }
            
            EditorUtility.ClearProgressBar();
        }

        private void AddNewEntryToFoldout(Foldout foldout)
        {
            var newIncludeNameStartsWithVisualElement = new VisualElement();
            newIncludeNameStartsWithVisualElement.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            
            var newIncludeNameStartsWithTextField = new TextField();
            newIncludeNameStartsWithTextField.style.flexGrow = 1;
            
            var newIncludeNameStartsWithButton = new Button()
            {
                text = "X"
            };
            newIncludeNameStartsWithButton.style.borderTopLeftRadius = 0;
            newIncludeNameStartsWithButton.style.borderTopRightRadius = 0;
            newIncludeNameStartsWithButton.style.borderBottomRightRadius = 0;
            newIncludeNameStartsWithButton.style.borderBottomLeftRadius = 0;
            newIncludeNameStartsWithButton.clickable.clicked += () => 
                RemoveVisualElementFromFoldout(
                    newIncludeNameStartsWithVisualElement,
                    foldout);
            
            newIncludeNameStartsWithVisualElement.Add(newIncludeNameStartsWithButton);
            newIncludeNameStartsWithVisualElement.Add(newIncludeNameStartsWithTextField);
            
            foldout.Add(newIncludeNameStartsWithVisualElement);
        }
        
        private void RemoveVisualElementFromFoldout(VisualElement visualElement, Foldout foldout)
        {
            foldout.Remove(visualElement);
        }

        private void RemoveFilteredListEntryAsset(VisualElement entryVisualElement)
        {
            _filterResultsScrollView.Remove(entryVisualElement);
        }

        private void SelectCurrentFoundAsset(string assetPath)
        {
            var obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
            Selection.activeObject = obj;
        }

        private void ConfirmFilterList()
        {
            _isFilterTab = false;
            _isToolsTab = true;
            InitUi();
        }
        
        private void Rename()
        {
            
        }

        private void Move()
        {
            
        }
        
    }                                                                                                                                        
}
