using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float stickToGroundForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private RotationController rotationController;

    private CharacterController characterController;
    private new Camera camera;
    private CollisionFlags collisionFlags;
    private bool isWalking;
    private bool mustJump;
    private bool isJumping;
    private bool previouslyGrounded;
    private Vector2 input;
    private Vector3 moveDir = Vector3.zero;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        rotationController = new RotationController();
        rotationController.Init(transform, camera.transform);
    }


    private void Update()
    {
        RotateCharacterAndCamera();

        // the jump state needs to read here to make sure it is not missed
        if (!mustJump)
        {
            mustJump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!previouslyGrounded && characterController.isGrounded)
        {  
            moveDir.y = 0f;
            isJumping = false;
        }
        if (!characterController.isGrounded && !isJumping && previouslyGrounded)
        {
            moveDir.y = 0f;
        }

        previouslyGrounded = characterController.isGrounded;
    }


    private void FixedUpdate()
    {      
        float speed;
        GetInput(out speed);

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                           characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;


        if (characterController.isGrounded)
        {
            moveDir.y = -stickToGroundForce;

            if (mustJump)
            {
                moveDir.y = jumpSpeed;                
                mustJump = false;
                isJumping = true;
            }
        }
        else
        {
            moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        collisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);

        rotationController.UpdateCursorLock();
    }


    private void RotateCharacterAndCamera()
    {
        rotationController.ApplyRotation(transform, camera.transform);
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = isWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = isWalking ? walkSpeed : runSpeed;
        input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

}
