using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Animator animator;
    public float jumpForce = 100;
    private float multiplier = 1;
    private Vector3 colliderCenter;
    private float colliderHeight;
    private bool isJumping, isGroundRaycastOn, shouldPerformHardLanding;

    private void Start() {
        colliderCenter = GetComponent<CapsuleCollider>().center;
        colliderHeight = GetComponent<CapsuleCollider>().height;
        
    }
    
    void Update() {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");
        float jump = Input.GetAxis("Jump");

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
            float jumpForceMultiplier = Random.Range(1f, 2f);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * jumpForceMultiplier, ForceMode.Force);
        }

        if (isGroundRaycastOn) {
            DetectGround();
        }
        
        // if (animator.GetNextAnimatorStateInfo(0).IsName("Jumping")) {
        //     // float duration = animator.GetNextAnimatorStateInfo(0).length;
        //     if (animator.GetNextAnimatorStateInfo(0).normalizedTime > 0.9f) {
        //         Debug.Log("Catch");
        //         GetComponent<CapsuleCollider>().center = colliderCenter;
        //         GetComponent<CapsuleCollider>().height = colliderHeight;
        //         GetComponent<Rigidbody>().useGravity = true;
        //         isjumping = false;
        //     }
        // }

        // if (animator.GetNextAnimatorStateInfo(0).IsName("jumping up")) {
        //     if (animator.GetNextAnimatorStateInfo(0).normalizedTime > 0.8f) {
        //         isGroundRaycastOn = true;
        //     }
        // }

        animator.SetFloat("BackForward", ver * multiplier);
        animator.SetFloat("LeftRight", hor * multiplier);
        
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
