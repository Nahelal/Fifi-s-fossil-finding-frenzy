/************************************************************************************
MIT License

Copyright (c) 2023 Mr EdEd Productions  Author: Mike Talbot (whydoidoidoit)

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

/// <summary>
/// Adds an item to a character's inventory when the character enters the trigger area,
/// and removes it upon exiting.
/// </summary>
public class AddToInventoryWhenInArea : MonoBehaviour
{
    /// <summary>
    /// The inventory item to be added or removed.
    /// </summary>
    public InventoryItem itemToAdd;

    /// <summary>
    /// Called when another collider enters the trigger area.
    /// Adds the specified item to the inventory of the entering character.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponentInParent<Inventory>();
        if (inventory)
        {
            inventory.Add(itemToAdd);
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger area.
    /// Removes the specified item from the inventory of the exiting character.
    /// </summary>
    /// <param name="other">The collider of the object exiting the trigger.</param>
    void OnTriggerExit(Collider other)
    {
        var inventory = other.GetComponentInParent<Inventory>();
        if (inventory)
        {
            inventory.Consume(itemToAdd.type);
        }
    }
}
