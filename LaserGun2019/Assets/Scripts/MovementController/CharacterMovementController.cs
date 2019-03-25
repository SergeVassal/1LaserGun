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
    private bool isRunning;
    private CollisionFlags collisionFlags;
    private bool mustJump;
    private bool isJumping;
    private bool previouslyGrounded;
    private Vector3 previousMovementDelta=Vector3.zero;
    


    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();        
        camera = GetComponentInChildren<Camera>();
        rotationController.InitializeRotationController(transform, camera.transform);
    }

    private void Update()
    {
        rotationController.RotateCharacterAndCamera();
    }    

    private void FixedUpdate()
    {     
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        Vector2 movementInputRaw = GetCrossPlatformMovementInput();

        Vector2 normalizedInput=NormalizeInput(movementInputRaw);

        Vector3 movementDelta = GetMovementDeltaRelativeToCamForward(normalizedInput);

        Vector3 movementDeltaProjectedOnGround = GetMovementDeltaProjectedOnGround(movementDelta);

        Vector3 movementDeltaWithSpeed = GetMovementDeltaWithSpeed(movementDeltaProjectedOnGround);

        Vector3 movementDeltaWithJump = GetMovementDeltaWithJumpForce(movementDeltaWithSpeed);
                
        collisionFlags = characterController.Move(movementDeltaWithJump * Time.fixedDeltaTime);        
    }

    private Vector2 GetCrossPlatformMovementInput()
    {
        float rawInputHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float rawInputVertical = CrossPlatformInputManager.GetAxis("Vertical");  

        return new Vector2(rawInputHorizontal, rawInputVertical);
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

    private Vector3 GetMovementDeltaRelativeToCamForward(Vector2 movementInput)
    {
        Vector3 movementDelta = transform.forward * movementInput.y + transform.right * movementInput.x;
        return movementDelta;
    }

    private Vector3 GetMovementDeltaProjectedOnGround(Vector3 movementDelta)
    {
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
            characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        Vector3 movementDeltaOnGround = Vector3.ProjectOnPlane(movementDelta, hitInfo.normal).normalized;

        return movementDeltaOnGround;
    }

    private Vector3 GetMovementDeltaWithSpeed(Vector3 movementDelta)
    {
        float speed = GetMovementSpeed();

        Vector3 movementDeltaWithSpeed = new Vector3(movementDelta.x * speed, 0f, movementDelta.z * speed);

        return movementDeltaWithSpeed;
    }

    private float GetMovementSpeed()
    {
#if !MOBILE_INPUT
        isRunning = Input.GetKey(KeyCode.LeftShift);
#endif
        return isRunning ? runSpeed : walkSpeed;        
    }

    private Vector3 GetMovementDeltaWithJumpForce(Vector3 movementDelta)
    {
        Vector3 movementDeltaWithJump = movementDelta;
        if (characterController.isGrounded)
        {
            previousMovementDelta = movementDeltaWithJump;
            movementDeltaWithJump.y = -stickToGroundForce;

            if (mustJump)
            {
                movementDeltaWithJump.y = jumpForce;
                mustJump = false;
                isJumping = true;
            }
        }
        else
        {
            
            movementDeltaWithJump = previousMovementDelta+Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            previousMovementDelta = movementDeltaWithJump;
        }

        return movementDeltaWithJump;
    }

}
