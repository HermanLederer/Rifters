using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicGrass
{
	[ExecuteInEditMode]
	public class DynamicGrassPopulator : MonoBehaviour
	{
		//
		// Other components

		//
		// Editor varaibles
		[Header("Grass")]
		[SerializeField] private Mesh grassMesh;
		[SerializeField] private Material grassMaterial;
		[SerializeField] private int grassAmount = 0;
		[SerializeField] [Range(0, 180)] private float slopeThreshold = 45;
		[Header("Region")]
		[SerializeField] private Vector3 boxSize;

		//--------------------------
		// MonoBehaviour methods
		//--------------------------
		private void Awake()
		{
			
		}

		private void Update()
		{
			Populate();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position, boxSize);
		}

		//--------------------------
		// DynamicGrassPopulator methods
		//--------------------------
		public void Populate()
		{
			Random.InitState((int) (transform.position.x + transform.position.z));
			List<Matrix4x4> matrixes = new List<Matrix4x4>(grassAmount);

			for (int i = 0; i < grassAmount; ++i)
			{
				Vector3 origin = transform.position;

				origin.x += boxSize.x * Random.Range(-0.5f, 0.5f);
				origin.z += boxSize.z * Random.Range(-0.5f, 0.5f);
				origin.y += boxSize.y / 2;

				Ray ray = new Ray(origin, Vector3.down);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, boxSize.y))
				{
					DynamicGrassSurface surface = hit.transform.gameObject.GetComponent<DynamicGrassSurface>();
					if (surface != null)
					{
						if (Vector3.Angle(hit.normal, Vector3.up) <= slopeThreshold)
						{
							origin = hit.point;
							matrixes.Add(Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one));
						}
					}
				}
			}
			Graphics.DrawMeshInstanced(grassMesh, 0, grassMaterial, matrixes);
		}
	}
}