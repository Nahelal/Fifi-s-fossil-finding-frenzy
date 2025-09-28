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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ConversationMachine
{
    public Component source;
    public DialogStep step;
    List<DialogStep> steps;
    public Action reset;
    public Action unfreeze;
    public bool useContent = true;
    public bool canExit = true;

    public void AddUnfreeze(Action extra)
    {
        var existing = unfreeze;
        unfreeze = () =>
        {
            existing();
            extra();
        };


    }

    public ConversationMachine(Component component)
    {
        source = component;
        var kill = source.GetState()?.Pause();
        unfreeze = () => { kill?.Invoke(); };
        Conversation opening = null;
        var conversations = component.GetComponentsInChildren<Conversation>(false);
        var openers = new List<Conversation>();
        var min = int.MaxValue;
        foreach (var conv in conversations)
        {
            if (conv.count <= min)
            {
                min = conv.count;
                if (conv.isOpener)
                {
                    openers.Add(conv);
                    opening = conv;
                }
            }
        }

        var toBeSpoken = openers.Where(c => c.count == min).ToList();
        opening = toBeSpoken[UnityEngine.Random.Range(0, toBeSpoken.Count)];
        opening.count++;
        canExit = opening.canExit;
        var copy = Object.Instantiate(opening, source.transform);
        copy.transform.ActivateChildren(true);
        steps = copy.GetComponentsInDirectChildren<DialogStep>();
        step = steps[0];
        reset = () =>
        {
            Object.Destroy(copy.gameObject);
        };



    }

    public ConversationMachine(DialogStep startStep)
    {
        source = startStep.transform.parent;
        steps = startStep.transform.parent.GetComponentsInDirectChildren<DialogStep>();
        step = startStep;
        reset = () => { };
    }

    static readonly List<DialogChoice> Dummy = new();

    public List<DialogChoice> choices => step ? step.GetComponentsInDirectChildren<DialogStep>(true).Count > 0 ? Dummy : step.GetComponentsInDirectChildren<DialogChoice>(false) : null;

    public void Choose(DialogChoice choice)
    {
        choice.transform.ActivateChildren(true);
        if (choice.dialog)
        {
            steps = choice.dialog.transform.parent.GetComponentsInDirectChildren<DialogStep>();
            reset();
            step = Object.Instantiate(choice.dialog, source.transform);
            var localStep = step;
            reset = () =>
            {
                Object.Destroy(localStep);
            };
            steps = new List<DialogStep>() { step };
        }
        else
        {
            var alternateSteps = choice.GetComponentsInDirectChildren<DialogStep>();
            if (alternateSteps.Count > 0)
            {
                steps = alternateSteps;
                step = alternateSteps[0];
            }
            else
            {
                NextStep();
            }
        }

    }

    public bool NextStep()
    {
        useContent = true;
        var alternateSteps = step.GetComponentsInDirectChildren<DialogStep>();
        if (alternateSteps.Count > 0)
        {
            steps = alternateSteps;
            step = alternateSteps[0];

        }
        else
        {

            var link = step.GetComponentInChildren<DialogLink>();
            if (link)
            {
                reset();
                step = Object.Instantiate(link.dialog, source.transform);
                var localStep = step;
                reset = () => { Object.Destroy(localStep); };
                steps = new List<DialogStep>() { step };
                useContent = link.useStepContent;
                return step;
            }

            var index = steps.IndexOf(step) + 1;
            if (index > 0 && index < steps.Count)
            {
                step = steps[index];
            }
            else
            {
                step = null;
            }
        }

        var subLink = step?.GetComponent<DialogLink>();
        if (subLink)
        {
            reset();
            step = Object.Instantiate(subLink.dialog, source.transform);
            var localStep = step;
            reset = () =>
            {
                Object.Destroy(localStep);
            };
            steps = new List<DialogStep>() { step };
            useContent = subLink.useStepContent;

        }

        return step;
    }
}
