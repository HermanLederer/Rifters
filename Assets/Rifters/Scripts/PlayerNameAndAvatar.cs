using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameAndAvatar : MonoBehaviour
{
    [Header("Player Input UI")]
    [SerializeField] private GameObject playerInputUI = null;
    [SerializeField] private GameObject mainMenuUI = null;
    [SerializeField] private InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    [Header("Player Layout UI")]
    [SerializeField] private Image avatarImage = null;
    [SerializeField] private Text nameText = null;

    public static string DisplayName { get; private set; }
    public static int AvatarInt { get; private set; } = 0;

    public static Color[] AvatarColors = { Color.white, new Color(1, .9f, .4f), new Color(1, .4f, .3f), new Color(.95f, .4f, .9f), new Color(.5f, 1, .75f) };

    private const string PlayerPrefsNameKey = "PlayerName";
    private const string PlayerPrefsAvatarKey = "PlayerAvatar";

    // Start is called before the first frame update
    void Start()
    {
        SetInputField();

        if (CheckHasNameAndAvatar())
        {
            SavePlayerPreferences();
            mainMenuUI.SetActive(true);
            playerInputUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SetInputField();
    }

    public void SetInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; } //Checking if there is already a name for this build

        string storedName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = storedName;

        SetPlayerUsername(storedName);
    }

    public bool CheckHasNameAndAvatar()
    {
        return PlayerPrefs.HasKey(PlayerPrefsAvatarKey) && PlayerPrefs.HasKey(PlayerPrefsNameKey);
    }

    public void SetPlayerUsername(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name) && name.Length < 14;
    }

    public void SetAvatarInt(int value)
    {
        AvatarInt = value;
    }

    public void SavePlayerPreferences()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        PlayerPrefs.SetInt(PlayerPrefsAvatarKey, AvatarInt);

        SetUserLayoutValues();
    }

    public void SetUserLayoutValues()
    {
        avatarImage.color = AvatarColors[PlayerPrefs.GetInt(PlayerPrefsAvatarKey)];
        nameText.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
    }
}
