using UnityEngine;
using System;

public class RuntimeVariableData<T> : ScriptableObject
{
	[NonSerialized]
	public T Value;
}
