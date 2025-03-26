using UnityEngine;

namespace IUtil.Utils
{
	public static class IUtilDebug
	{
		public static void NoFieldError(string attr, string fieldName)
		{
			Debug.LogError($"IUtil Error : [{attr}] can't find field: {fieldName}");
		}

		public static void TypeError(string attr, string wrongType)
		{
			Debug.LogError($"IUtil Error : [{attr}] can't matching type: {wrongType}");
		}
	}
}
