using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 0.04f;

    
    void Update() {
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");

        transform.position += transform.forward * (ver * moveSpeed) + transform.right * (hor * moveSpeed);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);



    }

}
