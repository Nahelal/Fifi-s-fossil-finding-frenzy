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

/// <summary>
/// Adds an InventoryItem to the Inventory component when enabled.
/// </summary>
[DefaultExecutionOrder(500)]
public class AddInventoryItem : Enabler
{
    /// <summary>
    /// The inventory item to add.
    /// </summary>
    public InventoryItem item;

    /// <summary>
    /// Reference to the Inventory where the item will be added.
    /// </summary>
    public Inventory inventory;

    /// <summary>
    /// Ensures the inventory reference is set, fetching it from the parent if needed.
    /// </summary>
    void Awake()
    {
        inventory ??= GetComponentInParent<Inventory>();
    }

    /// <summary>
    /// Adds the inventory item to the inventory and then clears the item reference.
    /// </summary>
    protected override void Enable()
    {
        if (inventory)
        {
            inventory.Add(item);
            item = null;
        }
    }
}
