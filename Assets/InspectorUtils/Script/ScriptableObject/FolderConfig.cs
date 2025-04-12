using UnityEngine;
using System.Collections.Generic;

namespace IUtil.SO
{
	[System.Serializable]
	public class FolderConfigElement
	{
		public string FolderPath = "";
		public FolderColorType ColorType = FolderColorType.None;
		public FolderIconType IconType = FolderIconType.None;
		public Color CustomFolderColor = Color.white;
	}

	[CreateAssetMenu(fileName = "FolderConfig", menuName = "IUtil/FolderConfig")]
	public class FolderConfig : ScriptableObject
	{
		public List<FolderConfigElement> Elements = new();
	}
}