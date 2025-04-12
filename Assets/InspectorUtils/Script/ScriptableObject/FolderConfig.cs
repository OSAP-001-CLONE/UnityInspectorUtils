using UnityEngine;

namespace IUtil.SO
{
	[CreateAssetMenu(fileName = "FolderConfig", menuName = "IUtil/FolderConfig")]
	public class FolderConfig : ScriptableObject
	{
		public string FolderPath = "";
		public Color CustomFolderColor = Color.white;
		public FolderColorType ColorType = FolderColorType.None;
		public FolderIconType IconType = FolderIconType.None;
	}
}