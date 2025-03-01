//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;

public class BCG_TPSCameraController : MonoBehaviour {

    public BCG_TPSCharacterController target; // The target the camera will follow (usually the player)

    /// <summary>
    /// Inputs to control the character by feeding inputMovementX and inputMovementY.
    /// </summary>
    public BCG_Inputs inputs;

    /// <summary>
    /// Offset from the target
    /// </summary>
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    /// <summary>
    /// Speed of rotation around the target
    /// </summary>
    public float rotationSpeed = 5f;

    /// <summary>
    /// Distance from the target
    /// </summary>
    public float distance = 10f;

    /// <summary>
    /// Minimum vertical angle of the camera
    /// </summary>
    public float minYAngle = -30f;

    /// <summary>
    /// Maximum vertical angle of the camera
    /// </summary>
    public float maxYAngle = 60f;

    /// <summary>
    /// Smoothing speed for camera movement
    /// </summary>
    public float smoothSpeed = 0.125f;

    private float currentX = 0f;
    private float currentY = 0f;

    private void OnEnable() {

        target = FindFirstObjectByType<BCG_TPSCharacterController>();

        if (!target) {

            // Initialize camera rotation based on the initial offset
            Vector3 thisAngles = transform.eulerAngles;
            currentX = thisAngles.y;
            currentY = thisAngles.x;

            Debug.Log("Target couldn't found on " + gameObject.name + "!");
            return;

        }

        // Initialize camera rotation based on the initial offset
        Vector3 angles = target.transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

    }

    private void Inputs() {

        inputs = BCG_InputManager.Instance.GetInputs();

        //  Receive keyboard inputs if controller type is not mobile. If controller type is mobile, inputs will be received by BCG_MobileCharacterController component attached to FPS/ TPS Controller UI Canvas.
        if (!BCG_EnterExitSettings.Instance.mobileController) {

            // Input for camera rotation
            currentX += inputs.aim.x * rotationSpeed * Time.deltaTime * 200f;
            currentY -= inputs.aim.y * rotationSpeed * Time.deltaTime * 200f;

        } else {

            // Input for camera rotation
            currentX += BCG_MobileCharacterController.mouse.x * rotationSpeed * Time.deltaTime * 200f;
            currentY -= BCG_MobileCharacterController.mouse.y * rotationSpeed * Time.deltaTime * 200f;

        }

        // Clamp the vertical rotation to prevent flipping
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

    }

    private void LateUpdate() {

        if (!target) {

            Debug.Log("Target couldn't found on " + gameObject.name + "!");
            return;

        }

        Inputs();

        // Calculate the desired rotation
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);

        // Calculate the desired position
        Vector3 desiredPosition = target.transform.position + rotation * offset;

        // Smoothly move the camera to the desired position
        transform.position = desiredPosition;

        // Always look at the target
        transform.LookAt(target.transform.position);

    }

}
