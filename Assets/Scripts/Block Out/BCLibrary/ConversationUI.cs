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
using System.Collections;
using System.Collections.Generic;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class ConversationUI : MonoBehaviour
{
    [Serializable]
    public class ConversationPanel
    {
        public GameObject panel;
        public Image avatar;
        public Image avatarShadow;
        public GameObject speak;
        public GameObject think;
        public TextMeshProUGUI text;
        public TextMeshProUGUI name;
    }
    public float rotationSpeed = 6;
    public float yOffset = 2;
    public KeyCode activationKey = KeyCode.T;
    public static ConversationUI Instance;
    public GameObject pressToTalk;
    public ConversationPanel player;
    public ConversationPanel npc;
    public ConversationMachine conversation;
    public GameObject source;
    public GameObject potential;
    public float separationDistance = 1.4f;
    public float separationSpeed = 2f;
    public Color choiceColor = Color.blue;
    Coroutine running;
    string currentPlayerState;
    int currentId = -1;
    bool canExit = true;
    bool wasRunning = false;


    public void SetLinkId(int id)
    {
        currentId = id + 1;
    }

    public static bool IsRunning()
    {
        return Instance?.running != null;
    }

    void Awake()
    {
        Instance = this;
        player.panel.SetActive(false);
        npc.panel.SetActive(false);
        pressToTalk.SetActive(false);
    }

    public void RegisterConversation(GameObject sourceGameObject)
    {
        if (!sourceGameObject || conversation != null) return;
        var former = potential;
        potential = null;
        var conversations = sourceGameObject.GetComponentsInChildren<Conversation>(false);
        var ok = false;
        foreach (var c in conversations)
        {
            if (c.isOpener)
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            potential = former;
            return;
        }
        potential = sourceGameObject;
    }

    public void UnregisterConversation(GameObject sourceGameObject)
    {
        if (sourceGameObject == potential)
        {
            potential = null;
        }
    }

    bool CanTalk()
    {
        if (!potential) return false;
        var canTalk = potential.GetState() as ICanTalk;
        return canTalk == null || canTalk.canTalk;
    }


    void LateUpdate()
    {

        var canTalk = CanTalk();
        pressToTalk.SetActive(potential && canTalk);
        if (potential && canTalk)
        {
            var screenPos = Camera.main.WorldToScreenPoint(potential.transform.position + Vector3.up * yOffset);

            pressToTalk.transform.position = screenPos;
            var autoStart = potential.GetComponent<AutoStartConversation>();
            if (Input.GetKeyDown(activationKey) || (autoStart && autoStart.times > 0))
            {
                if (autoStart)
                {
                    autoStart.times--;
                }
                source = potential;
                potential = null;
                running = StartCoroutine(HaveConversation(conversation = new ConversationMachine(source.transform)));
            }
        }
    }

    protected void CheckEarlyExit()
    {
        if (running != null && canExit)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                StopCoroutine(running);
                conversation?.unfreeze();
                conversation?.reset();
                conversation = null;
                running = null;
                PlayerMarker.Instance.SetState(currentPlayerState);

                TurnOffPanels();
                wasRunning = false;
                enabled = true;
            }
        }
    }

    protected void TurnOffPanels()
    {
        player.panel.SetActive(false);
        npc.panel.SetActive(false);
    }

    protected static readonly List<string> Joiner = new();

    protected virtual IEnumerator HaveConversation(ConversationMachine newConversation)
    {

        var machine = PlayerMarker.Instance.GetMachine();
        enabled = false;

        currentPlayerState = PlayerMarker.Instance.SetState("Paused");
        canExit = newConversation.canExit;

        pressToTalk.SetActive(false);
        var npcTransform = newConversation.source.transform;

        var playerTransform = machine.transform;

        var canRotate = newConversation.source.GetState() as ICanRotate;
        var playerController = machine.GetComponent<CharacterController>();

        while (true)
        {
            var vector = (npcTransform.position - playerTransform.position);
            vector.y = 0;
            var distance = vector.magnitude;
            vector.Normalize();
            if (distance < separationDistance)
            {
                playerController.Move(-vector * (Time.deltaTime * separationSpeed));

            }
            var playerRotation = Quaternion.LookRotation(vector);
            var angleDiffPlayer = Quaternion.Angle(playerTransform.rotation, playerRotation);
            var angleDiffCharacter = 0f;
            if (canRotate?.canRotate == true)
            {
                if (distance < separationDistance)
                {
                    npcTransform.position += vector * (Time.deltaTime * separationSpeed);
                }
                var characterRotation = Quaternion.LookRotation(-vector);
                angleDiffCharacter = Quaternion.Angle(npcTransform.rotation, characterRotation);
                npcTransform.rotation = Quaternion.Slerp(npcTransform.rotation,
                    characterRotation, Time.deltaTime * rotationSpeed);
            }

            if (angleDiffPlayer < 4 && angleDiffCharacter < 4) break;
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, playerRotation,
                Time.deltaTime * rotationSpeed);


            yield return null;
        }


        while (newConversation.step)
        {
            TurnOffPanels();
            var panel = newConversation.step.isPlayer ? player : npc;
            if (newConversation.useContent)
            {
                if (newConversation.step.isPlayer)
                {
                    PlayerMarker.Instance.BroadcastMessage("Talking", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    npcTransform.BroadcastMessage("Talking", SendMessageOptions.DontRequireReceiver);
                }
                newConversation.step.Activate(true);
                yield return WriteStepToPanel(panel, newConversation.step, newConversation.step.isPlayer ? playerTransform : npcTransform);
                yield return WaitForSkip();
                if (newConversation.step.isPlayer)
                {
                    PlayerMarker.Instance.BroadcastMessage("NotTalking", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    npcTransform.BroadcastMessage("NotTalking", SendMessageOptions.DontRequireReceiver);
                }
                newConversation.step.Activate(false);
            }
            else
            {
                newConversation.step.Activate(true);
                newConversation.step.Activate(false);
            }

            newConversation.useContent = true;

            var choices = conversation.choices;
            if (choices.Count > 0)
            {
                if (choices.Count > 1)
                {
                    TurnOffPanels();
                    Joiner.Clear();
                    var i = 1;
                    foreach (var choice in choices)
                    {
                        Joiner.Add($"({i}) <link={i++.ToString()}><color=#{ColorUtility.ToHtmlStringRGBA(choiceColor)}>{choice.choice.Trim()}</color></link>");
                    }

                    if (player.speak)
                    {
                        player.speak.SetActive(false);
                        player.think.SetActive(true);
                    }
                    yield return WriteTextToPanel(player,
                        $"<size=70%>What should I say?</size>\n{string.Join("\n", Joiner)}", playerTransform);
                    while (true)
                    {
                        if (newConversation.canExit) CheckEarlyExit();
                        var choice = GetChoice(choices.Count);
                        if (choice > 0)
                        {
                            currentId = -1;
                            TurnOffPanels();
                            if (player.speak)
                            {
                                player.speak.SetActive(true);
                                player.think.SetActive(false);
                            }
                            yield return WriteTextToPanel(player, choices[choice - 1].speech, playerTransform);
                            yield return WaitForSkip();
                            newConversation.Choose(choices[choice - 1]);
                            break;
                        }

                        yield return null;
                    }
                    if (player.speak)
                    {
                        player.speak.SetActive(true);
                        player.think.SetActive(false);
                    }

                }
                else
                {
                    TurnOffPanels();
                    yield return WriteTextToPanel(player, choices[0].speech, playerTransform);
                    yield return WaitForSkip();
                    newConversation.Choose(choices[0]);
                }
            }
            else
            {
                newConversation.NextStep();
            }
        }

        TurnOffPanels();
        PlayerMarker.Instance.BroadcastMessage("NotTalking", SendMessageOptions.DontRequireReceiver);
        PlayerMarker.Instance.SetState(currentPlayerState);
        npcTransform.gameObject.BroadcastMessage("NotTalking", SendMessageOptions.DontRequireReceiver);

        yield return null;
        yield return null;
        yield return null;
        yield return null;
        newConversation.unfreeze();
        newConversation.reset();
        enabled = true;
        conversation = null;
        running = null;
        yield return null;
        yield return null;
        yield return null;
    }

    protected virtual IEnumerator WriteStepToPanel(ConversationPanel panel, DialogStep step, Transform follow)
    {
        if (panel.avatar)
        {
            panel.avatar.gameObject.SetActive(!step.hideAvatar && (step.avatar || step.isPlayer));
            panel.avatar.sprite = step.avatar ? step.avatar : panel.avatar.sprite;
        }

        if (panel.name)
        {
            panel.name.text = follow.name;
        }

        if (panel.speak)
        {
            if (step.isThinking)
            {
                panel.speak.SetActive(false);
                panel.think.SetActive(true);
            }
            else
            {
                panel.speak.SetActive(true);
                panel.think.SetActive(false);
            }
        }
        step.ConfigurePanel(panel);
        yield return WriteTextToPanel(panel, step.speech, follow);
    }

    protected virtual IEnumerator WriteTextToPanel(ConversationPanel panel, string text, Transform follow)
    {
        panel.panel.SetActive(false);
        panel.text.text = "";
        panel.panel.SetActive(true);
        panel.text.text = text;
        yield return WaitFor(panel.text);
    }


    protected IEnumerator WaitForSkip()
    {
        do
        {
            CheckEarlyExit();
            if (Input.anyKeyDown)
            {
                yield return null;
                yield break;
            }
            yield return null;
        } while (true);
    }


    protected IEnumerator WaitFor(TextMeshProUGUI text)
    {
        var typewriter = text.GetComponent<TypewriterCore>();

        do
        {
            CheckEarlyExit();
            yield return null;
            if (Input.anyKeyDown)
            {
                typewriter.SkipTypewriter();
            }
        } while (!typewriter.TextAnimator.allLettersShown);

        yield return null;

        currentId = -1;
    }

    protected int GetChoice(int max = 9)
    {
        if (currentId != -1)
        {
            return currentId;
        }
        for (var i = 0; i < max; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                return i + 1;
            }
        }

        return -1;
    }


    public void StartConversation(Conversation newConversation)
    {
        conversation = new ConversationMachine(newConversation);
        StartCoroutine(HaveConversation(conversation));
    }
}
