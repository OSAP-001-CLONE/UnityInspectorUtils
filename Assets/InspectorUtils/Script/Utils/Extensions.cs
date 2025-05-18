using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IUtil.Utils
{
#if UNITY_EDITOR
	public static class Extensions
	{
		public static System.Array GetArray(this SerializedProperty property, string fieldName)
		{
			object targetObject = property.serializedObject.targetObject;
			FieldInfo fieldInfo = targetObject.GetType().GetField(
				fieldName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
			);

			if (fieldInfo == null || !fieldInfo.FieldType.IsArray)
			{
				IUtilDebug.NoFieldError("PopupOption", fieldName);
				return null;
			}

			return fieldInfo.GetValue(targetObject) as System.Array;
		}

        public static bool GetBoolean(this SerializedProperty property, string attr, string fieldName)
        {
            object parentObject = GetTargetObjectWithProperty(property);

            if (parentObject == null)
            {
                IUtilDebug.NoFieldError(attr, fieldName + "'s parent");
                return false;
            }

            var fieldInfo = parentObject.GetType().GetField(
                fieldName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (fieldInfo == null)
            {
                IUtilDebug.NoFieldError(attr, fieldName);
                return false;
            }

            if (fieldInfo.FieldType != typeof(bool))
            {
                IUtilDebug.TypeError(attr, fieldInfo.FieldType.ToString());
                return false;
            }

            return (bool)fieldInfo.GetValue(parentObject);
        }

        /// <summary>
        /// Find targetObject that contains serializedProperty.
        /// </summary>
        public static object GetTargetObjectWithProperty(SerializedProperty prop)
        {
            string path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            string[] elements = path.Split('.')[..^1];

            // follow paths to track property in same object
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetFieldValue(obj, elementName, index);
                }
                else
                {
                    obj = GetFieldValue(obj, element);
                }

                if (obj == null)
                    return null;
            }

            return obj;
        }

        private static object GetFieldValue(object source, string name)
        {
            if (source == null)
                return null;

            var type = source.GetType();
            var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                return null;
            }

            return field.GetValue(source);
        }

        private static object GetFieldValue(object source, string name, int index)
        {
            var enumerable = GetFieldValue(source, name) as System.Collections.IEnumerable;
            if (enumerable == null)
                return null;

            var enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                    return null;
            }

            return enumerator.Current;
        }

        public static string GetIconName(this FolderIconType type)
		{
			switch (type)
			{
				case FolderIconType.Script:
					return "cs Script Icon";
				case FolderIconType.Material:
					return "d_Material Icon";
				case FolderIconType.Shader:
					return "d_Shader Icon";
				case FolderIconType.Prefab:
					return "Prefab Icon";
				case FolderIconType.ScriptableObject:
					return "d_ScriptableObject Icon";
				case FolderIconType.Texture:
					return "d_Texture Icon";
				case FolderIconType.Animator:
					return "AnimatorController Icon";
				case FolderIconType.Audio:
					return "AudioClip Icon";
				case FolderIconType.Font:
					return "d_Font Icon";
				case FolderIconType.None:
				default:
					return null;
			}
		}

	}
#endif
}
