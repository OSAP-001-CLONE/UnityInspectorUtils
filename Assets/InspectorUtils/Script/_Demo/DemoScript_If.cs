using System.Collections.Generic;
using UnityEngine;

namespace IUtil._Demo
{
	[System.Serializable]
	public class NestedClass
	{
		public bool IsShow;
		[ShowIf(nameof(IsShow))] public string showString;
	}
	public class DemoScript_If : MonoBehaviour
	{
		public bool isReadonly;
		[ReadOnlyIf(nameof(isReadonly))]
		public int readonlyValue;

		public bool isHide;
		[HideIf(nameof(isHide))]
		public float hideValue;

		public bool isShow;
		[ShowIf(nameof(isShow))]
		public float showValue;

		public List<NestedClass> nestedClass = new();
	}
}