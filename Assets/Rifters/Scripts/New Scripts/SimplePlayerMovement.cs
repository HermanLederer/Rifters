using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : NetworkBehaviour
{

    [SerializeField] private GameObject cameraRig = null;

    public CharacterController characterController;

    void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        cameraRig.SetActive(true);
    }

    private void OnDisable()
    {
        if (hasAuthority)
        {
            cameraRig.SetActive(false);
        }
    }

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float turnSensitivity = 5f;
    public float maxTurnSpeed = 150f;

    [Header("Diagnostics")]
    public float horizontal;
    public float vertical;
    public float turn;
    public float jumpSpeed;
    public bool isGrounded = true;
    public bool isFalling;
    public Vector3 velocity;

    void Update()
    {
        if (!hasAuthority)
            return;

        horizontal = Input.GetAxis("Horizontal P1");
        vertical = Input.GetAxis("Vertical P1");

        // Q and E cancel each other out, reducing the turn to zero
        if (Input.GetKey(KeyCode.Q))
            turn = Mathf.MoveTowards(turn, -maxTurnSpeed, turnSensitivity);
        if (Input.GetKey(KeyCode.E))
            turn = Mathf.MoveTowards(turn, maxTurnSpeed, turnSensitivity);
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
            turn = Mathf.MoveTowards(turn, 0, turnSensitivity);
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
            turn = Mathf.MoveTowards(turn, 0, turnSensitivity);

        if (isGrounded)
            isFalling = false;

        if ((isGrounded || !isFalling) && jumpSpeed < 1f && Input.GetKey(KeyCode.Space))
        {
            jumpSpeed = Mathf.Lerp(jumpSpeed, 1f, 0.5f);
        }
        else if (!isGrounded)
        {
            isFalling = true;
            jumpSpeed = 0;
        }
    }

    void FixedUpdate()
    {
        if (!hasAuthority || characterController == null)
            return;

        transform.Rotate(0f, turn * Time.fixedDeltaTime, 0f);

        Vector3 direction = new Vector3(horizontal, jumpSpeed, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);
        direction = transform.TransformDirection(direction);
        direction *= moveSpeed;

        if (jumpSpeed > 0)
            characterController.Move(direction * Time.fixedDeltaTime);
        else
            characterController.SimpleMove(direction);

        isGrounded = characterController.isGrounded;
        velocity = characterController.velocity;
    }
}
