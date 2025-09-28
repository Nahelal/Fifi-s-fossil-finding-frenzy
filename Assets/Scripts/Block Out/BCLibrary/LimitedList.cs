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

using System.Collections.Generic;

/// <summary>
/// Represents a list with a limited capacity, automatically removing oldest items when the maximum is exceeded.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class LimitedList<T> : List<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LimitedList{T}"/> class with a specified maximum capacity.
    /// </summary>
    /// <param name="max">The maximum number of items the list can hold.</param>
    public LimitedList(int max)
    {
        maximum = max;
    }
    
    /// <summary>
    /// The maximum number of items allowed in the list.
    /// </summary>
    public int maximum;

    /// <summary>
    /// Adds an item to the list and removes the oldest item if the maximum capacity is exceeded.
    /// </summary>
    /// <param name="item">The item to add to the list.</param>
    public new void Add(T item)
    {
        base.Add(item);
        if (Count > maximum)
        {
            RemoveAt(0);
        }
    }

    /// <summary>
    /// Adds a range of items to the list and removes oldest items if the maximum capacity is exceeded.
    /// </summary>
    /// <param name="items">The collection of items to add.</param>
    public new void AddRange(IEnumerable<T> items)
    {
        base.AddRange(items);
        while (Count > maximum)
        {
            RemoveAt(0);
        }
    }
}
