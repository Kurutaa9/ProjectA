using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController rb;

    public float speed;
    public float gravity = -9.81f;

    private float verticalVelocity = 0f;

    private Vector2 movementInput;
    private Vector2 nowDirection;

    // InputActionReference for inspector hookup
    public InputActionReference move;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 
    }

    void OnEnable()
    {
        if (move != null && move.action != null) move.action.Enable();
    }

    void OnDisable()
    {
        if (move != null && move.action != null) move.action.Disable();
    }

    void Update()
    {
        UpdateDirection();

        // Read Movement (Vector2) from the referenced action
        if (move != null && move.action != null)
            movementInput = move.action.ReadValue<Vector2>();

        if (animator != null)
        {
            float horizSpeed = new Vector2(nowDirection.x, nowDirection.y).magnitude * speed;
            animator.SetFloat("Speed", horizSpeed);
        }
    }

    void FixedUpdate()
    {
        // Horizontal move (XZ plane)
        Vector3 move = new Vector3(nowDirection.x, 0f, nowDirection.y).normalized;

        // Gravity keeps player grounded
        if (rb.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.fixedDeltaTime;

        // Compose motion
        Vector3 motion =
            (move * speed) * Time.fixedDeltaTime +
            Vector3.up * verticalVelocity * Time.fixedDeltaTime;

        rb.Move(motion);

        // Face move direction
        if (move.sqrMagnitude > 0.01f)
        {
            float yaw = Mathf.Rad2Deg * Mathf.Atan2(move.x, move.z);
            transform.eulerAngles = new Vector3(0f, yaw, 0f);
        }

        if (animator != null)
            animator.SetFloat("Speed", move.magnitude);
    }

    // Optional: still works with PlayerInput "Send Messages"
    public void Movement(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            movementInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            movementInput = Vector2.zero;
    }

    #region Do not modify
    Vector2 Vec2Interpolation(Vector2 input_1, Vector2 input_2, float alpha)
    {
        return input_1 * (1 - alpha) + input_2 * alpha;
    }

    void UpdateDirection()
    {
        if (movementInput == Vector2.zero)
            nowDirection = Vec2Interpolation(movementInput, nowDirection, 0.1f);
        else
            nowDirection = Vec2Interpolation(movementInput, nowDirection, 0.99f);

        nowDirection.x = Mathf.Round(nowDirection.x * 10000f) / 10000f;
        nowDirection.y = Mathf.Round(nowDirection.y * 10000f) / 10000f;
    }
    #endregion
}
