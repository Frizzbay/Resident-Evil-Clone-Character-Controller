using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerCamera playerCamera;
    InputManager inputManager;
    Animator animator;
    AnimatorManager animatorManager;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerEquipmentManager playerEquipmentManager;
    


    [Header("Player Actions")]
    public bool disableRootMotion;
    public bool isPerformingAction;
    public bool isPerformingQuickTurn;
    public bool isAiming;


    public void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        disableRootMotion = animator.GetBool("disableRootMotion");
        isPerformingAction = animator.GetBool("isPerformingAction");
        isPerformingQuickTurn = animator.GetBool("isPerformingQuickTurn");
        isAiming = animator.GetBool("isAiming");
    }

    private void FixedUpdate()  // anything with movement on a rigid body should be called on 'FixedUpdate'
    {
        playerLocomotionManager.HandleAllLocomotion();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }

    public void UseCurrentWeapon()
    {
        // In the future we will add the option to use knives also
        if (isPerformingAction)
            return;

        //animatorManager.PlayAnimationWithoutRootMotion("Pistol_Recoil", true);    //Recoil animation wont work because of the rig stuff I think.  Needs an avatar mask on a different layer.   
        playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera);
    }
}
