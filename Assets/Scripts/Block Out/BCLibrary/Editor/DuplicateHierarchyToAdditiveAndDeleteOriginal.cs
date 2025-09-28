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
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DuplicateHierarchyToAdditiveAndDeleteOriginal : Editor
{
    [MenuItem("GameObject/Custom/Duplicate Hierarchy To Additive & Delete Originals", false, 0)]
    static void DuplicateHierarchy()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected.");
            return;
        }

        // Pick target scene (must be under Assets)
        string scenePath = EditorUtility.OpenFilePanel("Select target scene", "Assets", "unity");
        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogWarning("No scene selected.");
            return;
        }
        if (scenePath.StartsWith(Application.dataPath))
            scenePath = "Assets" + scenePath.Substring(Application.dataPath.Length);

        Scene targetScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        if (!targetScene.IsValid())
        {
            Debug.LogError("Failed to open scene: " + scenePath);
            return;
        }

        if (!EditorUtility.DisplayDialog("Delete Originals?",
            "The selected objects will be duplicated into the target scene and then deleted from the current scene. Continue?", "Yes", "No"))
        {
            return;
        }

        Dictionary<Transform, GameObject> parentMap = new Dictionary<Transform, GameObject>();
        HashSet<GameObject> movedTopParents = new HashSet<GameObject>(); // track which top parents we've moved

        foreach (GameObject go in selectedObjects)
        {
            // Duplicate parent's chain as empty objects, preserving local transform.
            GameObject newParentChain = DuplicateParentChain(go.transform.parent, parentMap);

            // Duplicate the selected object.
            GameObject duplicate = Instantiate(go);
            duplicate.name = go.name;  // Remove "(Clone)" suffix.
            Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate object");

            if (newParentChain != null)
            {
                // Preserve original local transform relative to its parent.
                Vector3 localPos = go.transform.localPosition;
                Quaternion localRot = go.transform.localRotation;
                Vector3 localScale = go.transform.localScale;

                duplicate.transform.SetParent(newParentChain.transform, false);
                duplicate.transform.localPosition = localPos;
                duplicate.transform.localRotation = localRot;
                duplicate.transform.localScale = localScale;
            }
            else
            {
                duplicate.transform.SetParent(null, false);
            }

            // Get the top-most parent in the duplicated chain (or the duplicate if no chain exists).
            GameObject topParent = newParentChain != null ? GetRoot(newParentChain) : duplicate;
            // Only move if not already in target scene.
            if (topParent.scene != targetScene && !movedTopParents.Contains(topParent))
            {
                SceneManager.MoveGameObjectToScene(topParent, targetScene);
                movedTopParents.Add(topParent);
            }
        }

        // Delete original objects.
        foreach (GameObject go in selectedObjects)
        {
            Undo.DestroyObjectImmediate(go);
        }

        EditorSceneManager.SaveScene(targetScene);
        Debug.Log("Duplicated hierarchy moved to scene: " + targetScene.path + " and originals deleted.");
    }

    // Recursively duplicate the parent chain as empty GameObjects, copying local transform values.
    static GameObject DuplicateParentChain(Transform originalParent, Dictionary<Transform, GameObject> parentMap)
    {
        if (originalParent == null)
            return null;

        if (parentMap.ContainsKey(originalParent))
            return parentMap[originalParent];

        GameObject newGrandParent = DuplicateParentChain(originalParent.parent, parentMap);

        GameObject emptyParent = new GameObject(originalParent.name);
        if (newGrandParent != null)
        {
            emptyParent.transform.SetParent(newGrandParent.transform, false);
            emptyParent.transform.localPosition = originalParent.localPosition;
            emptyParent.transform.localRotation = originalParent.localRotation;
            emptyParent.transform.localScale = originalParent.localScale;
        }
        else
        {
            // If no parent exists, copy world transform.
            emptyParent.transform.position = originalParent.position;
            emptyParent.transform.rotation = originalParent.rotation;
            emptyParent.transform.localScale = originalParent.localScale;
        }

        Undo.RegisterCreatedObjectUndo(emptyParent, "Create empty parent");
        parentMap.Add(originalParent, emptyParent);
        return emptyParent;
    }

    // Retrieve the top-most GameObject in the duplicated parent chain.
    static GameObject GetRoot(GameObject go)
    {
        while (go.transform.parent != null)
            go = go.transform.parent.gameObject;
        return go;
    }
}