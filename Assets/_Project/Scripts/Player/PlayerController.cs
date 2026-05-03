using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Limit")]
    [SerializeField] private float maximumMoveSpeed = 9f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 facingDirection = Vector2.right;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;

    public Vector2 FacingDirection => facingDirection;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadMoveInput();
        UpdateFacingDirection();

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMoveInput()
    {
        moveInput = Vector2.zero;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveInput.y += 1f;

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveInput.y -= 1f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            moveInput.x -= 1f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            moveInput.x += 1f;

        moveInput = moveInput.normalized;
    }

    private void UpdateFacingDirection()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            facingDirection = moveInput;
        }
    }

    private void Move()
    {
        if (knockbackTimer > 0f)
        {
            rb.linearVelocity = knockbackVelocity;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void ImproveMoveSpeed(float bonusRate)
    {
        if (bonusRate <= 0f)
            return;

        moveSpeed *= 1f + bonusRate;
        moveSpeed = Mathf.Min(moveSpeed, maximumMoveSpeed);
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (direction.sqrMagnitude <= 0.01f)
            return;

        knockbackVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }
}