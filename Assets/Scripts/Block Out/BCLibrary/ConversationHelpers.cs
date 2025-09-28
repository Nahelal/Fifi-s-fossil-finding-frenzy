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
/// Provides extension methods to simplify working with Conversation components on GameObjects and Components.
/// </summary>
public static class ConversationHelpers
{
    /// <summary>
    /// Retrieves a specific Conversation component attached to a Component based on the conversation's name.
    /// </summary>
    /// <param name="component">The component to search for conversations.</param>
    /// <param name="name">The name of the conversation to retrieve.</param>
    /// <returns>The matching Conversation or null if not found.</returns>
    public static Conversation GetConversation(this Component component, string name)
    {
        var conversations = component.GetComponents<Conversation>();
        foreach (var conversation in conversations)
        {
            if (conversation.name == name) return conversation;
        }

        return null;
    }
    
    /// <summary>
    /// Retrieves a specific Conversation component attached to a GameObject based on the conversation's name.
    /// </summary>
    /// <param name="gameObject">The GameObject to search for conversations.</param>
    /// <param name="name">The name of the conversation to retrieve.</param>
    /// <returns>The matching Conversation or null if not found.</returns>
    public static Conversation GetConversation(this GameObject gameObject, string name)
    {
        var conversations = gameObject.GetComponents<Conversation>();
        foreach (var conversation in conversations)
        {
            if (conversation.name == name) return conversation;
        }

        return null;
    }

    /// <summary>
    /// Retrieves an opening conversation marked as an opener from a GameObject, selecting randomly if multiple exist.
    /// </summary>
    /// <param name="gameObject">The GameObject to search for opener conversations.</param>
    /// <returns>An opener Conversation or null if none are marked as openers.</returns>
    public static Conversation GetOpener(this GameObject gameObject)
    {
        var conversations = gameObject.GetConversations();
        Conversation lastOpener = null;
        foreach (var conversation in conversations)
        {
            if (conversation.isOpener)
            {
                lastOpener = conversation;
                if (Random.value < 0.5f)
                {
                    return conversation;
                }
            }
        }

        return lastOpener;
    }

    /// <summary>
    /// Retrieves all Conversation components attached to a GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject to search for conversations.</param>
    /// <returns>An array of Conversation components.</returns>
    public static Conversation[] GetConversations(this GameObject gameObject)
    {
        return gameObject.GetComponents<Conversation>();
    }
}