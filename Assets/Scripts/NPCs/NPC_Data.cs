using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPC_Data : ScriptableObject
{
    public Sprite characterSprite;
    public string characterName;
    public Color characterNameColor;
    [TextArea]
    public string[] characterDialogue;
}
