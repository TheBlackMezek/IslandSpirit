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

    private float camRotX = 0;
    private float camRotY = 0;
    private float camDist;

    private CharacterController cc;
    private Animator animator;



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
        cc.SimpleMove(camera.forward * Input.GetAxis("ForeBack") * Time.deltaTime * moveSpeed
                    + camera.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed);
        Vector3 lookDir = cc.velocity;
        lookDir.y = 0;
        transform.forward = Vector3.RotateTowards(transform.forward, lookDir.normalized, 1000f, 1000f);
        if(lookDir.magnitude > 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        //transform.position += transform.forward * Input.GetAxis("ForeBack") * Time.deltaTime * moveSpeed
        //                    + transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

        camera.position = transform.position + Vector3.up * heightOffset;

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            camRotY +=  Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            camRotX += -Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
            Mathf.Clamp(camRotX, -90f, 90f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        camera.eulerAngles = new Vector3(camRotX, camRotY, camera.eulerAngles.z);

        camera.position -= camera.forward * camDist;
    }

}
