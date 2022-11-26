using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{

    [Header("Settings")]
    [Tooltip("The speed at which player moves")]
    public float moveSpeed = 2f;
    [Tooltip("The speed at which player rotates to look and right (calculated in degrees")]
    public float lookSpeed = 60f;
    [Tooltip("The power with which player jump")]
    public float jumpPower = 8f;
    [Tooltip("The strength of a gravity")]
    public float gravity = 9.81f;

    [Header("Jump Timing")]
    public float jumpTimeLeniecy = 0.1f;
    private float timeToStopBeingLeniet = 0;

    [Header("Required Reference")]
    [Tooltip("The player shooter script that fires projectiles")]
    public Shooter playerShooter;

    public Health playerHealth;
    public List<GameObject> disableWhileDead;

    private bool doubleJumpingAvaleble = false;

    //The character controller component on the player
    private CharacterController controller;
    private InputManager inputManager;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetUpCharacterController();
        SetUpInputManager();
    }

    private void SetUpCharacterController()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.Log("The player controller script dose note have a characnter controller on the same game object");
        }
    }

    private void SetUpInputManager()
    {
        inputManager = InputManager.instance;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
            foreach(GameObject inGO in disableWhileDead)
            {
                inGO.SetActive(false);
            }
            return;
        }
        else
        {
            foreach(GameObject inGO in disableWhileDead)
            {
                inGO.SetActive(true);
            }
        }

        ProcessMovement();
        ProcessRotation();
    }

    Vector3 moveDerection;
    private void ProcessMovement()
    {
        // Get the input form the input manager
        float leftRightInput = inputManager.horizontalMoveAxis;
        float forwardBackwardInput = inputManager.verticalMoveAxis;
        bool jumpPressed = inputManager.jumpPressed;

        // Handle the control of the player while it is on the ground
        if (controller.isGrounded)
        {
            doubleJumpingAvaleble = true;
            timeToStopBeingLeniet = Time.time + jumpTimeLeniecy;

            // Set the movement direction to be the received input, set y to 0 since are on the ground
            moveDerection  = new Vector3(leftRightInput,0,forwardBackwardInput);
            // Set the move direction in relation to the transform
            moveDerection = transform.TransformDirection(moveDerection);
            moveDerection *= moveSpeed;

            if (jumpPressed)
            {
                moveDerection.y = jumpPower;
            }
        }
        else
        {
            moveDerection = new Vector3(leftRightInput * moveSpeed, moveDerection.y, forwardBackwardInput * moveSpeed);
            moveDerection = transform.TransformDirection(moveDerection);

            if(jumpPressed && Time.time < timeToStopBeingLeniet)
            {
                moveDerection.y = jumpPower;
            }
            else if(jumpPressed && doubleJumpingAvaleble)
            {
                moveDerection.y = jumpPower;
                doubleJumpingAvaleble = false;
            }
        }

        moveDerection.y -= gravity * Time.deltaTime;

        if(controller.isGrounded && moveDerection.y < 0)
        {
            moveDerection.y = -0.3f;
        }

        controller.Move(moveDerection * Time.deltaTime);
    }

    private void ProcessRotation()
    {
        float horizontalLookInput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime,
            playerRotation.z));
    }
}
