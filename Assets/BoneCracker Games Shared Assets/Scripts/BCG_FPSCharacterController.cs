//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BCG_FPSCharacterController : MonoBehaviour {

    public bool canControl = true;

    /// <summary>
    /// Inputs to control the character by feeding inputMovementX and inputMovementY.
    /// </summary>
    public BCG_Inputs inputs;

    /// <summary>
    /// Move inputs.
    /// </summary>
    private float inputMoveX = 0f;
    private float inputMoveZ = 0f;

    /// <summary>
    /// Aim inputs.
    /// </summary>
    private float inputMouseX = 0f;
    private float inputMouseY = 0f;

    /// <summary>
    /// Movement speed
    /// </summary>
    public float moveSpeed = 5f;

    /// <summary>
    /// Jump force
    /// </summary>
    public float jumpForce = 5f;

    /// <summary>
    /// Mouse sensitivity
    /// </summary>
    public float mouseSensitivity = 2f;

    /// <summary>
    /// Reference to the camera's transform
    /// </summary>
    public Transform cameraTransform;

    /// <summary>
    /// Rigidbody.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Vertical look rotation.
    /// </summary>
    private float verticalLookRotation;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating

    }

    private void OnEnable() {

        inputMoveX = 0f;
        inputMoveZ = 0f;

        inputMouseX = 0f;
        inputMouseY = 0f;

    }

    private void OnDisable() {

        inputMoveX = 0f;
        inputMoveZ = 0f;

        inputMouseX = 0f;
        inputMouseY = 0f;

    }

    private void Update() {

        Inputs();

        transform.Rotate(Vector3.up * inputMouseX * Time.deltaTime * 200f); // Rotate the player horizontally

        verticalLookRotation -= inputMouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -70f, 70f); // Clamp the vertical rotation
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f); // Rotate the camera vertically

    }

    private void Inputs() {

        inputs = BCG_InputManager.Instance.GetInputs();

        if (canControl) {

            //  Receive keyboard inputs if controller type is not mobile. If controller type is mobile, inputs will be received by BCG_MobileCharacterController component attached to FPS/ TPS Controller UI Canvas.
            if (!BCG_EnterExitSettings.Instance.mobileController) {

                //	X and Y inputs based "Vertical" and "Horizontal" axes.
                inputMoveX = inputs.horizonalInput;
                inputMoveZ = inputs.verticalInput;

                // Handle mouse look
                inputMouseX = inputs.aim.x * mouseSensitivity;
                inputMouseY = inputs.aim.y * mouseSensitivity;

                // Handle jumping
                if (Input.GetButtonDown("Jump") && IsGrounded())
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            } else {

                //	X and Y inputs based "Vertical" and "Horizontal" axes.
                inputMoveX = BCG_MobileCharacterController.move.x;
                inputMoveZ = BCG_MobileCharacterController.move.y;

                // Handle mouse look
                inputMouseX = BCG_MobileCharacterController.mouse.x * mouseSensitivity;
                inputMouseY = BCG_MobileCharacterController.mouse.y * mouseSensitivity;

            }

        } else {

            inputMoveX = 0f;
            inputMoveZ = 0f;

            inputMouseX = 0f;
            inputMouseY = 0f;

        }

    }

    private void Motor() {

        Vector3 move = transform.right * inputMoveX + transform.forward * inputMoveZ;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

    }

    private void FixedUpdate() {

        Motor();

    }

    public bool IsGrounded() {

        Debug.DrawRay(transform.position, -Vector3.up * 1.1f, Color.white);

        // Simple check if the player is grounded (can be expanded with more complex ground checks)
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }

}

