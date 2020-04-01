using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayer : MonoBehaviour
{
	// Editor variables

	public Character character;

	public Image sprite;
	public Transform spawnPosition;

	public Button button;

	private int id;

	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		sprite.sprite = character.Image;

		RectTransform rect = sprite.GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(character.ImageWidth, character.ImageHeight);
		rect.localPosition = new Vector3(character.ImageX, character.ImageY, 0f);

		button.interactable = !character.locked;
	}

	void Update()
	{
		
	}

	//--------------------------
	// CharacterDisplayer events
	//--------------------------

	public void SetModel()
	{
		foreach (Transform child in spawnPosition)
		{
			Destroy(child.gameObject);
		}

		Quaternion direction = Quaternion.LookRotation(-Vector3.forward);
		GameObject characterModel = Instantiate(character.CharacterModel, spawnPosition.position, direction);
		characterModel.transform.SetParent(spawnPosition);
	}

}
