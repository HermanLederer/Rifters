using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGrids
{
	[ExecuteInEditMode]
	public class UIGrid : MonoBehaviour
	{


		//
		// Editor variables
		public RectTransform rect;
		[Header("Grid properties")]
		public int columns = 16;
		public int rows = 9;

		void Start()
		{

		}

		void Update()
		{

		}

		private void OnDrawGizmosSelected()
		{
			float width = rect.rect.width * rect.lossyScale.x;
			float height = rect.rect.height * rect.lossyScale.y;

			for (float x = 0; x < width; x += width / columns)
				Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, height, 0));

			for (float y = 0; y < height; y += height / rows)
				Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(width, y, 0));
		}
	}
}