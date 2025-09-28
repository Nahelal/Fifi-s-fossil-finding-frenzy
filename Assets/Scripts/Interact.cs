using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class Interact : MonoBehaviour
{
    public GameObject UIPopupPrefab;
    public Vector3 PopupOffset;
    public KeyCode interactKey = KeyCode.E;
    public UnityEvent OnInteract;
    bool isPlayerInRange = false;
    public bool active = false;

    GameObject UIPrefabInstance;
    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;

        if (UIPopupPrefab)
        {
            UIPrefabInstance = Instantiate(UIPopupPrefab, transform.position + PopupOffset, Quaternion.Euler(35f, 0, 0));
            UIPrefabInstance.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!active) return;
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (UIPopupPrefab) UIPrefabInstance.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!active) return;
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (UIPopupPrefab) UIPrefabInstance.SetActive(false);
        }
    }



    private void Update()
    {
        if (!active)
        {
            if (UIPopupPrefab) UIPrefabInstance.SetActive(false);
        }
        if (Input.GetKeyDown(interactKey) && isPlayerInRange && active)
        {
            OnInteract.Invoke();
        }
    }
}
