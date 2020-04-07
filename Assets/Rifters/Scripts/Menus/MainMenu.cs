using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;

    public GameObject spellImage;
    public GameObject movementImage;

    public Animator mainAnimator;
    public Animator controlsAnimator;



    public void PlayGame()
    {
        SceneManager.LoadScene(2);
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

    public void OpenCharacterSelection()
    {
        mainAnimator.SetTrigger("Dissapear");
        Invoke("ChangeToCharacterSelection", 1f);
    }

    private void ChangeToCharacterSelection()
    {
        SceneManager.LoadScene("Character selection");
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

    public void ViewMovementControls()
    {
        movementImage.SetActive(true);
        spellImage.SetActive(false);
    }

    public void ViewSpellControls()
    {
        spellImage.SetActive(true);
        movementImage.SetActive(false);
    }
}
