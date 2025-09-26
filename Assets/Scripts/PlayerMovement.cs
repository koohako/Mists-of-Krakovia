using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction dashAction;
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashDistance = -5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool canDash = true;
    private bool isDashing = false;

    private Vector2 moveDirection;

    public bool IsDashing => isDashing;
    public bool CanDash => canDash;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Sprint");
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (dashAction.triggered && canDash && moveDirection != Vector2.zero)
            {
                StartCoroutine(PerformDash());
            } else if (!isDashing)
            {
                Move();
            }
        }
    }

    void Move()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        
        // Posição inicial e final
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (moveDirection * dashDistance);
        
        // Verifica se o dash não vai colidir com algo (opcional)
        // RaycastHit2D hit = Physics2D.Raycast(startPosition, moveDirection, dashDistance);
        // if (hit.collider != null)
        // {
        //     endPosition = hit.point - (moveDirection * 0.5f); // Para um pouco antes da colisão
        // }
        
        float elapsedTime = 0f;
        
        // Animação suave do dash
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / dashDuration;
            
            // Curva de animação (começa rápido, desacelera no final)
            float easedProgress = 1 - Mathf.Pow(1 - progress, 3);
            
            Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, easedProgress);
            rb.MovePosition(currentPosition);
            
            yield return null;
        }
        
        // Garante que chegue na posição final
        rb.MovePosition(endPosition);
        isDashing = false;
        
        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
