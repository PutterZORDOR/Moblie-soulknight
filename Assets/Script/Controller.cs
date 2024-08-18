using UnityEngine;
using UnityEngine.InputSystem;
public class Controller : MonoBehaviour
{
    public InputActionAsset inputActions;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    void OnEnable()
    {
        var gameplayActionMap = inputActions.FindActionMap("Movement");
        var moveAction = gameplayActionMap.FindAction("Walk");
        moveAction.Enable();
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    void OnDisable()
    {
        var gameplayActionMap = inputActions.FindActionMap("Movement");
        var moveAction = gameplayActionMap.FindAction("Walk");
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        moveAction.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        FlipCharacter();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void FlipCharacter()
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}