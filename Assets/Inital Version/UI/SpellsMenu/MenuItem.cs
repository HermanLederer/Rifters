using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Color hoverColorIcon;
    public Color baseColorIcon;
    public Image background;
    public Image icon;
    public GameObject description;
    public float scaleFactor;
    public string spellName;

    // Start is called before the first frame update
    void Start()
    {
        background.color = baseColor;
    }

    public void Select()
    {
        background.color = hoverColor;
        background.rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        icon.color = hoverColorIcon;
        description.SetActive(true);
    }

    public void Deselect()
    {
        background.color = baseColor;
        background.rectTransform.localScale = Vector3.one;
        icon.color = baseColorIcon;
        description.SetActive(false);
    }
}
