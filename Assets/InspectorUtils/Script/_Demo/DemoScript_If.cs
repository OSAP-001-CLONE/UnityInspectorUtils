using UnityEngine;

namespace IUtil._Demo
{
    public class DemoScript_If : MonoBehaviour
	{
		[SerializeField]
		private bool isReadonly;

		[SerializeField, ReadOnlyIf(nameof(isReadonly))]
		private int readonlyValue;


		[Space(10)]
		[SerializeField]
		private bool isHide;

		[SerializeField, HideIf(nameof(isHide))]
		private float hideValue;


		[Space(10)]
		[SerializeField]
		private bool isShow;

		[SerializeField, ShowIf(nameof(isShow))]
		private float showValue;

	}
}