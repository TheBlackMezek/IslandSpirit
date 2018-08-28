using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Transform camera;

    public float lookSensitivity;
    public float initialCamDist;
    public float heightOffset;
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;

    private float camRotX = 0;
    private float camRotY = 0;
    private float camDist;
    private float yvel = 0;

    private CharacterController cc;
    private Animator animator;

    private bool onGround = false;



    private void OnValidate()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        camDist = initialCamDist;
    }

    private void Update()
    {
        if(cc.isGrounded)
        {
            onGround = true;
            yvel = 0;
        }
        else
        {
            yvel += Time.deltaTime * gravity;
        }

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onGround = false;
            yvel = jumpSpeed;
            animator.SetTrigger("Jump");
        }


        Vector3 moveVec = (camera.forward * Input.GetAxis("ForeBack")
              + camera.right * Input.GetAxis("Horizontal")).normalized * Time.deltaTime * moveSpeed;
        moveVec.y = yvel;
        cc.Move(moveVec);
        
        Vector3 lookDir = cc.velocity;
        lookDir.y = 0;
        if(lookDir.magnitude > 0)
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
        
        camera.position = transform.position + Vector3.up * heightOffset;

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            camRotY +=  Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            camRotX += -Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
            camRotX = Mathf.Clamp(camRotX, -90f, 90f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        camera.eulerAngles = new Vector3(camRotX, camRotY, camera.eulerAngles.z);

        camera.position -= camera.forward * camDist;
    }

}
