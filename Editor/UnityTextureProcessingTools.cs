using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

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
        window.minSize = new Vector2(350, 550);
    }
    
    private void OnEnable()
    {
        // Reference to the root of the window.
        _root = rootVisualElement;
        
        // Associates a stylesheet to our root. Thanks to inheritance, all root’s
        // children will have access to it.
        _root.styleSheets.Add(Resources.Load<StyleSheet>("UnityTextureProcessingToolsEditorWindow"));

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var mainVisualTree = Resources.Load<VisualTreeAsset>("UnityTextureProcessingToolsEditorWindow");
        mainVisualTree.CloneTree(_root);
    }
}
