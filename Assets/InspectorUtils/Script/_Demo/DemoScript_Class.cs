using UnityEngine;

namespace IUtil._Demo
{
	[System.Serializable]
	public class NestedClass
	{
		public bool IsShow;
		[ShowIf(nameof(IsShow))] public string showString;
	}

    public class DemoScript_Class : MonoBehaviour
	{
		[SerializeField]
		private NestedClass nestedClass = new();


	}
}