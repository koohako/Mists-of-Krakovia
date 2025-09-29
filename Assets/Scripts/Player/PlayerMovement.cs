using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody2D rb;
    InputAction moveAction;
    public float moveSpeed = 5f;
    private Vector2 moveDirection;
    private bool isDashing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (!isDashing)
            {
                Move();
            }else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Move()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SetIsDashing(bool value)
    {
        isDashing = value;
    }

    public bool GetDashing()
    {
        return isDashing;
    }

}
