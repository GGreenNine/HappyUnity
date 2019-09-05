using System.Collections;
using System.Collections.Generic;
using UIEventDelegate;
using UnityEngine;

namespace UI.EventDelegate
{
	public class SimpleReorderableList 
	{
		
	}

	public class ReorderableList<T> : SimpleReorderableList
	{
		public List<T> List;
	}

	[System.Serializable]
	public class ReorderableEventList : ReorderableList<UIEventDelegate.EventDelegate>
	{
	}
}