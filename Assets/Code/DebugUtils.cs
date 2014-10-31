using System;
using UnityEngine;

public class DebugUtils 
{ 
	static public void Assert(bool condition, string message = "")
	{
#if UNITY_EDITOR
		if (!condition)
		{
			Debug.LogError(message);
			throw new Exception();
		}
#endif 
	} 
}