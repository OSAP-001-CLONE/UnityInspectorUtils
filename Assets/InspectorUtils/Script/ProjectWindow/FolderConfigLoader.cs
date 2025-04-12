
using System.Collections.Generic;
using UnityEditor;
using IUtil.SO;
using UnityEngine;
using IUtil.Utils;

namespace IUtil.ProjectWindow
{

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class FolderConfigLoader
    {
        public static Dictionary<string, FolderConfigElement> ConfigDict { get; private set; } = new();
        public static Dictionary<FolderColorType, Texture2D> ColoredFolders { get; private set; } = new();
        public static Dictionary<FolderIconType, Texture2D> Icons { get; private set; } = new();

        static FolderConfigLoader()
        {
            LoadAll();

            EditorApplication.projectChanged -= LoadAll;

            EditorApplication.projectChanged += LoadAll;
        }

        public static void LoadAll()
        {
            RefreshConfigs();
            LoadFolderColorTextures();
            LoadIconTextures();
        }

        private static void RefreshConfigs()
        {
            ConfigDict.Clear();

            FolderConfig config = AssetDatabase.LoadAssetAtPath<FolderConfig>(Constants.PATH_FOLDER_CONFIG);

            for (int i = 0; i < config.Elements.Count; i++)
            {
                if (config != null && !string.IsNullOrEmpty(config.Elements[i].FolderPath))
                    ConfigDict[config.Elements[i].FolderPath] = config.Elements[i];
            }
        }

        private static void LoadFolderColorTextures()
        {
            ColoredFolders.Clear();

            foreach (FolderColorType colorType in System.Enum.GetValues(typeof(FolderColorType)))
            {
                if (colorType == FolderColorType.None)
                    continue;

                string fileName = colorType.ToString();
                string[] guids = AssetDatabase.FindAssets($"{fileName} t:Texture2D", new[] { Constants.PATH_FOLDER_TEXTURE });

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                    if (texture != null)
                    {
                        ColoredFolders[colorType] = texture;
                    }
                }
            }
        }

        public static void LoadIconTextures()
        {
            Icons.Clear();

            foreach (FolderIconType iconType in System.Enum.GetValues(typeof(FolderIconType)))
            {
                if (iconType == FolderIconType.None)
                    continue;
                    
                Icons[iconType] = EditorGUIUtility.IconContent(GetIconName(iconType)).image as Texture2D;
            }
        }

        private static string GetIconName(FolderIconType type)
        {
            switch (type)
            {
                case FolderIconType.Script:
                    return "cs Script Icon";
                case FolderIconType.Material:
                    return "d_Material Icon";
                case FolderIconType.Shader:
                    return "d_Shader Icon";
                case FolderIconType.Prefab:
                    return "Prefab Icon";
                case FolderIconType.ScriptableObject:
                    return "d_ScriptableObject Icon";
                case FolderIconType.Texture:
                    return "d_Texture Icon";
                case FolderIconType.Animator:
                    return "AnimatorController Icon";
                case FolderIconType.Audio:
                    return "AudioClip Icon";
                case FolderIconType.Font:
                    return "d_Font Icon";
                case FolderIconType.None:
                default:
                    return null;
            }
        }
    }
#endif
}
