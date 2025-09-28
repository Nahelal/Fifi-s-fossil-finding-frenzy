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

using UnityEngine;
/// <summary>
/// Activates and deactivates specified GameObjects based on a delay and active period.
/// </summary>
[DefaultExecutionOrder(500)]
public class ActivateWhenActive : Enabler, IActivator
{
    /// <summary>
    /// GameObjects to be activated when the component becomes active.
    /// </summary>
    public GameObject[] activate;

    /// <summary>
    /// GameObjects to be deactivated when the component becomes active.
    /// </summary>
    public GameObject[] deactivate;

    /// <summary>
    /// Duration for which the GameObjects remain activated.
    /// </summary>
    public float forPeriod = 0;

    /// <summary>
    /// Delay before activation occurs.
    /// </summary>
    public float delay = 0;

    /// <summary>
    /// Determines whether to deactivate the GameObjects when the component is disabled.
    /// </summary>
    public bool deactivateOnDisable = true;

    float timeToDeactivate;
    float timeToActivate;
    bool active;

    protected override void OnEnable()
    {
        timeToActivate = delay;
        timeToDeactivate = forPeriod;
        active = false;
        base.OnEnable();
    }

    /// <summary>
    /// Resets activation timers and sets the active state to false when the component is enabled.
    /// </summary>
    protected override void Enable()
    {
        timeToActivate = delay;
        timeToDeactivate = forPeriod;
        active = false;
    }

    /// <summary>
    /// Called when the component is disabled; deactivates GameObjects if required.
    /// </summary>
    protected override void OnDisable()
    {
        if (deactivateOnDisable || forPeriod > 0)
        {
            Activate(false);
        }
    }

    /// <summary>
    /// Activates or deactivates the specified GameObjects.
    /// </summary>
    /// <param name="isActive">If true, activates the GameObjects; otherwise, deactivates them.</param>
    public void Activate(bool isActive)
    {
        active = isActive;
        if (activate != null)
        {
            foreach (var go in activate)
            {
                if (go) go.SetActive(isActive);
            }
        }
        if (deactivate != null)
        {
            foreach (var go in deactivate)
            {
                if (go) go.SetActive(!isActive);
            }
        }
    }

    /// <summary>
    /// Manages activation timing, triggering activation after a delay and deactivating after the active period expires.
    /// </summary>
    void Update()
    {
        timeToActivate -= Time.deltaTime;
        if (!active && timeToActivate <= 0)
        {
            Activate(true);
        }

        if (!active) return;
        timeToDeactivate -= Time.deltaTime;
        if (timeToDeactivate <= 0)
        {
            enabled = false;
        }
    }
}