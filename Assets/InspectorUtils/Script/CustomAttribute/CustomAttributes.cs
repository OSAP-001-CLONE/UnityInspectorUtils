using System;
using UnityEngine;

namespace IUtil
{
	#region Grouping

	/// <summary>
	/// Attribute class for [TabGroup]
	/// 
	/// - Usage
	///		[TabGroup(HeaderName, FontColor, TabColor)]
	///		public float m_float1;
	///		public float m_float2;
	///		...
	///		
	/// - Descript
	///		When a specific tab is selected, only the variables and functions included in that tab are displayed.
	///		It can contain [FoldoutGroup].
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class TabGroupAttribute : PropertyAttribute
	{
		public string GroupName { get; }
		public string TabName { get; }
		public string ParentGroup { get; set; }
		public string ParentTab { get; set; }

		public TabGroupAttribute(
			string groupName,
			string tabName,
			string parentGroup = null,
			string parentTab = null)
		{
			GroupName = groupName;
			TabName = tabName;
			ParentGroup = parentGroup;
			ParentTab = parentTab;
		}
	}

	/// <summary>
	/// Attribute class for [FoldoutGroup]
	/// 
	/// - Usage
	///		[FoldoutGroup(HeaderName, HeaderSize, HeaderColor)]
	///		public float m_float1;
	///		public float m_float2;
	///		...
	///		
	/// - Descript
	///		When the header is collapsed, the variables are hidden; when expanded, they become visible.
	///		It can be part of [TabGroup], but it cannot contain [TabGroup].
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class FoldoutGroupAttribute : PropertyAttribute
	{
		public string Name { get; }
		public int FontSize { get; }
		public ColorType ColorType { get; }

		public FoldoutGroupAttribute(string name) : this(name, 14, ColorType.White) { }
		public FoldoutGroupAttribute(string name, int fontSize) : this(name, fontSize, ColorType.White) { }

		public FoldoutGroupAttribute(string name, int fontSize, ColorType type)
		{
			Name = name;
			FontSize = fontSize;
			ColorType = type;
		}
	}

	#endregion


	#region Function Utility

	/// <summary>
	/// Attribute class for [Button]
	/// 
	/// - Usage
	///		[Button(DisplayName, TabName)]
	///		public void Function() { }
	///		
	/// - Descript
	///		You can display a button in the inspector to execute the corresponding function.
	///		You can include the function in a specific tab.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Method)]
	public class ButtonAttribute : PropertyAttribute
	{
		public string DisplayName { get; }
		public string TabName { get; }

		public ButtonAttribute(string displayName, string tabName)
		{
			DisplayName = displayName;
			TabName = tabName;
		}
	}

	#endregion


	#region Variable Utility
	
	/// <summary>
	/// Attribute class for [ShowIf]
	/// 
	/// - Usage
	///		[ShowIf(Condition)]
	///		public float m_float;
	///		
	/// - Descript
	///		It can hide or reveal variables based on a condition.
	///			true  : reveal
	///			false : hide
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class ShowIfAttribute : PropertyAttribute
	{
		public bool Condition { get; }

		public ShowIfAttribute(bool condition)
		{
			Condition = condition;
		}
	}

	/// <summary>
	/// Attribute class for [HideIf]
	/// 
	/// - Usage
	///		[HideIf(Condition)]
	///		public float m_float;
	///		
	/// - Descript
	///		It can hide or reveal variables based on a condition.
	///			true  : hide
	///			false : reveal
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class HideIfAttribute : PropertyAttribute
	{
		public bool Condition { get; }

		public HideIfAttribute(bool condition)
		{
			Condition = condition;
		}
	}

	/// <summary>
	/// Attribute class for [ReadOnlyIf]
	/// 
	/// - Usage
	///		[ReadOnlyIf(Condition)]
	///		public float m_float;
	///		
	/// - Descript
	///		It can readonly or default property based on a condition.
	///			true  : ReadOnly
	///			false : Default property
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class ReadOnlyIfAttribute : PropertyAttribute
	{
		public bool Condition { get; }

		public ReadOnlyIfAttribute(bool condition)
		{
			Condition = condition;
		}
	}

	/// <summary>
	/// Attribute class for [PopupOption]
	/// 
	/// - Usage
	///		[PopupOption(nameof(OptionArray, DefaultIndex))]
	///		public float m_float;
	///		
	/// - Descript
	///		You can set this variable using only the values from the OptionArray through the Popup.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class PopupOptionAttribute : PropertyAttribute
	{
		public string ArrayName { get; }
		public int DefaultIndex { get; }

		public PopupOptionAttribute(string arrayName) : this(arrayName, 0) { }

		public PopupOptionAttribute(string arrayName, int defaultIndex)
		{
			ArrayName = arrayName;
			DefaultIndex = defaultIndex;
		}
	}

	/// <summary>
	/// Attribute class for [HelpBox]
	/// 
	/// - Usage
	///		[HelpBox(Content, MessageType)]
	///		public float m_float;
	///		
	/// - Descript
	///		It displays helpbox below variable.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class HelpBoxAttribute : PropertyAttribute
	{
		public string Content { get; }
		public IUtil.MessageType MessageType { get; }

		public HelpBoxAttribute(string content) : this(content, IUtil.MessageType.None) { }

		public HelpBoxAttribute(string content, MessageType mType)
		{
			Content = content;
			MessageType = mType;
		}
	}

	/// <summary>
	/// Attribute class for [ReadOnly]
	/// 
	/// - Usage
	///		[ReadOnly(OnlyInRuntime)]
	///		public float m_float;
	///		
	/// - Descript
	///		It can prevent the variable's value from being changed in the editor inspector.
	///		If OnlyInRuntime is true, block value only in runtime.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Field)]
	public class ReadOnlyAttribute : PropertyAttribute
	{
		public bool OnlyInRuntime { get; }

		public ReadOnlyAttribute() : this(false) { }
		public ReadOnlyAttribute(bool onlyInRuntime)
		{
			OnlyInRuntime = onlyInRuntime;
		}
	}

	#endregion

}