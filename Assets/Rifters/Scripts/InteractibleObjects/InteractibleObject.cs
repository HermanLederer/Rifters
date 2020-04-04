using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interactible Object", menuName = "Interactible Object")]
public class InteractibleObject : ScriptableObject
{
    public Material normalMat;
    public Material fresnelMat;
}
