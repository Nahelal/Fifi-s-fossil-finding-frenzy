
using Cinemachine;

using UnityEngine;

public class SoloOnKey : MonoBehaviour
{
    public KeyCode key;
    CinemachineVirtualCamera vc;

    void Start()
    {
        vc = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(key))
        {
            CinemachineBrain.SoloCamera = vc;
        }
#endif
    }
}