using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    [SerializeField] private AudioClip JumpOnGrass;
    [SerializeField] private AudioClip JumpOnRocks;

    [SerializeField] private AudioClip WalkOnGrass;
    [SerializeField] private AudioClip WalkOnRocks;



    private void Update()
    {
<<<<<<< Updated upstream
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AudioManager.Instance.PlaySfx(SFX, 1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AudioManager.Instance.PlayMusic(music1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            AudioManager.Instance.PlayMusic(music2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            AudioManager.Instance.PlayMusicWithFade(music1);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            AudioManager.Instance.PlayMusicWithFade(music2);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            AudioManager.Instance.PlayMusicWithCrossFade(music1, 3.0f);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            AudioManager.Instance.PlayMusicWithCrossFade(music2, 3.0f);
=======
        if (Input.GetKeyDown(KeyCode.Space))
            AudioManager.Instance.PlayJump(JumpOnGrass);

        if (Input.GetKeyDown(KeyCode.W))
            AudioManager.Instance.PlayMusic(WalkOnGrass);

        if (Input.GetKeyUp(KeyCode.W))
            AudioManager.Instance.StopMusic(WalkOnGrass);
>>>>>>> Stashed changes
    }
}
