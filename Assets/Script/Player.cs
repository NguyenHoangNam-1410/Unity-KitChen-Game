using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    [SerializeField] private float rotationSpeed = 14f;

    [SerializeField] private GameInput gameInput;


    private bool isWalking;

    private void Update()
    {
        HandleMovement();
        HandleInteraction();

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        // Handle interaction logic here

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f; // Adjust this value based on your interaction range

        Physics.Raycast(transform.position, moveDirection, out RaycastHit raycastHit, interactDistance);

    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.7f; // Adjust this value based on your player size

        float playerHeight = 2f; // Adjust this value based on your player height

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            //Attemp only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                //Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {

                }
            }

        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero; // Check if the player is walking
        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime); // Normalize only for rotation
        }
    }
}
