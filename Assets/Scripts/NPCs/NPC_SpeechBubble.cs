using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci;
using UnityEngine.UI;
using TMPro;

public class NPC_SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextbox;
    public TextMeshProUGUI nameTextbox;
    public Image characterImage;

    public void UpdateSpeechBubble(string text)
    {
        dialogueTextbox.text = text;
    }
}
