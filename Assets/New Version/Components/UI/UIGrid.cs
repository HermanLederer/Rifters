using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGrids
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIGrid : MonoBehaviour
	{
		//
		// Editor properties
		[Header("Grid properties")]
		public int columns = 16;
		public int rows = 9;

		//
		// Private variables
		public RectTransform rectTransform;
		private float scaledColSize = 0f;
		private float scaledRowSize = 0f;
		private float unscaledColSize = 0f;
		private float unscaledRowSize = 0f;

		private void OnDrawGizmos()
		{
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			float width = rectTransform.rect.width * rectTransform.lossyScale.x;
			float height = rectTransform.rect.height * rectTransform.lossyScale.y;

			for (float x = 0; x < width; x += width / columns)
				Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, height, 0));

			for (float y = 0; y < height; y += height / rows)
				Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(width, y, 0));
		}

		private void RecalculateSizes()
		{
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			float width = rectTransform.rect.width;
			float height = rectTransform.rect.height;

			scaledColSize = (width * rectTransform.lossyScale.x) / columns;
			scaledRowSize = (height * rectTransform.lossyScale.y) / rows;

			unscaledColSize = width / columns;
			unscaledRowSize = height / rows;
		}

		//
		// Grid to world
		public Vector2 PositionGridToWorld(float col, float row)
		{
			RecalculateSizes();
			return new Vector2(col * scaledColSize, row * scaledRowSize);
		}

		public Vector2 SizeGridToWorld(float cols, float rows)
		{
			RecalculateSizes();
			return new Vector2(cols * unscaledColSize, rows * unscaledRowSize);
		}

		//
		// World to grid
		public Vector2 PositionWorldToGrid(float x, float y)
		{
			RecalculateSizes();

			x = Mathf.Round(x / scaledColSize);
			y = Mathf.Round(y / scaledRowSize);

			return new Vector2(x, y);
		}

		public Vector2 SizeWorldToGrid(float width, float height)
		{
			RecalculateSizes();

			width = Mathf.Round(width / unscaledColSize);
			height = Mathf.Round(height / unscaledRowSize);

			return new Vector2(width, height);
		}

		//
		// Quantization
		/*public Vector2 GetQuantizedPosition(float x, float y)
		{
			return PositionGridToWorld(x, y);
		}

		public Vector2 GetQuantizedSize(float width, float height)
		{
			return SizeGridToWorld(width, height);
		}*/
	}
}