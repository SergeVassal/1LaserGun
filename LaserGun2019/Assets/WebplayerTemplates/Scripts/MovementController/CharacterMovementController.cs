

public class CharacterMovementController : MonoBehaviour
{
    



    private void Update()
    {        

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

        rotationController.UpdateCursorLock();
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
