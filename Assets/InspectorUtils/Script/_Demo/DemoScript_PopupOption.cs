using UnityEngine;

namespace IUtil._Demo
{
    public class DemoScript_PopupOption : MonoBehaviour
	{
		public int[] intOptions = new int[] { 1, 2, 3 };
		public float[] floatOptions = new float[] { 5.1f, 6.3f, 7.3f };
		public string[] stringOptions = new string[] { "Option1", "Option2", "Option3" };

		[SerializeField, PopupOption(nameof(intOptions))]
		private int selectInt;

		[SerializeField, PopupOption(nameof(floatOptions), 1)]
		public float selectFloat;

		[SerializeField, PopupOption(nameof(stringOptions), 2)]
		public string selectString;
	}
}