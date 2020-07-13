using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private Vector2 normalisedMousePosition;
    private float currentAngle;
    private int selection;
    private int previousSelection;

    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject[] spellObjects;

    private MenuItem menuItemSc;
    private MenuItem previousMenuItemSc;

    [SerializeField] private Text currentSpell;

    [HideInInspector] public bool showing;

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
        currentSpell.text = menuItemSc.spellName;
        currentSpell.color = menuItemSc.hoverColor;

        /*
        switch (selectedSpell)
        {
            case 0:
                Debug.Log("Has elegido Geyser");
                spellObjects[0].SetActive(true);
                break;
            case 1:
                Debug.Log("Has elegido Push");
                spellObjects[1].SetActive(true);
                break;
            case 2:
                Debug.Log("Has elegido Pull Rocks");
                spellObjects[2].SetActive(true);
                break;
            case 3:
                Debug.Log("Has elegido Grow Wall");
                spellObjects[3].SetActive(true);
                break;
            case 4:
                Debug.Log("Has elegido Proyectiles");
                spellObjects[4].SetActive(true);
                break;
            default:
                Debug.Log("Has elegido Roots");
                spellObjects[5].SetActive(true);
                break;
        }*/
    }
}
