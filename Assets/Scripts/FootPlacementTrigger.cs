using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlacementTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            other.GetComponent<IKAnimationManager>().StartFootPositioning(transform);
        }
    } 

    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            other.GetComponent<IKAnimationManager>().StopFootPositioning(transform);
        }
    }
}
