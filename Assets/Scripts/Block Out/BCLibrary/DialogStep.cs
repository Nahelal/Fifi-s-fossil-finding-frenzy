/************************************************************************************
MIT License

Copyright (c) 2023 Mr EdEd Productions  

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
************************************************************************************/
using System;
using UnityEngine;

[Serializable]
public class DialogStep : MonoBehaviour
{
    public GameObject[] activateBefore;
    public GameObject[] deactivateBefore;
    public GameObject[] activateAfter;
    public GameObject[] deactivateAfter;


    public Sprite avatar;
    public AnimationFrames animatedFrames;
    public GameObject instantiate;
    public float delay = 0.1f;
    public bool isPlayer;
    public bool hideAvatar;
    public bool isThinking;
    [TextArea(1, 4)]
    public string speech;

    private GameObject _created;
    private Coroutine _running;

    public void Activate(bool before)
    {
        if (before)
        {
            foreach (var go in activateBefore)
            {
                go.SetActive(true);
            }
            foreach (var go in deactivateBefore)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach (var go in activateAfter)
            {
                go.SetActive(true);
            }
            foreach (var go in deactivateAfter)
            {
                go.SetActive(false);
            }
            if (_created)
            {
                Destroy(_created);
                _created = null;
            }
            if (_running != null)
            {
                StopCoroutine(_running);
                _running = null;
            }
        }
    }

    public void ConfigurePanel(ConversationUI.ConversationPanel panel)
    {
        if (instantiate)
        {
            _created = Instantiate(instantiate, panel.avatar.transform);
        }
        if (animatedFrames)
        {
            _running = StartCoroutine(animatedFrames.Animate(panel.avatar));
        }
    }

}



