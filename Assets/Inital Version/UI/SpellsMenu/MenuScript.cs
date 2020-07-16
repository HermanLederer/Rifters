using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject rocks;
    public GameObject walls;

    public GameObject pauseMenu;

    private Vector2 normalisedMousePosition;
    private float currentAngle;
    private int selection;
    private int previousSelection;

    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject[] spellObjects;

    private MenuItem menuItemSc;
    private MenuItem previousMenuItemSc;

    [SerializeField] private Image currentSpell;

    [HideInInspector] public bool showing;
    [HideInInspector] public bool paused;

    // Start is called before the first frame update
    void Awake()
    {
        selection = 0;
        menuItemSc = menuItems[selection].GetComponent<MenuItem>();
        ChooseSpell();
        HideMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ShowMenu();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ChooseSpell();
            HideMenu();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (!showing)
            return;

        normalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

        currentAngle = (currentAngle + 360) % 360;

        selection = (int) currentAngle / 60;

        if(selection != previousSelection)
        {
            previousMenuItemSc = menuItems[previousSelection].GetComponent<MenuItem>();
            previousMenuItemSc.Deselect();
            previousSelection = selection;

            menuItemSc = menuItems[selection].GetComponent<MenuItem>();
            menuItemSc.Select();
        }
    }

    private void ShowMenu()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        showing = true;
    }

    private void HideMenu()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        showing = false;
    }

    private void ChooseSpell()
    {
        foreach (var item in spellObjects)
        {
            item.SetActive(false);
        }

        spellObjects[selection].SetActive(true);
        currentSpell.sprite = menuItemSc.icon.sprite;
        currentSpell.color = menuItemSc.hoverColor;

        bool activateRocks = selection == 2;
        bool activateWalls = selection == 3;

        rocks.SetActive(activateRocks);
        walls.SetActive(activateWalls);
    }
}
