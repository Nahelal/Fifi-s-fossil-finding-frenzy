using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ConversationUIBubbles : ConversationUI
{
    [Space(40)]
    [Header("Speech Bubble Info")]
    public float worldYOffset = 2f;
    public float bubbleLineHeight = 40;
    public float minBubbleHeight = 140;
    public float maxBubbleHeight = 350;
    protected override IEnumerator WriteTextToPanel(ConversationPanel panel, string text, Transform follow)
    {
        var follower = panel.panel.GetComponentInParent<UIFollowTransform>();
        panel.text.text = text;
        panel.text.ForceMeshUpdate(true, true);
        var rt = panel.panel.GetComponent<RectTransform>();
        rt.sizeDelta= new Vector2(rt.sizeDelta.x,  Mathf.Clamp(minBubbleHeight - bubbleLineHeight + panel.text.textInfo.lineCount * bubbleLineHeight, minBubbleHeight, maxBubbleHeight));
        panel.text.text = "";
        panel.text.ForceMeshUpdate(true, true);
        follower.target = follow;
        var offset = follow.GetComponent<SpeechOffset>();
        follower.yOffset = offset ? offset.yOffset : worldYOffset;
        return base.WriteTextToPanel(panel, text, follow);
    }
}
