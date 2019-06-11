using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private CharacterRotationController rotationController;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float stickToGroundForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;

    private CharacterController characterController;
    private new Camera camera;
    private Vector3 movementDelta=Vector3.zero;
    private Vector3 previousMovementDelta = Vector3.zero;    
    private bool isJumpPressed;
    private bool isJumpPressedDuringFixedUpdate;    
    private bool hasJumpedDuringThisUpdate;    
    private bool isRunning;
    private bool isCloseToGround;
    private bool isGrounded;
    private CollisionFlags collisionFlags;
    private const float CHARACTER_COLLISION_FORCE=0.1f;
    private RaycastHit hitInfo;
    


    private void Start()
    {      
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        rotationController.Initialize(transform, camera.transform);
    }

    private void Update()
    {
        rotationController.RotateCharacterAndCamera();        

        GetJumpInput();        
    }

    private void GetJumpInput()
    {
        isJumpPressed = CrossPlatformInputManager.GetButtonDown("Jump");
        
        if (isJumpPressed&&isCloseToGround)
        {            
            isJumpPressedDuringFixedUpdate = true;
            hasJumpedDuringThisUpdate = false;
        }
    }


    private void FixedUpdate()
    {   
        Vector2 movementInputRaw = GetCrossPlatformMovementInput();        
        Vector2 normalizedInput = NormalizeInput(movementInputRaw);
        MakeMovementDeltaRelativeToCamForward(normalizedInput);
        CheckIfCloseToGround();
        ProjectMovementDeltaOnGround();        
        AddSpeedToMovementDelta();
        CheckIfGrounded();
        AddJumpForceToMovementDelta();
        collisionFlags = characterController.Move(movementDelta * Time.fixedDeltaTime);        
    }

    private Vector2 GetCrossPlatformMovementInput()
    {
        float inputHorizontalRaw = CrossPlatformInputManager.GetAxis("Horizontal");
        float inputVerticalRaw = CrossPlatformInputManager.GetAxis("Vertical");

        return new Vector2(inputHorizontalRaw, inputVerticalRaw);
    }

    private Vector2 NormalizeInput(Vector2 input)
    {
        Vector2 normInput = input;
        normInput.Normalize();
        return normInput;
    }

    private void MakeMovementDeltaRelativeToCamForward(Vector2 movementInput)
    {
        movementDelta = transform.forward * movementInput.y + transform.right * movementInput.x;
    }

    public void CheckIfCloseToGround()
    {
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down,
            out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            isCloseToGround = true;
        }
        else
        {
            isCloseToGround = false;
        }        
    }

    private void ProjectMovementDeltaOnGround()
    {                
        movementDelta = Vector3.ProjectOnPlane(movementDelta, hitInfo.normal).normalized;
    }

    private void AddSpeedToMovementDelta()
    {
        float speed = GetMovementSpeed();

        movementDelta = new Vector3(movementDelta.x * speed, 0f, movementDelta.z * speed);
    }

    private float GetMovementSpeed()
    {
#if !MOBILE_INPUT
        isRunning = Input.GetKey(KeyCode.LeftShift);
#endif
        return isRunning ? runSpeed : walkSpeed;
    }

    private void CheckIfGrounded()
    {
        isGrounded = characterController.isGrounded;
    }

    private void AddJumpForceToMovementDelta()
    {
        if (isGrounded)
        {
            movementDelta.y = -stickToGroundForce;

            if (isJumpPressedDuringFixedUpdate && !hasJumpedDuringThisUpdate)
            {
                movementDelta.y = jumpForce;
                hasJumpedDuringThisUpdate = true;
                isJumpPressedDuringFixedUpdate = false;
                isJumpPressed = false;
            }
        }
        else
        {
            movementDelta.y = previousMovementDelta.y + Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }
        previousMovementDelta = movementDelta;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rBody = hit.collider.attachedRigidbody;

        if (collisionFlags == CollisionFlags.Below || rBody == null || rBody.isKinematic)
        {
            return;
        }
        rBody.AddForceAtPosition(characterController.velocity * CHARACTER_COLLISION_FORCE, hit.point, ForceMode.Impulse);
    }
}
