using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abstract_Ammonites;
using System.Linq;
using UnityEngine.SceneManagement;

public class CameraShake : MonoBehaviour
{
    float x;
    float y;

    private List<GameObject> gameObj;

    private GameObject cam;
    private GameObject player;

    public float timeLimit = 2f;
    float timeElapsed = 0f;

    Vector3 ogPos;

    // Start is called before the first frame update
    void Start()
    {

        cam = GameObject.Find("Camera");

        gameObj = World_Utils.grab_collection();

        ogPos = cam.transform.position;

        player = World_Utils.grab_all_tiles_of_type(gameObj, World_Utils.p_atts.player).First().self;

        StartCoroutine(CamShake());
    }

    //StartCoroutine(CamShake());
    public IEnumerator CamShake()
    {
        while (timeElapsed < timeLimit)
        {
            x = Random.Range(-0.3f, 0.3f);
            y = Random.Range(-0.3f, 0.3f);

            cam.transform.position = new Vector3(x, y, 0) + ogPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        cam.transform.position = ogPos;

        gameObject.transform.ActivateChildren(true);
    }
}
