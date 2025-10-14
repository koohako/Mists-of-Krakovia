using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveDirection)
    {
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }
}