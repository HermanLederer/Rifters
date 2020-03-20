using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;

    public Animator mainAnimator;
    public Animator controlsAnimator;



    public void PlayGame()
    {
        SceneManager.LoadScene("Prototype");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {
        mainAnimator.SetTrigger("Disappear");
        Invoke("ChangeToControls", 1f);
    }

    private void ChangeToControls()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void MenuFromControls()
    {
        controlsAnimator.SetTrigger("Disappear");
        Invoke("BackToMenuFromControls", 1f);
    }

    private void BackToMenuFromControls()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
}
