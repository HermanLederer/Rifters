using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public abstract class ColoredUI : MonoBehaviour
{
	public enum UIColor
	{
		Dark,
		White,
		Yellow,
		Red
	}

	public UIColor color = UIColor.White;

	private void Start()
	{
		UpdateColor();
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			UpdateColor();
		}
	}

	private void UpdateColor()
	{
		switch (color)
		{
			case UIColor.White:
				SetColor(new Color(1f, 1f, 1f, 1f));
				break;
			case UIColor.Yellow:
				SetColor(new Color(1f, 238f / 255f, 23f / 255f, 1f));
				break;
			case UIColor.Red:
				SetColor(new Color(212f / 255f, 40f / 255f, 66f / 255f, 1));
				break;
			default:
				SetColor(new Color(12f / 255f, 15f / 255f, 20f / 255f, 1));
				break;
		}
	}

	protected abstract void SetColor(Color color);
}
