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

    private int terrainX;
    private int terrainY;

    

    private void Start()
    {
        Vector2 tpos = GodController.Instance.GetTerrainPos(transform);
        terrainX = (int)tpos.x;
        terrainY = (int)tpos.y;
    }

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
        Vector2 tpos = GodController.Instance.GetTerrainPos(transform);
        if(tpos.x != terrainX || tpos.y != terrainY)
        {
            GodController.Instance.UpdadePlacedObjectLocation(gameObject, terrainX, terrainY);
            terrainX = (int)tpos.x;
            terrainY = (int)tpos.y;
        }
    }

}
