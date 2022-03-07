using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Animator animator;
    private float multiplier = 1;
    
    void Update() {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ) {
            multiplier = 2;
        } else {
            multiplier = 1;
        }

        animator.SetFloat("BackForward", ver * multiplier);
        animator.SetFloat("LeftRight", hor * multiplier);
        

    }

}
