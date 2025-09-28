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
using UnityEngine.UIElements;
using System.Linq;

[CustomEditor(typeof(State), true)]
[CanEditMultipleObjects]
public class StateEditor : Editor
{
    private Label headerLabel;

    public override VisualElement CreateInspectorGUI()
    {
        var container = new VisualElement();

        // Build header text based on selected targets
        var headerText = GetHeaderText();

        headerLabel = new Label(headerText)
        {
            style =
            {
                fontSize = 24,
                marginBottom = 5,
                color = Color.white,
                backgroundColor = new Color(0.1f, 0.1f, 0.2f),
                paddingBottom = 2,
                paddingLeft = 5,
                paddingRight = 5,
                paddingTop = 2
            }
        };

        container.Add(headerLabel);

        // Create an IMGUIContainer to host the default inspector.
        var defaultInspector = new IMGUIContainer(() =>
        {
            DrawDefaultInspector();
        });
        container.Add(defaultInspector);

        return container;
    }

    private void OnEnable()
    {
        EditorApplication.update += UpdateHeader;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateHeader;
    }

    // Determines the header text based on multi-object selection.
    private string GetHeaderText()
    {
        // If there's only one target, use its GetLabel() result.
        if (targets.Length == 1)
        {
            return $"State: {((State)target).GetLabel()}";
        }
        else
        {
            // For multiple selections, check if all have the same label.
            var labels = targets.Cast<State>().Select(s => s.GetLabel()).Distinct().ToArray();
            if (labels.Length == 1)
            {
                return $"State: {labels[0]}";
            }
            else
            {
                return "Multiple States";
            }
        }
    }

    private void UpdateHeader()
    {
        if (headerLabel != null)
        {
            var newHeaderText = GetHeaderText();
            if (headerLabel.text != newHeaderText)
            {
                headerLabel.text = newHeaderText;
                headerLabel.MarkDirtyRepaint();
            }
        }
    }
}