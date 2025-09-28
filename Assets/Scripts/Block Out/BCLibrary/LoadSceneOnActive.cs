using UnityEngine.SceneManagement;

public class LoadSceneOnActive : Enabler
{
    public string sceneToLoad;
    protected override void Enable()
    {
        SaveState.Instance.Save();
        SceneManager.LoadScene(sceneToLoad);
    }
}