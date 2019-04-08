using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float stickToGroundForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private CharacterRotationController rotationController;

    private CharacterController characterController;    
    private new Camera camera;    
    private CollisionFlags collisionFlags;
    private bool mustJump;
    private bool isRunning;
    private bool isJumping;
    private bool previouslyGrounded;
    private Vector3 previousMovementDelta=Vector3.zero;
    private Vector3 movementDelta=Vector3.zero;
    


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        rotationController.InitializeRotationController(transform, camera.transform);
    }

    private void Update()
    {
        rotationController.RotateCharacterAndCamera();

        CatchJumpInput();
        CheckJumpState();        
    }   
    
    private void CatchJumpInput()
    {
        if (!mustJump)
        {
            mustJump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }

    private void CheckJumpState()
    {
        if (!previouslyGrounded && characterController.isGrounded)
        {
            isJumping = false;
        }
        previouslyGrounded = characterController.isGrounded;
    }

    private void FixedUpdate()
    {
        Vector2 movementInputRaw = GetCrossPlatformMovementInput();

        Vector2 normalizedInput = NormalizeInput(movementInputRaw);

        MakeMovementDeltaRelativeToCamForward(normalizedInput);

        ProjectMovementDeltaOnGround();

        AddSpeedToMovementDelta();

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
        if (input.sqrMagnitude > 1)
        {
            Vector2 normInput = input;
            normInput.Normalize();
            return normInput;
        }
        else
        {
            return input;
        }
    }

    private void MakeMovementDeltaRelativeToCamForward(Vector2 movementInput)
    {
        movementDelta = transform.forward * movementInput.y + transform.right * movementInput.x;        
    }

    private void ProjectMovementDeltaOnGround()
    {
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
            characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

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

    private void AddJumpForceToMovementDelta()
    {    
        if (characterController.isGrounded)
        {          
            movementDelta.y = -stickToGroundForce;

            if (mustJump)
            {                
                movementDelta.y = jumpForce;
                mustJump = false;
                isJumping = true;
            }
        }
        else
        {
            movementDelta = previousMovementDelta+Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;            
        }
        previousMovementDelta = movementDelta;  
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rBody = hit.collider.attachedRigidbody;

        if (collisionFlags == CollisionFlags.Below || rBody==null || rBody.isKinematic)
        {
            return;
        }
        rBody.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);        
    }
}
