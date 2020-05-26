using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGrids
{
	[RequireComponent(typeof(RectTransform))]
	public class UICell : MonoBehaviour
	{
		//
		// Other components
		[HideInInspector]
		public RectTransform rect;
		[HideInInspector]
		public UIGrid grid;

		public void quantizePosition()
		{
			rect.position = grid.GetQuantizedPosition(rect.position.x, rect.position.y);
		}

		public void quantizeSize()
		{
			rect.sizeDelta = grid.GetQuantizedSize(rect.sizeDelta.x, rect.sizeDelta.y);
		}
	}
}
