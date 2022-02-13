using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;

    void Awake()
    {
       input = new PlayerInput();

        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void handleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3 (currentPosition.x, 0, currentPosition.y);

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    private void OnEnable()
    {
        input.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
