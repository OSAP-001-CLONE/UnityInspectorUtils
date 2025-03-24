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
        #region Internal Classes 
        private class PropertyData
        {
            public SerializedProperty Property;
            public EditorAttribute Attribute;
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
        #endregion

        #region Local Variables
        /** Every variables to draw in monobehavioru editor. **/
        private List<PropertyData> propertyDataList = new List<PropertyData>();

        /** Dictionary to save tab/foldout states. **/
        private static Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>();
        private static Dictionary<string, string> activeTabs = new Dictionary<string, string>();
        
        /** Variables to trace properties' parent group to decide whether draw or not. **/
        private Stack<TabGroupContext> contextStack = new Stack<TabGroupContext>();
        private Dictionary<string, TabGroupInfo> tabGroups = new Dictionary<string, TabGroupInfo>();
        private Dictionary<string, TabGroupContext> foldoutGroups = new Dictionary<string, TabGroupContext>();
        
        private string currentFoldout = null;

        #endregion

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CollectProperties();
            DrawProperties();
            serializedObject.ApplyModifiedProperties();
        }

        private void Init()
        {
            propertyDataList.Clear();
            tabGroups.Clear();
            contextStack.Clear();
            currentFoldout = null;
        }

        private void CollectProperties()
        {
            Init();

            SerializedProperty prop = serializedObject.GetIterator();
            while (prop.NextVisible(true))
            {
                var tabAttr = GetTabGroupAttribute(prop);
                var foldAttr = GetFoldoutGroupAttribute(prop);

                if (tabAttr != null)
                {
                    UpdateTabContext(tabAttr);
                    currentFoldout = null;
                }

                if (foldAttr != null)
                {
                    UpdateFoldoutContext(foldAttr);
                }

                // Property data collection
                propertyDataList.Add(new PropertyData
                {
                    Property = prop.Copy(),
                    Attribute = foldAttr == null ? tabAttr : foldAttr
                });

                RegisterGroup(tabAttr);
                RegisterGroup(foldAttr);
            }
        }

        private void UpdateTabContext(TabGroupAttribute tabAttr)
        {
            // No Parent && Stack exist -> set as parent.
            if (string.IsNullOrEmpty(tabAttr.ParentGroup) && contextStack.Count > 0)
            {
                var parent = contextStack.Peek();
                tabAttr.ParentGroup = parent.GroupName;
                tabAttr.ParentTab = parent.TabName;
            }

            contextStack.Push(new TabGroupContext
            {
                GroupName = tabAttr.GroupName,
                TabName = tabAttr.TabName
            });
        }

        private void UpdateFoldoutContext(FoldoutGroupAttribute foldAttr)
        {
            currentFoldout = foldAttr.Name;


            if (contextStack.Count > 0)
            {
                var parent = contextStack.Peek();
                foldAttr.ParentGroup = parent.GroupName;
                foldAttr.ParentTab = parent.TabName;
            }
        }
        
        private void RegisterGroup(TabGroupAttribute tabAttr)
        {
            if (tabAttr == null) return;
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

        private void RegisterGroup(FoldoutGroupAttribute foldAttr)
        {
            if (foldAttr == null) return;

            if (!foldoutGroups.ContainsKey(foldAttr.Name))
            {
                foldoutGroups[foldAttr.Name] = new TabGroupContext
                {
                    GroupName = foldAttr.ParentGroup,
                    TabName = foldAttr.ParentTab
                };
            }
        }

        private void DrawProperties()
        {
            HashSet<string> processedGroups = new HashSet<string>();
            string currentFoldout = null;
            bool isFoldoutOpen = false;

            foreach (var data in propertyDataList)
            {
                // 1. Check tab group or foldout group to be revealed (or hidden)
                if (IsAbleToDrawGroup(data.Attribute)) continue;

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
                    // Close the previous foldout group
                    if (currentFoldout != null && currentFoldout != data.FoldoutAttr.Name)
                    {
                        isFoldoutOpen = false;
                    }

                    // Start a new foldout group
                    currentFoldout = data.FoldoutAttr.Name;
                    isFoldoutOpen = DrawFoldoutGroup(data.FoldoutAttr);
                }

                // 5. Skip properties if the foldout is closed
                if (currentFoldout != null && !isFoldoutOpen)
                    continue;

                // 6. Property visibility check
                if (IsPropertyVisible(data))
                {
                    EditorGUILayout.PropertyField(data.Property, true);
                }
            }
        }

        private bool IsAbleToDrawGroup(EditorAttribute attr)
        {
            if (attr == null) return true;


            // TODO - refer below function

        }

        // TODO - consider foldout also
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

        private bool DrawFoldoutGroup(FoldoutGroupAttribute attr)
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
            return foldoutStates[attr.Name];
        }
    }
}
#endif