using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoneJointTrigger : MonoBehaviour
{
    public FossilConstruction parentBone;
    public bool bIsSnapped = false;

    private SpriteRenderer spriteRenderer;
    private Color jointColour;

    private void Start()
    {
        if (parentBone == null)
        {
            parentBone = GetComponentInParent<FossilConstruction>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            jointColour = spriteRenderer.color;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bIsSnapped) return; 

        if (other.CompareTag("BoneJoint"))
        {
            BoneJointTrigger otherTrigger = other.GetComponent<BoneJointTrigger>();

            if (parentBone.bIsInTray() && otherTrigger.parentBone.bIsInTray())
            {
                parentBone.SetSnapInRange(this, otherTrigger);
                HighlightJoint(true);
                otherTrigger.HighlightJoint(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (bIsSnapped) return;

        if (other.CompareTag("BoneJoint"))
        {
            BoneJointTrigger otherTrigger = other.GetComponent<BoneJointTrigger>();

            if (otherTrigger != null && parentBone != null)
            {
                parentBone.ClearSnapInRange(this, otherTrigger);
                HighlightJoint(false);
                otherTrigger.HighlightJoint(false);
            }
        }
    }

    public void JointSnapped()
    {
        bIsSnapped = true;
        HighlightJoint(false);
    }

    public void JointUnsnapped()
    {
        bIsSnapped = false;
        HighlightJoint(false);
    }

    public void HighlightJoint(bool highlight)
    {
        if (spriteRenderer == null) return;

        if (highlight)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = jointColour;
        }
    }
}
