using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredUIButton : ColoredUI
{
	public Button button;

	protected override void SetColor(Color color)
	{
		if (button != null)
		{
			ColorBlock cb = button.colors;
			cb.normalColor = color;
			cb.highlightedColor = color * 2;
			cb.pressedColor = color * 2;
			cb.selectedColor = color * 2;
			cb.disabledColor = color;
			button.colors = cb;
		}
	}
}
