using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveAndLoad
{
    private const char SEP = '/';
    private const string ESC_SLASH = "⁄";   // U+2044 FRACTION SLASH

    public static string GetId(Component c)
    {
        if (!c) return string.Empty;

        var sb = new StringBuilder(128)
            .Append(c.gameObject.scene.path.Replace("/", ESC_SLASH))
            .Append(SEP);

        Stack<string> stack = new();
        for (Transform t = c.transform; t != null; t = t.parent)
            stack.Push(t.name.Replace('/', '⁄'));

        while (stack.Count > 0)
        {
            sb.Append(stack.Pop());
            if (stack.Count > 0) sb.Append(SEP);
        }
        return sb.ToString();
    }

    public static T FindById<T>(string id) where T : Component
    {
        if (string.IsNullOrEmpty(id)) return null;

        int first = id.IndexOf(SEP);
        if (first <= 0) return null;

        string scenePath = id[..first].Replace(ESC_SLASH, "/");
        string[] parts = id[(first + 1)..].Split(SEP);

        Scene scene = SceneManager.GetSceneByPath(scenePath);
        if (!scene.isLoaded) return null;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name != parts[0]) continue;
            var t = Traverse(root.transform, parts, 1);
            if (t) return t.GetComponent<T>();
        }
        return null;
    }

    private static Transform Traverse(Transform parent, string[] parts, int idx)
    {
        if (idx >= parts.Length) return parent;
        foreach (Transform child in parent)
            if (child.name.Replace('/', '⁄') == parts[idx])
                return Traverse(child, parts, idx + 1);
        return null;
    }
}
