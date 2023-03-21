using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _controller;

    [Range(0,100)] public float speed = 5.0f;

    public InputActionReference moveAction;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
        moveAction.action.Enable();
    }

    private void OnDisable() {
        moveAction.action.Disable();
    }

    private void Update() {
        Movement();
        _controller.enabled = !Grapple.isGrappling;
    }

    private void Movement() {
        if(Grapple.isGrappling) return;
        if(!moveAction.action.IsPressed()) return;
        Vector2 move = moveAction.action.ReadValue<Vector2>();

        Vector3 direction = new Vector3(move.x, 0, move.y);
        direction = transform.TransformDirection(direction);
        direction *= speed;

        _controller.Move(direction * Time.deltaTime);
    }
}

