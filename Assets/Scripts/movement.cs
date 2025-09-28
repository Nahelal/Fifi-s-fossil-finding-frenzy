using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TBone;
using System;
public class movement : MonoBehaviour
{  
    
    //Simple Init

    public float spd = 3.0f;
    public float grav_mult= 2.0f;
    public CharacterController characterController;
    private Dictionary<Utils.dirs, Vector3> our_dirs = new Dictionary<Utils.dirs, Vector3>();
    // Start is called before the first frame update

  
    void Start()
    {
        //TBone is a util namespace, it has many of the functions used here abstracted into there. use it or add to it as you like!
        characterController.detectCollisions = true;
        our_dirs = Utils.return_dir_dict();
    }

    private void input_system()
    {
        
        //Bad movement, however - it's fine for now.
        if (Input.GetKey(KeyCode.W) == true)
        {
            characterController.SimpleMove(our_dirs[Utils.dirs.FORWARD] * 3);
            Utils.rotate_character(gameObject, our_dirs[Utils.dirs.FORWARD]);
        }
        else if (Input.GetKey(KeyCode.S) == true)
        {
            characterController.SimpleMove(our_dirs[Utils.dirs.BACKWARDS] * 3);
            Utils.rotate_character(gameObject, our_dirs[Utils.dirs.BACKWARDS]);

        }
        else if (Input.GetKey(KeyCode.A) == true)
        {
            characterController.SimpleMove(our_dirs[Utils.dirs.LEFT] * 3);
            Utils.rotate_character(gameObject, our_dirs[Utils.dirs.LEFT]);
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            characterController.SimpleMove(our_dirs[Utils.dirs.RIGHT] * 3);
            Utils.rotate_character(gameObject, our_dirs[Utils.dirs.RIGHT]);
        }
    }

    void Update()
    {
        input_system();
        Utils.apply_gravity(gameObject, our_dirs, grav_mult);
    }
}
