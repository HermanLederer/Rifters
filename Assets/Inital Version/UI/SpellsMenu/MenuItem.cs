using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;
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
        description.SetActive(true);
    }

    public void Deselect()
    {
        background.color = baseColor;
        background.rectTransform.localScale = Vector3.one;
        description.SetActive(false);
    }
}
