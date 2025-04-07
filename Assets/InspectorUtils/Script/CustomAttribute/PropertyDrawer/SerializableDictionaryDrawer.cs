#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using IUtil.CustomContainer;

namespace IUtil.CustomAttribute
{
	[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
	public class SerializableDictionaryDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty keysProperty = property.FindPropertyRelative("keys");
			SerializedProperty valuesProperty = property.FindPropertyRelative("values");

			if (keysProperty == null || valuesProperty == null)
			{
				EditorGUI.LabelField(position, "Dictionary Serialization Error");
				return;
			}

			// UI 확장 버튼
			property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
													property.isExpanded, label);
			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				for (int i = 0; i < keysProperty.arraySize; i++)
				{
					Rect keyRect = new Rect(position.x, position.y + (i + 1) * 20, position.width * 0.4f, 18);
					Rect valueRect = new Rect(position.x + position.width * 0.4f + 5, position.y + (i + 1) * 20, position.width * 0.6f - 10, 18);

					SerializedProperty keyElement = keysProperty.GetArrayElementAtIndex(i);
					SerializedProperty valueElement = valuesProperty.GetArrayElementAtIndex(i);

					EditorGUI.PropertyField(keyRect, keyElement, GUIContent.none);
					EditorGUI.PropertyField(valueRect, valueElement, GUIContent.none);
				}

				// 추가 버튼
				Rect addButtonRect = new Rect(position.x, position.y + (keysProperty.arraySize + 1) * 20, position.width, 18);
				if (GUI.Button(addButtonRect, "Add Entry"))
				{
					keysProperty.arraySize++;
					valuesProperty.arraySize++;
				}

				// 삭제 버튼
				Rect removeButtonRect = new Rect(position.x, position.y + (keysProperty.arraySize + 2) * 20, position.width, 18);
				if (GUI.Button(removeButtonRect, "Remove Last Entry") && keysProperty.arraySize > 0)
				{
					keysProperty.arraySize--;
					valuesProperty.arraySize--;
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty keysProperty = property.FindPropertyRelative("keys");
			return (property.isExpanded ? (keysProperty.arraySize + 3) * 20 : 20);
		}
	}
}
#endif