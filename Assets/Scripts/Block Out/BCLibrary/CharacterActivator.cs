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
/// Activates or deactivates specified GameObjects when a character enters or exits the trigger zone.
/// </summary>
public class CharacterActivator : MonoBehaviour, IActivator
{
    public bool onlyIfEnabled;
    /// <summary>
    /// GameObjects to activate when a character enters the trigger zone.
    /// </summary>
    public GameObject[] activate;

    /// <summary>
    /// GameObjects to deactivate when a character enters the trigger zone.
    /// </summary>
    public GameObject[] deactivate;

    /// <summary>
    /// If true, the objects will be deactivated when the character exits the trigger zone.
    /// </summary>
    public bool deactivateOnExit = true;

    // Tracks the number of character objects within the trigger zone.
    int inside = 0;

    /// <summary>
    /// Called when a Collider enters the trigger zone.
    /// If the collider belongs to a character (determined by the presence of a CharacterMarker),
    /// increments the count and activates the specified GameObjects.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        var marker = other.GetComponentInParent<CharacterMarker>();
        if (marker?.enabled == true || (!onlyIfEnabled && marker))
        {
            inside++;
            Activate(true);
        }
    }

    /// <summary>
    /// Called when a Collider exits the trigger zone.
    /// If <see cref="deactivateOnExit"/> is true and the collider belongs to a character,
    /// decrements the count and deactivates the specified GameObjects if no characters remain.
    /// </summary>
    /// <param name="other">The Collider exiting the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (deactivateOnExit)
        {
            var marker = other.GetComponentInParent<CharacterMarker>();
            if (marker?.enabled == true || (!onlyIfEnabled && marker))
            {
                inside--;
                if (inside <= 0)
                {
                    Activate(false);
                }
            }
        }
    }

    /// <summary>
    /// Activates or deactivates the specified GameObjects.
    /// </summary>
    /// <param name="isActive">If true, activates the GameObjects; if false, deactivates them.</param>
    public void Activate(bool isActive)
    {
        foreach (var go in activate)
        {
            go.SetActive(isActive);
        }
        foreach (var go in deactivate)
        {
            go.SetActive(!isActive);
        }
    }
}