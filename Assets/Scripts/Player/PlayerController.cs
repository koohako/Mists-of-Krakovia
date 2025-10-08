using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving,
        Dashing,
        Attacking,
        Dead
    }

    [SyncVar(hook = nameof(OnStateChanged))]
    private PlayerState currentState = PlayerState.Idle;

    private PlayerMovement playerMovement;
    private InputAction moveAction;
    private Rigidbody2D rb;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        // Atualiza o estado com base no input
        if (currentState != PlayerState.Dashing && currentState != PlayerState.Attacking && currentState != PlayerState.Dead)
        {
            if (moveAction.GetControlMagnitude() == 0f)
                SetState(PlayerState.Idle);
            else
                SetState(PlayerState.Moving);
        }


        switch (currentState)
        {
            case PlayerState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerState.Dashing:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerState.Attacking:
                // Lógica para Atacando
                break;
            case PlayerState.Dead:
                // Lógica para Morto
                break;
        }

    }
    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (currentState == PlayerState.Moving)
        {
            Vector2 input = GetMoveDirection();
            playerMovement.Move(input);
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public void SetState(PlayerState newState)
    {
        currentState = newState;
        // Aqui você pode adicionar lógica ao entrar em um novo estado
    }

    public PlayerState GetState()
    {
        return currentState;
    }

    public void OnStateChanged(PlayerState oldState, PlayerState newState)
    {
        SetState(newState);
    }
}
