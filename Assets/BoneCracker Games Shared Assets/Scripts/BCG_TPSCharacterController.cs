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
public class BCG_TPSCharacterController : MonoBehaviour {

    public bool canControl = true;

    /// <summary>
    /// Inputs to control the character by feeding inputMovementX and inputMovementY.
    /// </summary>
    public BCG_Inputs inputs;

    private float inputMoveX = 0f;
    private float inputMoveZ = 0f;

    /// <summary>
    /// Movement speed
    /// </summary>
    public float moveSpeed = 5f;

    /// <summary>
    /// Jump force
    /// </summary>
    public float jumpForce = 5f;

    /// <summary>
    /// Character rotation speed
    /// </summary>
    public float rotationSpeed = 10f;

    /// <summary>
    /// Reference to the camera's transform
    /// </summary>
    public Transform cameraTransform;

    private Rigidbody rb;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating

    }

    private void OnEnable() {

        inputMoveX = 0f;
        inputMoveZ = 0f;

    }

    private void OnDisable() {

        inputMoveX = 0f;
        inputMoveZ = 0f;

    }

    private void Update() {

        Inputs();
        HandleRotation();

    }

    private void Inputs() {

        inputs = BCG_InputManager.Instance.GetInputs();

        if (canControl) {

            //  Receive keyboard inputs if controller type is not mobile. If controller type is mobile, inputs will be received by BCG_MobileCharacterController component attached to FPS/ TPS Controller UI Canvas.
            if (!BCG_EnterExitSettings.Instance.mobileController) {

                // Handle movement
                inputMoveX = inputs.horizonalInput;
                inputMoveZ = inputs.verticalInput;

                // Handle jumping
                if (Input.GetButtonDown("Jump") && IsGrounded())
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            } else {

                // Handle movement
                inputMoveX = BCG_MobileCharacterController.move.x;
                inputMoveZ = BCG_MobileCharacterController.move.y;

            }

        } else {

            inputMoveX = 0f;
            inputMoveZ = 0f;

        }

    }

    private void Motor() {

        Vector3 move = cameraTransform.right * inputMoveX + cameraTransform.forward * inputMoveZ;
        move.y = 0f;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

    }

    private void FixedUpdate() {

        Motor();

    }

    private void HandleRotation() {

        // Rotate the character to face the direction of the camera
        if (inputMoveX != 0 || inputMoveZ != 0) {

            Vector3 direction = new Vector3(inputMoveX, 0f, inputMoveZ).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.forward * direction.z + cameraTransform.right * direction.x);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

    }

    public bool IsGrounded() {

        Debug.DrawRay(transform.position, -Vector3.up * 1.1f, Color.white);
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }

}
