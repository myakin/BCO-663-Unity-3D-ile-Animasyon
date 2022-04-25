using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectMovement : MonoBehaviour {
    public float range = 5;
    public enum MovementAxis { forward, right, up };
    public MovementAxis direction;
    public float moveSpeed = 0.5f;
    private Vector3 initialPos;
    private float directionMultiplier = 1;

    private void Start() {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - initialPos).sqrMagnitude>=range*range) {
            directionMultiplier *= -1;
        }
        
        Vector3 moveDirection = direction==MovementAxis.forward ? transform.forward : (direction==MovementAxis.right ? transform.right : transform.up);
        transform.position += moveDirection * (moveSpeed * Time.deltaTime * directionMultiplier);


        
    }
}
