using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RuntimeVariableData<T> : ScriptableObject
{
	[NonSerialized]
	public T Value;
}
