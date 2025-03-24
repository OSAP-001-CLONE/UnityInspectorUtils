#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using IUtil;

[CustomPropertyDrawer(typeof(PopupOptionAttribute))]
public class PopupOptionDrawer : PropertyDrawer
{
	private const string TYPE_ERROR = "[PopupOption] �迭 Ÿ���� ȣȯ���� �ʽ��ϴ�!";

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var popupAttribute = attribute as PopupOptionAttribute;
		if (popupAttribute == null) return;

		// ���÷������� �迭 ������ ��������
		System.Array valueArray = GetArray(property, popupAttribute.ArrayName);
		if (valueArray == null)
		{
			EditorGUI.PropertyField(position, property, label);
			return;
		}

		// ���� ���� ��ġ�ϴ� �ε��� ã��
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

		// �˾� �ɼ� ����
		string[] options = new string[valueArray.Length];
		for (int i = 0; i < valueArray.Length; i++)
			options[i] = valueArray.GetValue(i).ToString();

		// Enum ��Ÿ�� �˾� �׸���

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
			Debug.LogError($"[PopupOption] �ʵ带 ã�� �� �����ϴ�: {fieldName}");
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