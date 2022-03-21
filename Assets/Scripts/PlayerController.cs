using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour {
    public Animator animator;
    public float jumpForce = 100;
    public static Action<float> OnCameraLookUpDown;
    public Transform weaponPositionDummy;
    public GameObject[] weapons;

    private float multiplier = 1;
    private Vector3 colliderCenter;
    private float colliderHeight;
    private bool isJumping, isGroundRaycastOn, shouldPerformHardLanding, isAttacking;

    private void Start() {
        colliderCenter = GetComponent<CapsuleCollider>().center;
        colliderHeight = GetComponent<CapsuleCollider>().height;
        
    }
    
    void Update() {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float jump = Input.GetAxis("Jump");
        float fire2 = Input.GetAxis("Fire2");

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ) {
            multiplier = 2;
        } else {
            multiplier = 1;
        }

        if (jump>0 && !isJumping) {
            isJumping = true;
            shouldPerformHardLanding = false;
            animator.SetTrigger("Jump");
            Vector3 centerPos = GetComponent<CapsuleCollider>().center;
            GetComponent<CapsuleCollider>().center = new Vector3(centerPos.x , 1.1f, centerPos.z);
            GetComponent<CapsuleCollider>().height = 1.78f;
            // GetComponent<Rigidbody>().useGravity = false;
            float jumpForceMultiplier = UnityEngine.Random.Range(1f, 2f);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * jumpForceMultiplier, ForceMode.Force);
        }

        if (isGroundRaycastOn) {
            DetectGround();
        }

        animator.SetFloat("BackForward", ver * multiplier);
        animator.SetFloat("LeftRight", hor * multiplier);

        transform.rotation *= Quaternion.Euler(0, mouseX, 0);   

        OnCameraLookUpDown(mouseY);     
        // public void LookUpDown(float value) {
        //     lookUpDownOffset += value; 
        // }


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeWeapon(0);
            SetAnimationLayerForUpperBody(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeWeapon(1);
            SetAnimationLayerForUpperBody(2);
        }

        if (fire2>0 && !isAttacking && weaponPositionDummy.childCount>0) {
            isAttacking = true;
            if (weaponPositionDummy.GetChild(0).GetComponent<WeaponController>().weaponType==WeaponController.WeaponType.rifle) {
                AnimateFire2(1);
            } else if (weaponPositionDummy.GetChild(0).GetComponent<WeaponController>().weaponType==WeaponController.WeaponType.sword) {
                AnimateFire2(2);
            }
        }

        
    }

    private void ChangeWeapon(int weaponIndex) {
        if (weaponPositionDummy.childCount>0) {
            weaponPositionDummy.GetChild(0).gameObject.SetActive(false);
            weaponPositionDummy.GetChild(0).SetParent(null);
        }
        weapons[weaponIndex].transform.SetParent(weaponPositionDummy);
        weapons[weaponIndex].transform.localPosition = Vector3.zero;
        weapons[weaponIndex].transform.localRotation = Quaternion.identity;
        weapons[weaponIndex].SetActive(true);
    }

    private void SetAnimationLayerForUpperBody(int aLayerIndex) {
        animator.SetLayerWeight(aLayerIndex, 1);
    }

    private void AnimateFire2(int aLayerIndex) {
        animator.SetLayerWeight(aLayerIndex, 1);
        animator.SetTrigger("Fire2");
    }

    public void ResetLayerWeight(int aLayerIndex) {
        // animator.SetLayerWeight(aLayerIndex, 0);
    }
    public void ResetAttack() {
        isAttacking = false;
    }

    public void NeutarizeJumping() {
        Debug.Log("Catch");
        GetComponent<CapsuleCollider>().center = colliderCenter;
        GetComponent<CapsuleCollider>().height = colliderHeight;
        GetComponent<Rigidbody>().useGravity = true;
        isJumping = false;
    }

    private void DetectGround() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, 1<<0, QueryTriggerInteraction.Ignore)) {
            if (hit.distance>5) {
                shouldPerformHardLanding = true;
            }
            Debug.Log(hit.distance);
            if (hit.distance<0.5f) {
                Debug.Log(hit.transform.name);
                if (shouldPerformHardLanding) {
                    animator.SetBool("HardLandingBool", true);
                } else {
                    animator.SetBool("FallToRollBool", true);
                }
            }
            
        }
    }

    
    

    public void EnableGroundRaycasting() {
        isGroundRaycastOn = true;
    }

    public void EndJump() {
        isJumping = false;
        isGroundRaycastOn = false;
        shouldPerformHardLanding = false;
        animator.SetBool("FallToRollBool", false);
        animator.SetBool("HardLandingBool", false);
        // NeutarizeJumping();
    }

    public void ResetCollider() {
        GetComponent<CapsuleCollider>().center = colliderCenter;
        GetComponent<CapsuleCollider>().height = colliderHeight;
        GetComponent<Rigidbody>().useGravity = true;
    }



}
