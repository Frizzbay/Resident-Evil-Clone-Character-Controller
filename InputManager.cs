using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerManager playerManager;
    Animator animator;
    PlayerUIManager playerUIManager;


    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;


    [Header("Camera Movement")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool runInput;
    public bool quickTurnInput;
    public bool aimingInput;
    public bool shootInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();

    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
            playerControls.PlayerMovement.QuickTurn.performed += i => quickTurnInput = true;
            playerControls.PlayerActions.Aim.performed += i => aimingInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimingInput = false;
            playerControls.PlayerActions.Shoot.performed += i => shootInput = true;
            playerControls.PlayerActions.Shoot.canceled += i => shootInput = false;
            //playerControls.whatever action + i = i and whatever boolean becomes true or false.
        }

        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleQuickTurnInput();
        HandleAimingInput();
        HandleShootingInput();
       
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);

        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            animatorManager.rightHandIK.weight = 0;
            animatorManager.leftHandIK.weight = 0;
        }
        else
        {
            animatorManager.rightHandIK.weight = 1;
            animatorManager.leftHandIK.weight = 1;
        }

    }
       
    
    private void HandleCameraInput()
    {
        //Divded these values for lower sensitivity but might need to be reverted later.  
        horizontalCameraInput = cameraInput.x / 10;
        verticalCameraInput = cameraInput.y / 10;
    }

    private void HandleQuickTurnInput() {

        if (playerManager.isPerformingAction)
            return;

        if (quickTurnInput)
        {
            animator.SetBool("isPerformingQuickTurn", true);
            //Play an animation that turns the player
            animatorManager.PlayAnimationWithoutRootMotion("Quick Turn", true);
        }
       
    }

    private void HandleAimingInput()
    {
        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            aimingInput = false;
            animator.SetBool("isAiming", false);
            playerUIManager.crosshair.SetActive(false);
            return;
        }

        if (aimingInput)
        {
            animator.SetBool("isAiming", true);
            playerUIManager.crosshair.SetActive(true);

        }
        else 
        {
            animator.SetBool("isAiming", false);
            playerUIManager.crosshair.SetActive(false);

        }
        animatorManager.UpdateAimConstraints();
    }

    private void HandleShootingInput()
    {
        //Decide if the weapon is semi or fully automatic
        if (shootInput && aimingInput)
        {
            shootInput = false;
            //Shoot current weapon
            Debug.Log("BANG!");
            playerManager.UseCurrentWeapon();
        }
    }
}
