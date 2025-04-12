
using System.Collections.Generic;
using UnityEditor;
using IUtil.SO;
using UnityEngine;

namespace IUtil.ProjectWindow
{

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class FolderConfigLoader
    {
        public static Dictionary<string, FolderConfig> ConfigDict { get; private set; } = new();
        public static Dictionary<FolderColorType, Texture2D> ColoredFolders { get; private set; } = new();
        public static Dictionary<FolderIconType, Texture2D> Icons { get; private set; } = new();

        static FolderConfigLoader()
        {
            LoadAll();

            EditorApplication.projectChanged -= RefreshConfigs;
            EditorApplication.projectChanged -= LoadFolderColorTextures;
            EditorApplication.projectChanged -= LoadIconTextures;

            EditorApplication.projectChanged += RefreshConfigs;
            EditorApplication.projectChanged += LoadFolderColorTextures;
            EditorApplication.projectChanged += LoadIconTextures;
        }

        public static FolderConfig FindOrCreateFolderConfig(string folderPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:FolderConfig", new[] { "Assets/InspectorUtils/Data/FolderConfig" });

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<FolderConfig>(path);
                if (config.FolderPath == folderPath)
                    return config;
            }

            // 없으면 새로 생성
            var newConfig = ScriptableObject.CreateInstance<FolderConfig>();
            newConfig.FolderPath = folderPath;

            string name = System.IO.Path.GetFileName(folderPath);


            return null;
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

            string[] guids = AssetDatabase.FindAssets("t:FolderConfig", new[] { "Assets/InspectorUtils/Data/FolderConfig" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                FolderConfig config = AssetDatabase.LoadAssetAtPath<FolderConfig>(path);

                if (config != null && !string.IsNullOrEmpty(config.FolderPath))
                {
                    ConfigDict[config.FolderPath] = config;
                }
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
                string[] guids = AssetDatabase.FindAssets($"{fileName} t:Texture2D", new[] { "Assets/InspectorUtils/Sprites/Folders" });

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
        private static void LoadIconTextures()
        {
            Icons.Clear();

            foreach (FolderIconType iconType in System.Enum.GetValues(typeof(FolderIconType)))
            {
                if (iconType == FolderIconType.None)
                    continue;

                string fileName = iconType.ToString();
                string[] guids = AssetDatabase.FindAssets($"{fileName} t:Texture2D", new[] { "Assets/InspectorUtils/Sprites/Icons" });

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                    if (texture != null)
                    {
                        Icons[iconType] = texture;
                    }
                }
            }
        }
    }
#endif
}
