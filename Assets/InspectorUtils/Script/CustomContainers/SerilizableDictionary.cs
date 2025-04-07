using System.Collections.Generic;
using System;
using UnityEngine;

namespace IUtil.CustomContainer
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField] private List<TKey> keys = new List<TKey>();
		[SerializeField] private List<TValue> values = new List<TValue>();

		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();

			foreach (var kvp in this)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			this.Clear();
			for (int i = 0; i < keys.Count; i++)
			{
				if (!this.ContainsKey(keys[i]))
					this.Add(keys[i], values[i]);
			}
		}
	}
}