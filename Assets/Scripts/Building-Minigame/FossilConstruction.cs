using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilConstruction : MonoBehaviour
{
    public BuildingTray buildingTray;

    private bool bDragging = false;
    private bool bHasSnapped = false;
    private Vector3 offset;
    private Rigidbody2D rb;

    public float snapDistance = 1.0f;
    private float rotateSpeed = 100f;

    private Vector3 currentMousePos;
    private Vector3 lastMousePos;

    private Transform closestSnapPoint;
    private GameObject closestBone;
    private Transform closestJoint;

    //ref to bone spawn script
    public BoneSpawning spawnManager;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private HashSet<FossilConstruction> connectedBones = new HashSet<FossilConstruction>();

    private HingeJoint2D activeJoint;
    public LayerMask boneLayer;

    //joint range bits
    private BoneJointTrigger inRangeJoint = null;
    private BoneJointTrigger otherInRangeJoint = null;

    [Header("SFX")]
    [SerializeField] GameObject grabSO;
    [SerializeField] GameObject dropSO;
    [SerializeField] GameObject connectSO;
    [SerializeField] GameObject disconnectSO;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        //no rotation
        rb.freezeRotation = true;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        buildingTray = FindAnyObjectByType<BuildingTray>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseRelease(); 
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            HandleRightClick();
        }

        if (bDragging)
        {
            HandleDragging();
        }
    }

    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, boneLayer);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            DetachBone();
        }
        else
        {
            Debug.Log("bum");
        }
    }

    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, boneLayer);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            bDragging = true;

            connectedBones = GetConnectedBones();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            currentMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            offset = transform.position - currentMousePos;

            Instantiate(grabSO);
        }
    }


    private void HandleMouseRelease()
    {
        if (!bDragging) return;

        bDragging = false;

        if (inRangeJoint != null && otherInRangeJoint != null &&
            !inRangeJoint.bIsSnapped && !otherInRangeJoint.bIsSnapped)
        {
            SnapToBone(inRangeJoint, otherInRangeJoint);
        }

        connectedBones.Clear();

        Instantiate(dropSO);
    }




    //upodate joint from another for bone trigger
    public void SetSnapInRange(BoneJointTrigger myJoint, BoneJointTrigger otherJoint)
    {
        inRangeJoint = myJoint;
        otherInRangeJoint = otherJoint;
    }

    public void ClearSnapInRange(BoneJointTrigger myJoint, BoneJointTrigger otherJoint)
    {
        if (inRangeJoint == myJoint && otherInRangeJoint == otherJoint)
        {
            inRangeJoint = null;
            otherInRangeJoint = null;
        }
    }

    private void HandleDragging()
    {
        lastMousePos = currentMousePos;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        currentMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 displacement = currentMousePos - lastMousePos;

        connectedBones = GetConnectedBones();

        foreach (var fossil in connectedBones)
        {
            fossil.transform.position += displacement;
        }

        RotateBones();

        if (closestBone != null && closestSnapPoint != null)
        {    
            float distance = Vector2.Distance(closestJoint.position, closestSnapPoint.position);
            if (distance < snapDistance) 
            {
                SnapToBone(inRangeJoint, otherInRangeJoint);
            }
        }
/*        else
        {
            foreach (var fossil in connectedBones)
            {
                fossil.transform.position += displacement;
            }
        }*/
    }

    HashSet<FossilConstruction> GetConnectedBones()
    {
        HashSet<FossilConstruction> connected = new HashSet<FossilConstruction>();
        Queue<FossilConstruction> queue = new Queue<FossilConstruction>();
        queue.Enqueue(this);

        GameObject[] allBones = GameObject.FindGameObjectsWithTag("Bone");

        while (queue.Count > 0)
        {
            FossilConstruction current = queue.Dequeue();
            if (!connected.Add(current)) continue;

            foreach (GameObject boneObj in allBones)
            {
                if (boneObj == current.gameObject) continue;

                FossilConstruction otherFossil = boneObj.GetComponent<FossilConstruction>();
                if (otherFossil == null) continue;

                if (current.IsConnectedTo(otherFossil)) 
                {
                    queue.Enqueue(otherFossil);
                }
            }
        }

        return connected;
    }
    bool IsConnectedTo(FossilConstruction other)
    {
        HingeJoint2D[] joints = GetComponents<HingeJoint2D>();
        foreach (var joint in joints)
        {
            if (joint.connectedBody == other.GetComponent<Rigidbody2D>()) return true;
        }

        HingeJoint2D[] otherJoints = other.GetComponents<HingeJoint2D>();
        foreach (var joint in otherJoints)
        {
            if (joint.connectedBody == rb) return true;
        }

        return false;
    }


    public bool bIsInTray()
    {
        return buildingTray != null && buildingTray.bBoneInTray(gameObject);
    }



    private void SnapToBone(BoneJointTrigger closestJoint, BoneJointTrigger closestSnapPoint)
    {
        if(!buildingTray.bBoneInTray(gameObject))
        {
             return; 
        }

        Transform myJoint = closestJoint.transform;
        Transform otherJoint = closestSnapPoint.transform;

        transform.position += (Vector3)(otherJoint.position - myJoint.position);

        activeJoint = gameObject.AddComponent<HingeJoint2D>();
        activeJoint.connectedBody = closestSnapPoint.parentBone.GetComponent<Rigidbody2D>();

        activeJoint.autoConfigureConnectedAnchor = false;
        activeJoint.anchor = transform.InverseTransformPoint(myJoint.position);
        activeJoint.connectedAnchor = closestSnapPoint.parentBone.transform.InverseTransformPoint(otherJoint.position);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        closestSnapPoint.parentBone.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        closestJoint.JointSnapped();
        closestSnapPoint.JointSnapped();

        //audio hooray!
        Instantiate(connectSO);
        bHasSnapped = true;
    }

    private void RotateBones()
    {
        if (!bDragging) return;

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1f; 
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1f; 
        }

        if (rotationInput != 0f)
        {
            foreach (var fossil in connectedBones)
            {
                fossil.transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);
            }
        }
    }

    void DetachBone()
    {
        if (activeJoint != null)
        {
            if (activeJoint.connectedBody != null)
            {
                activeJoint.connectedBody.constraints = RigidbodyConstraints2D.None;
                activeJoint.connectedBody.freezeRotation = true;
            }

            Destroy(activeJoint);
            activeJoint = null;
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;

        if (inRangeJoint != null) inRangeJoint.JointUnsnapped();
        if (otherInRangeJoint != null) otherInRangeJoint.JointUnsnapped();

        inRangeJoint = null;
        otherInRangeJoint = null;
        bHasSnapped = false;

        foreach (var joint in GetComponents<HingeJoint2D>())
        {
            Destroy(joint);
        }

        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        connectedBones.Clear();

        //audio stuff yippee!
        Instantiate(disconnectSO);
    }
}