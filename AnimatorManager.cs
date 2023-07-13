using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{

    public Animator animator;

    [Header("Hand IK Constraints")]
    public TwoBoneIKConstraint rightHandIK; //These constraints enable our character to hold the current weapon properly
    public TwoBoneIKConstraint leftHandIK;

    [Header("Aiming Constraints")]
    public MultiAimConstraint spine00;  //These constraints turn the character towards the aiming point
    public MultiAimConstraint spine01;
    public MultiAimConstraint head;


    RigBuilder rigBuilder;
    PlayerManager playerManager;
    PlayerLocomotionManager playerLocomotionManager;
    

    float snappedHorizontal;
    float snappedVertical;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    public void PlayAnimationWithoutRootMotion(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f);
    }
    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {

        if (horizontalMovement > 0.1)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isRunning && snappedVertical > 0) // dont wanna be able to run backwards

        {
            snappedVertical = 2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
        {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
        }
    
    // While aiming our character will turn towards the center of the screen

    public void UpdateAimConstraints()
    {
        if (playerManager.isAiming)
        {
            spine00.weight = 0.9f;
            spine01.weight = 0.3f;
            head.weight = 0.7f;
        }
        else
        {
            spine00.weight = 0f;
            spine01.weight = 0f;
            head.weight = 0f;
        }
    }

    public void UpdateHandIKConstraints()
    {
        if (playerManager.isPerformingQuickTurn)
        {
            rightHandIK.weight = 0.0f;
            leftHandIK.weight = 0.0f;
        }
       
    }

    public void OnAnimatorMove()
    {
        if (playerManager.disableRootMotion)
            return;

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;

        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidBody.drag = 0;
        playerLocomotionManager.playerRigidBody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }
}
