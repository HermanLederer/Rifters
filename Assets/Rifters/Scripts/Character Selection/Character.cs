using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public Sprite Image;

    public float ImageWidth;
    public float ImageHeight;

    public float ImageX;
    public float ImageY;

    public GameObject CharacterModel;

    public int id;

    public Animator animator;

    public bool locked;

    public List<string> animationTriggers;


}
