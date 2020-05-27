using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGrids
{
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UICell : MonoBehaviour
	{
		//
		// Other components
		[HideInInspector]
		public RectTransform rect;
		[HideInInspector]
		public UIGrid grid;

		//
		// Editor properties
		[Header("Cell properties")]
		public Vector2 pos = Vector2.zero;
		public Vector2 size = new Vector2(1f, 1f);

		public bool isFree = false;

		private void Start()
		{
			Reposition();
			Resize();
		}

		private void Update()
		{
			if (Application.isEditor)
			{
				if (!isFree)
				{
					Reposition();
					Resize();
				}
			}
		}

		//
		// Grid to world
		public void Reposition()
		{
			rect.position = grid.PositionGridToWorld(pos.x, pos.y);
		}

		public void Resize()
		{
			rect.sizeDelta = grid.SizeGridToWorld(size.x, size.y);
		}

		// World to grid
		public void SaveQuantizedPosition()
		{
			pos = grid.PositionWorldToGrid(rect.position.x, rect.position.y);
			
		}

		public void SaveQuantizedSize()
		{
			size = grid.SizeWorldToGrid(rect.sizeDelta.x, rect.sizeDelta.y);
		}
	}
}
