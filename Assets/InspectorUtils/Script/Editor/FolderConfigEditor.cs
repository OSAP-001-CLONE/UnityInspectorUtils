using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using IUtil;
using IUtil.ProjectWindow;

public class FolderIconSelectorWindow : EditorWindow
{
    private string targetFolderPath;
    private Vector2 scroll;
    private Dictionary<FolderColorType, Texture2D> iconDict;

    public static void Open(string folderPath)
    {
        var window = GetWindow<FolderIconSelectorWindow>("Select Folder Icon");
        window.targetFolderPath = folderPath;
        window.LoadIcons();
        window.Show();
    }

    private void LoadIcons()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("폴더 아이콘 선택", EditorStyles.boldLabel);
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var kvp in iconDict)
        {
            if (kvp.Key == FolderColorType.None) continue;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(kvp.Key.ToString(), GUILayout.Height(32)))
            {
                ApplyIcon(kvp.Key);
                Close();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label(kvp.Value, GUILayout.Width(32), GUILayout.Height(32));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ApplyIcon(FolderColorType type)
    {
        var config = FolderConfigLoader.FindOrCreateFolderConfig(targetFolderPath);
        config.ColorType = type;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        Debug.Log($"{targetFolderPath}에 {type} 아이콘 적용됨.");
    }
}