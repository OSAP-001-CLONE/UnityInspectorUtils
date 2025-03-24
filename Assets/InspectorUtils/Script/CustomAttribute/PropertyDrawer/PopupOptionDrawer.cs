#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using IUtil;

[CustomPropertyDrawer(typeof(PopupOptionAttribute))]
public class PopupOptionDrawer : PropertyDrawer
{
	private const string TYPE_ERROR = "[PopupOption] 배열 타입이 호환되지 않습니다!";

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var popupAttribute = attribute as PopupOptionAttribute;
		if (popupAttribute == null) return;

		// 리플렉션으로 배열 데이터 가져오기
		System.Array valueArray = GetArray(property, popupAttribute.ArrayName);
		if (valueArray == null)
		{
			EditorGUI.PropertyField(position, property, label);
			return;
		}

		// 현재 값과 일치하는 인덱스 찾기
		int selectedIndex = -1;
		object currentValue = GetPropertyValue(property);
		for (int i = 0; i < valueArray.Length; i++)
		{
			if (valueArray.GetValue(i).Equals(currentValue))
			{
				selectedIndex = i;
				break;
			}
		}

		// 팝업 옵션 생성
		string[] options = new string[valueArray.Length];
		for (int i = 0; i < valueArray.Length; i++)
			options[i] = valueArray.GetValue(i).ToString();

		// Enum 스타일 팝업 그리기

		EditorGUI.BeginChangeCheck();
		selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);
		if (EditorGUI.EndChangeCheck() && selectedIndex >= 0)
		{
			SetPropertyValue(property, valueArray.GetValue(selectedIndex));
		}
	}

	private System.Array GetArray(SerializedProperty property, string fieldName)
	{
		object targetObject = property.serializedObject.targetObject;
		FieldInfo fieldInfo = targetObject.GetType().GetField(
			fieldName,
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
		);

		if (fieldInfo == null || !fieldInfo.FieldType.IsArray)
		{
			Debug.LogError($"[PopupOption] 필드를 찾을 수 없습니다: {fieldName}");
			return null;
		}

		return fieldInfo.GetValue(targetObject) as System.Array;
	}

	private object GetPropertyValue(SerializedProperty property)
	{
		switch (property.propertyType)
		{
			case SerializedPropertyType.Integer: return property.intValue;
			case SerializedPropertyType.Float: return property.floatValue;
			case SerializedPropertyType.String: return property.stringValue;
			default:
				Debug.LogError(TYPE_ERROR);
				return null;
		}
	}

	private void SetPropertyValue(SerializedProperty property, object value)
	{
		switch (property.propertyType)
		{
			case SerializedPropertyType.Integer:
				property.intValue = (int)value;
				break;
			case SerializedPropertyType.Float:
				property.floatValue = (float)value;
				break;
			case SerializedPropertyType.String:
				property.stringValue = (string)value;
				break;
			default:
				Debug.LogError(TYPE_ERROR);
				break;
		}
	}
}
#endif