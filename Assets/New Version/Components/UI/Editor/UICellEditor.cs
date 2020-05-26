using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIGrids
{
	[CustomEditor(typeof(UICell))]
	public class UICellEditor : Editor
	{
		public void OnSceneGUI()
		{
			UICell cell = (UICell)target;

			// checking for components
			if (cell.rect == null)
				cell.rect = cell.GetComponent<RectTransform>();

			if (cell.grid == null)
			{
				cell.grid = cell.GetComponentInParent<UIGrid>();
				if (cell.grid == null)
				{
					Debug.LogError(cell.gameObject.name + " is not parented to any UIGrid. Consider adding a UIGrid to the canvas (Sorry for spam)");
					return;
				}
			}

			if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				cell.quantizePosition();
				cell.quantizeSize();
			}
		}
	}
}
