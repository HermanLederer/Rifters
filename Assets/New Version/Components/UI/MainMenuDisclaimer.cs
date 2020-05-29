using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDisclaimer : MonoBehaviour
{
	public Text text = null;
	[TextArea]
	public string messgae = null;

    void Start()
    {
		text.text = messgae.Replace("VER", Application.version);
    }
}
