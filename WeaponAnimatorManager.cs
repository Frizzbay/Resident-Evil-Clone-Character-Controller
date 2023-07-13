using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    Animator weaponAnimator;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX;  //The muzzle flash FX that is instantiated when the weapon is fired 
    public GameObject weaponBulletCaseFX; // The bullet case FX that is ejected from the weapon when the weapon is fired

    [Header("Weapon FX Transform")]
    public Transform weaponMuzzleFlashTransform;    // The location the muzzle flash FX will instantiate   
    public Transform weaponBulletCaseTransform;     // The location the bullet case will instantiate




    // Start is called before the first frame update
    void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        //weaponAnimator = GameObject.Find("M1911 Handgun_Model").GetComponent<Animator>();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        //Animate Weapon
        weaponAnimator.Play("Shoot");
        //Instantiate Muzzle Flash FX
        GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;
        //Instantiate Empty Bullet Case
        GameObject bulletCase = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        bulletCase.transform.parent = null;
        //Shoot Something

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
