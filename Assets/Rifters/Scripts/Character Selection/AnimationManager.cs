using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    public Character character;

    private Animator anim;
    /*private AnimatorClipInfo[] currentClip;
    private AnimationClip[] characterClips;

    private int clipIndex = -1;

    private float timeToNextTrigger = -1;
    private float idleTime;
    */

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        /*currentClip = anim.GetCurrentAnimatorClipInfo(0);
        characterClips = anim.runtimeAnimatorController.animationClips;
        idleTime = FindCurrentClipDuration();*/
    }

    /*private float FindCurrentClipDuration()
    {
        for( int i = 0; i < characterClips.Length; i++)
        {
            if(characterClips[i] == currentClip[0].clip)
            {
                return currentClip[0].clip.length;
            }
        }

        Debug.Log("No ha funcionado");
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        currentClip = anim.GetCurrentAnimatorClipInfo(0);

        if (timeToNextTrigger >= 0)
        {
            timeToNextTrigger -= Time.deltaTime;
        }
        else
        {
            if (currentClip[0].clip.name == "Idle")
            {
                TriggerNextAnimation();
            }
        }

        
        
    }*/

    public void TriggerNextAnimation()
    {
        int nextTrigger = Random.Range(0, character.animationTriggers.Count);

        anim.SetTrigger(character.animationTriggers[nextTrigger]);
    }
}
