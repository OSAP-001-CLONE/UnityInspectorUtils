#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IUtil;
using IUtil.Utils;

namespace InspectorUtils.Utils
{
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class MonoBehaviourEditor : Editor
	{
		private static Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
		private static Dictionary<string, string> activeTabs = new Dictionary<string, string>();
		private List<PropertyData> propertyDataList = new List<PropertyData>();
		private Stack<TabGroupContext> contextStack = new Stack<TabGroupContext>();
		private Dictionary<string, TabGroupInfo> tabGroups = new Dictionary<string, TabGroupInfo>();

		private class PropertyData
		{
			public SerializedProperty Property;
			public FoldoutGroupAttribute FoldoutAttr;
			public TabGroupAttribute TabAttr;
			public string EffectiveGroup;
			public string EffectiveTab;
		}

		private class TabGroupInfo
		{
			public List<string> Tabs = new List<string>();
			public string ParentGroup;
			public string ParentTab;
		}

		private class TabGroupContext
		{
			public string GroupName;
			public string TabName;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			CollectProperties();
			DrawProperties();
			serializedObject.ApplyModifiedProperties();
		}

		private void CollectProperties()
		{
			propertyDataList.Clear();
			tabGroups.Clear();
			contextStack.Clear();

			SerializedProperty prop = serializedObject.GetIterator();
			while (prop.NextVisible(true))
			{
				var tabAttr = GetTabGroupAttribute(prop);
				var foldoutAttr = GetFoldoutGroupAttribute(prop);

				// Context management
				if (tabAttr != null)
				{
					UpdateTabContext(tabAttr);
				}

				// Property data collection
				propertyDataList.Add(new PropertyData
				{
					Property = prop.Copy(),
					FoldoutAttr = foldoutAttr,
					TabAttr = tabAttr,
					EffectiveGroup = contextStack.Count > 0 ? contextStack.Peek().GroupName : null,
					EffectiveTab = contextStack.Count > 0 ? contextStack.Peek().TabName : null
				});

				// Tab group registration
				if (tabAttr != null)
				{
					RegisterTabGroup(tabAttr);
				}
			}
		}

		private void UpdateTabContext(TabGroupAttribute tabAttr)
		{
			// Automatically set parent if not specified
			if (string.IsNullOrEmpty(tabAttr.ParentGroup) && contextStack.Count > 0)
			{
				var parent = contextStack.Peek();
				tabAttr.ParentGroup = parent.GroupName;
				tabAttr.ParentTab = parent.TabName;
			}

			// Push new context to stack
			contextStack.Push(new TabGroupContext
			{
				GroupName = tabAttr.GroupName,
				TabName = tabAttr.TabName
			});
		}

		private void RegisterTabGroup(TabGroupAttribute tabAttr)
		{
			if (!tabGroups.ContainsKey(tabAttr.GroupName))
			{
				tabGroups[tabAttr.GroupName] = new TabGroupInfo
				{
					ParentGroup = tabAttr.ParentGroup,
					ParentTab = tabAttr.ParentTab
				};
			}

			if (!tabGroups[tabAttr.GroupName].Tabs.Contains(tabAttr.TabName))
			{
				tabGroups[tabAttr.GroupName].Tabs.Add(tabAttr.TabName);
			}
		}

		private void DrawProperties()
		{
			HashSet<string> processedGroups = new HashSet<string>();
			string activeFoldout = null;

			foreach (var data in propertyDataList)
			{
				// 1. Tab group visibility check
				if (data.TabAttr != null && !IsParentActive(data.TabAttr.GroupName))
					continue;

				// 2. Tab group toolbar drawing
				if (data.TabAttr != null && !processedGroups.Contains(data.TabAttr.GroupName))
				{
					DrawTabToolbar(data.TabAttr.GroupName);
					processedGroups.Add(data.TabAttr.GroupName);
				}

				// 3. Skip if not active tab
				if (data.TabAttr != null && !IsTabActive(data.TabAttr.GroupName, data.TabAttr.TabName))
					continue;

				// 4. Foldout group handling
				if (data.FoldoutAttr != null)
				{
					if (activeFoldout != data.FoldoutAttr.Name)
					{
						activeFoldout = data.FoldoutAttr.Name;
						DrawFoldoutGroup(data.FoldoutAttr);
					}
					if (!foldoutStates.ContainsKey(activeFoldout) || !foldoutStates[activeFoldout])
						continue;
				}
				else
				{
					activeFoldout = null;
				}

				// 5. Property visibility check
				if (IsPropertyVisible(data))
				{
					EditorGUILayout.PropertyField(data.Property, true);
				}
			}
		}

		private bool IsParentActive(string groupName)
		{
			if (!tabGroups.TryGetValue(groupName, out var groupInfo))
				return true;

			if (string.IsNullOrEmpty(groupInfo.ParentGroup))
				return true;

			return IsTabActive(groupInfo.ParentGroup, groupInfo.ParentTab);
		}

		private bool IsPropertyVisible(PropertyData data)
		{
			// Check effective tab context
			if (!string.IsNullOrEmpty(data.EffectiveGroup))
			{
				if (!IsTabActive(data.EffectiveGroup, data.EffectiveTab))
					return false;
			}

			// Check direct tab group
			if (data.TabAttr != null && !IsTabActive(data.TabAttr.GroupName, data.TabAttr.TabName))
				return false;

			return true;
		}

		private void DrawTabToolbar(string groupName)
		{
			if (!tabGroups.TryGetValue(groupName, out var groupInfo)) return;

			if (!activeTabs.ContainsKey(groupName))
				activeTabs[groupName] = groupInfo.Tabs.FirstOrDefault();

			GUILayout.BeginHorizontal();
			foreach (string tab in groupInfo.Tabs)
			{
				bool isActive = activeTabs[groupName] == tab;
				if (GUILayout.Toggle(isActive, tab, EditorStyles.toolbarButton))
				{
					if (!isActive) activeTabs[groupName] = tab;
				}
			}
			GUILayout.EndHorizontal();
		}

		private bool IsTabActive(string group, string tab)
		{
			return activeTabs.TryGetValue(group, out string activeTab) && activeTab == tab;
		}

		private FoldoutGroupAttribute GetFoldoutGroupAttribute(SerializedProperty prop)
		{
			var targetObj = prop.serializedObject.targetObject;
			var field = targetObj.GetType().GetField(prop.propertyPath,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return field?.GetCustomAttribute<FoldoutGroupAttribute>();
		}

		private TabGroupAttribute GetTabGroupAttribute(SerializedProperty prop)
		{
			var targetObj = prop.serializedObject.targetObject;
			var field = targetObj.GetType().GetField(prop.propertyPath,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return field?.GetCustomAttribute<TabGroupAttribute>();
		}

		private void DrawFoldoutGroup(FoldoutGroupAttribute attr)
		{
			if (!foldoutStates.ContainsKey(attr.Name))
				foldoutStates[attr.Name] = false;

			GUIStyleState onState = new GUIStyleState
			{
				textColor = Constants.PALLETE[(int)attr.ColorType]
			};

			GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
			{
				fontSize = attr.FontSize,
				fontStyle = FontStyle.Bold,
				normal = { textColor = Constants.PALLETE[(int)attr.ColorType] },
				alignment = TextAnchor.MiddleLeft,
				onFocused = onState,
				onNormal = onState
			};

			Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, attr.FontSize * 1.5f);
			foldoutStates[attr.Name] = EditorGUI.Foldout(rect, foldoutStates[attr.Name], attr.Name, true, foldoutStyle);
		}
	}
}
#endif