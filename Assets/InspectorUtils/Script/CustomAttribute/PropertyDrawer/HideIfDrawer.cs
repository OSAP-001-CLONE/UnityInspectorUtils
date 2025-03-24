#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IUtil.CustomAttribute
{
	[CustomPropertyDrawer(typeof(HideIfAttribute), true)]
	public class HideIfDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if(!(attribute as HideIfAttribute).Condition)
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (attribute as HideIfAttribute).Condition ?
				0f : EditorGUI.GetPropertyHeight(property, label);
		}
	}
}
#endif