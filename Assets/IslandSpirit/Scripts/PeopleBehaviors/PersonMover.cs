using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PersonMover : MonoBehaviour {

    public float walkSpeed;

    [SerializeField]
    private CharacterController cc;
    [SerializeField]
    private Animator animator;


    

    private void Update()
    {
        Vector3 lookDir = cc.velocity;
        lookDir.y = 0;
        if (lookDir.magnitude > 0)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, lookDir.normalized, 1000f, 1000f);
        }
        if (lookDir.magnitude > 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    public void Walk(Vector3 dir)
    {
        cc.SimpleMove(dir * walkSpeed);
    }

}
