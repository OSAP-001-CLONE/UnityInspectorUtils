using UnityEngine;

namespace IUtil._Demo
{
    public class DemoScript : MonoBehaviour
	{
		private int[] intOptions = new int[] { 1, 2, 3 };
		private float[] floatOptions = new float[] { 1.0f, 2.0f, 3.0f };
		private string[] stringOptions = new string[] { "Option1", "Option2", "Option3" };

		[PopupOption("intOptions")]
		public int m_selectInt;

		[PopupOption("floatOptions")]
		public float m_selectFloat;

		[PopupOption("stringOptions")]
		public string m_selectString;

		[SerializeField, HelpBox("Hello I'm helpbox.", IUtil.MessageType.Error)]
		private int test_int;

		[TabGroup("Main", "Tab1")]
		public int tab1Var;

		[FoldoutGroup("Settings")]
		public float setting1;

		[TabGroup("Sub1", "Tabb1")]
		public float sub1Float1;

		public float sub1Float2;

		[TabGroup("Sub1", "Tabb2")]
		public float sub1Float3;

		[TabGroup("Main", "Tab2")]
		public string tab2Var;

		public float m_SelectableFloat;


	}
}