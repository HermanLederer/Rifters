using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredUIText : ColoredUI
{
	public Text text;

	protected override void SetColor(Color color)
	{
		if (text != null) text.color = color;
	}
}
