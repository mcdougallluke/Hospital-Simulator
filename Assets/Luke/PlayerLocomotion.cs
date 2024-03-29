using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRididbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeightOffset = 0.5f;
    public float maxDistance = 1;
    public LayerMask groundLayer;
    
    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    
    public bool activeGrapple;
    
    private void Awake() {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRididbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;
        
        HandleMovement();
        HandleRotation();
    }
    
    private void HandleMovement()
    {
        if (activeGrapple) return;

        // Calculate the movement direction based on input and camera orientation
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0; // Ensure movement is only horizontal

        // Determine the speed based on sprinting status and input magnitude
        float currentSpeed = isSprinting ? sprintSpeed : (inputManager.moveAmount >= 0.5f ? runningSpeed : walkingSpeed);

        // Modify moveDirection with the current speed
        moveDirection *= currentSpeed;

        // If the player is grounded or jumping (but not just falling without a jump), apply horizontal movement
        if (isGrounded || isJumping)
        {
            Vector3 movementVelocity = new Vector3(moveDirection.x, playerRididbody.velocity.y, moveDirection.z);
            playerRididbody.velocity = movementVelocity;
        }
        else if (!isGrounded)
        {
            Vector3 airMovementVelocity = new Vector3(moveDirection.x * currentSpeed, playerRididbody.velocity.y, moveDirection.z * currentSpeed);
            playerRididbody.velocity = Vector3.Lerp(playerRididbody.velocity, airMovementVelocity, Time.deltaTime * leapingVelocity);
        }
    }

    
    private void HandleRotation()
    {
        if (isJumping || !isGrounded)
            return; // Skip rotation if jumping or not grounded

        Vector3 targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        
        // Only apply rotation if grounded and not jumping
        if (isGrounded && !isJumping)
        {
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }


    private void HandleFallingAndLanding() 
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + raycastHeightOffset;

        if (!isGrounded && !isJumping) {
            if (!playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Fall", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRididbody.AddForce(transform.forward * leapingVelocity);
            playerRididbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer)) {
            if (!isGrounded && !playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Land", true);
            }
            Vector3 rayCastHitPoint = hit.point;
            inAirTimer = 0;
            isGrounded = true;
        } else {
            isGrounded = false;
        }

    }

    public void HandleJumping() {
        if (isGrounded) {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRididbody.velocity = playerVelocity;
        }
    }


    private Vector3 velocityToSet;

    bool enableMovementOnNextTouch;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        Debug.Log("Setting velocity to: " + velocityToSet); // Add this line
        playerRididbody.velocity = velocityToSet;
    }


    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        //velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        velocityToSet = new Vector3(5, 5, 5);
        Invoke(nameof(SetVelocity), 0.1f);
        //Invoke(nameof(ResetRestrictions), 3f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponentInChildren<Grappling>().StopGrapple();
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        Debug.Log("Displacement XZ: " + displacementXZ + ", Velocity XZ: " + velocityXZ); // Add this line
        return velocityXZ + velocityY;
    }

}