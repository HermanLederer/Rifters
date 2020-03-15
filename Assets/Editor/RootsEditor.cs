using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Roots))]
public class RootsEditor : Editor
{
    private void OnSceneGUI()
    {
        Roots roots = (Roots)target;

        Handles.color = Color.red;

        Handles.DrawWireArc(roots.transform.position, Vector3.up, Vector3.forward, 360f, roots.radius);
    }
}
