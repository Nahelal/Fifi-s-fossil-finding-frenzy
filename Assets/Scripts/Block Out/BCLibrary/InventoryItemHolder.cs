/************************************************************************************
// MIT License
// 
// Copyright (c) 2023 Mr EdEd Productions  
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ************************************************************************************/
using UnityEngine;

/// <summary>
/// A class that holds an inventory item and allows sending it to a target inventory.
/// </summary>
public class InventoryItemHolder : MonoBehaviour, ISendItems
{
    /// <summary>
    /// The inventory item held by this holder.
    /// </summary>
    public InventoryItem item;
    
    void Awake()
    {
        item.owner = gameObject;
    }

    /// <summary>
    /// Indicates whether this holder has an item.
    /// </summary>
    public bool hasItem => item != null;

    /// <summary>
    /// Sends the held item to the specified inventory.
    /// </summary>
    /// <param name="target">The inventory to send the item to.</param>
    /// <param name="type">The type of the item being sent.</param>
    /// <param name="count">The number of items to send (default is 1).</param>
    /// <returns>The number of items successfully sent.</returns>
    public int SendToInventory(Inventory target, string type, int count = 1)
    {
        if (item == null)
        {
            return 0;
        }

        target.Add(item);
        item = null;
        return 1;
    }
}
