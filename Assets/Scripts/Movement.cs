using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    private float ySpeed = 0f;
    private CharacterController ccon;
    public bool isGrounded;

    void Start()
    {
        ccon = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Camera-relative directions
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Get input
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        // Movement relative to camera
        Vector3 moveDirection = (cameraForward * verticalMove + cameraRight * horizontalMove).normalized;

        // Rotate player to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }

        // Ground check
        isGrounded = ccon.isGrounded;

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -0.5f; // small negative to stay grounded
        }

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            ySpeed = jumpSpeed;
        }

        // Gravity
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // Final velocity
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = ySpeed;

        // Move character
        ccon.Move(velocity * Time.deltaTime);
    }
}