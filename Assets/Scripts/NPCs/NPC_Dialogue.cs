using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_Dialogue : MonoBehaviour
{
    NPC_SpeechBubble speechBubble;

    [SerializeField] NPC_Data npc_Data;
    [SerializeField] int dialogueIndex = 0;

    private void Awake()
    {
        speechBubble = FindFirstObjectByType<NPC_SpeechBubble>();
    }

    private void Start()
    {
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dialogueIndex++;

            if (dialogueIndex >= npc_Data.characterDialogue.Length)
            {
                dialogueIndex = 0;
                EndDialogue();

                return;
            }

            UpdateDialogue(npc_Data.characterDialogue[dialogueIndex]);
        }
    }

    public void StartDialogue()
    {
        speechBubble.gameObject.SetActive(true);

        speechBubble.nameTextbox.text = npc_Data.characterName;
        speechBubble.nameTextbox.color = npc_Data.characterNameColor;
        speechBubble.characterImage.sprite = npc_Data.characterSprite;

        //Displays first dialogue in NPC_Data
        UpdateDialogue(npc_Data.characterDialogue[dialogueIndex]);
    }

    //Call this when intracting in context sensitive scenarios with custom text
    public void UpdateDialogue(string text)
    {
        speechBubble.UpdateSpeechBubble(text);
    }

    public void EndDialogue()
    {
        speechBubble.gameObject.SetActive(false);
    }
}
