using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float movementSpeed = 12f;
    public float gravity = -9.81f;
    public bool isGrounded;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= transform.localScale.y && transform.position.y <= (transform.localScale.y + groundDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetMovementSpeed(float theSpeed)
    {
        movementSpeed = theSpeed;
    }
}
