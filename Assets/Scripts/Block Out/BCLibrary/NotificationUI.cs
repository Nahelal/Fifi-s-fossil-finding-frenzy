using UnityEngine;
using System.Collections;
using TMPro;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance;

    public TextMeshProUGUI textMesh;

    Coroutine coroutine;

    private void Awake()
    {
        Instance = this;
        textMesh.text = "";
    }

    public static void ShowMessage(string message, float time = 2)
    {
        Instance.coroutine = Instance.StartCoroutine(Instance.Show(message, time, Instance.coroutine));

    }


    IEnumerator Show(string message, float time = 2, Coroutine existing = null)
    {
        yield return existing;
        textMesh.text = message;
        var c = textMesh.color;
        c.a = 1;
        textMesh.color = c;
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        while(c.a > 0)
        {
            c.a -= Time.deltaTime;
            textMesh.color = c;
            yield return null;
        }



    }

}
