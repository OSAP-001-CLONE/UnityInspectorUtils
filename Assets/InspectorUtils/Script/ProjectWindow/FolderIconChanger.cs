using IUtil.SO;
using UnityEditor;
using UnityEngine;

namespace IUtil.ProjectWindow
{
#if UNITY_EDITOR
	[InitializeOnLoad]
	public class FolderIconChanger
	{
		private static Texture2D closedFolderIcon;
		static FolderIconChanger()
		{
			EditorApplication.projectWindowItemOnGUI += HandleProjectWindowItemOnGUI;
		}
		private static void HandleProjectWindowItemOnGUI(string guid, Rect rect)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			if (path == "Assets" || path == "Assets/") return;
			closedFolderIcon = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;

			Rect imageRect;
			Rect additionalRect;

			if (rect.height > 20)
			{
				imageRect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.width + 2);
			}
			else if (rect.x > 20)
			{
				imageRect = new Rect(rect.x - 1, rect.y - 1, rect.height + 2, rect.height + 2);
			}
			else
			{
				imageRect = new Rect(rect.x + 2, rect.y - 1, rect.height + 2, rect.height + 2);
			}

			additionalRect = new Rect(rect.x + imageRect.width * .4f, rect.y + imageRect.height * .4f, imageRect.width * .6f, imageRect.height * .6f);


			if (AssetDatabase.IsValidFolder(path))
			{
				if (!FolderConfigLoader.ConfigDict.TryGetValue(path, out FolderConfigElement config)) return;

				if (config.ColorType != FolderColorType.None && FolderConfigLoader.ColoredFolders != null)
				{
					if (config.ColorType == FolderColorType.Custom) GUI.color = config.CustomFolderColor;
					GUI.DrawTexture(imageRect, FolderConfigLoader.ColoredFolders[config.ColorType]);
					GUI.color = Color.white;
				}

				if (config.IconType != FolderIconType.None && FolderConfigLoader.Icons != null)
				{
					GUI.DrawTexture(additionalRect, FolderConfigLoader.Icons[config.IconType]);
				}
			}
		}
	}
#endif
}