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

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogChoice))]
public class DialogChoiceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var choiceProp = property.FindPropertyRelative("choice");
        var conversationProp = property.FindPropertyRelative("conversation");
        var dialogProp = property.FindPropertyRelative("dialog");
        var displayDialogProp = property.FindPropertyRelative("speech");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;

        Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

        // Draw "choice" field
        EditorGUI.PropertyField(currentRect, choiceProp);
        currentRect.y += lineHeight + padding;

        // Draw label
        EditorGUI.LabelField(currentRect, "Speech:");
        currentRect.y += lineHeight + padding;

// Draw text area below label
        currentRect.height = lineHeight * 3;
        displayDialogProp.stringValue = EditorGUI.TextArea(currentRect, displayDialogProp.stringValue);
        currentRect.y += currentRect.height + padding;
        currentRect.height = lineHeight;

        // Draw "conversation" field
        EditorGUI.PropertyField(currentRect, conversationProp);
        currentRect.y += lineHeight + padding;

        
        // Only draw "dialog" if "conversation" is not assigned
        if (conversationProp.objectReferenceValue == null)
        {
            EditorGUI.PropertyField(currentRect, dialogProp, new GUIContent("Dialog"), true);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;
        float height = 0f;

        // "choice" field
        height += lineHeight + padding;

        // "Speech:" label
        height += lineHeight + padding;

        // Multiline textarea (3 lines)
        height += (lineHeight * 3) + padding;

        // "conversation" field
        height += lineHeight + padding;

        var conversationProp = property.FindPropertyRelative("conversation");
        var dialogProp = property.FindPropertyRelative("dialog");

        // "dialog" field (only when conversation is null)
        if (conversationProp.objectReferenceValue == null)
        {
            height += EditorGUI.GetPropertyHeight(dialogProp, true) + padding;
        }

        return height;
    }
}