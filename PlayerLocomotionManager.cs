using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerManager playerManager;

    public Rigidbody playerRigidBody;

    [Header("Camera Transform")]
    public Transform playerCamera;  //Player camera transform would work too?

    [Header("Movement Speed")]
    public float rotationSpeed = 3.5f;
    public float quickTurnSpeed = 8;

    [Header("Rotation Variables")]
    Quaternion targetRotation; //The place we want to rotate 
    Quaternion playerRotation; //The place we are rotating now, constantly changing

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
    }
    public void HandleAllLocomotion()
    {
        HandleRotation();
        //HandleFalling();
    }

    private void HandleRotation()
    {
        if (playerManager.isAiming)
        {
            targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0)
            {
                transform.rotation = playerRotation;
            }

            if (playerManager.isPerformingQuickTurn)
            {
                playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, quickTurnSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
        }
        

    }

}