#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IUtil.CustomAttribute
{
	[CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
	public class ShowIfDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if((attribute as ShowIfAttribute).Condition)
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (attribute as ShowIfAttribute).Condition ?
				EditorGUI.GetPropertyHeight(property, label) : 0f;
		}
	}
}
#endif