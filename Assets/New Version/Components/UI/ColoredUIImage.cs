using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredUIImage : ColoredUI
{
	public Image image;

	protected override void SetColor(Color color)
	{
		if (image != null) image.color = color;
	}
}
