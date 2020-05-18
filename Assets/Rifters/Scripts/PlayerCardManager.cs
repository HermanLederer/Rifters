using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardManager : MonoBehaviour
{
    [Header("Card Components")]
    [SerializeField] private Image m_avatarImage = null;
    [SerializeField] private Text m_playerName = null;
    [SerializeField] private Text m_readyState = null;

    public void SetAvatarImage(Image avatarImage)
    {
        m_avatarImage = avatarImage;
    }

    public void SetPlayerName(string playerName)
    {
        m_playerName.text = playerName;
    }

    public void SetReadyState(bool isReady)
    {
        m_readyState.text = isReady? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
    }
}
