using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera)), RequireComponent(typeof(CharacterController))]
public class SimplePlayerController : MonoBehaviour
{   
    
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera playerCamera;

    #region Variables
    [Header("Walking / Sprinting")]
    [SerializeField] [Range(1f, 20f)] private float moveSpeed = 7.5f;
    [SerializeField] [Range(1f, 40f)] private float sprintSpeed = 11.5f;

    
    [SerializeField] [Range(1f, 20f)] private float jumpSpeed = 8.0f;
    [SerializeField] [Range(1f, 60f)] private float gravity = 20.0f;

    [Header("Camera")]
    [SerializeField] [Range(1f, 10f)] private float lookSpeed = 2.0f;
    [SerializeField] [Range(1f, 90f)]private float lookXLimit = 75.0f;

    [Header("Settings")]
    [SerializeField] private bool allowJump = true;
    [SerializeField] private bool allowSprinting = true;

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    #endregion

    #region Code

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector] public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        // Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(sprintKey) && allowSprinting;
        float curSpeedX = canMove ? (isRunning ? sprintSpeed : moveSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? sprintSpeed : moveSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetKey(jumpKey) && canMove && characterController.isGrounded && allowJump)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
    #endregion
}
