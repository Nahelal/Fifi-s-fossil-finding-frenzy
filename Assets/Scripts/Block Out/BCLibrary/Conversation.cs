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
/// Marks the root of a conversation tree consisting of child GameObjects representing conversation elements.
/// </summary>
[DefaultExecutionOrder(-1200)]
public class Conversation : MonoBehaviour
{
    public bool canExit = true;
    /// <summary>
    /// The unique identifier or name for this conversation.
    /// </summary>
    public string conversationName;

    /// <summary>
    /// Indicates whether this conversation is the opening conversation.
    /// </summary>
    public bool isOpener;

    [HideInInspector]
    public int count = 0;

    /// <summary>
    /// Deactivates all child GameObjects when the conversation object is enabled.
    /// </summary>
    void OnEnable()
    {
        transform.ActivateChildren(false);
    }
}