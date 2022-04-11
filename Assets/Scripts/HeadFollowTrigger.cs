using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollowTrigger : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            other.GetComponent<IKAnimationManager>().StartHeadFollow(transform);
        }
    } 

    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            other.GetComponent<IKAnimationManager>().StopHeadFollow(transform);
        }
    }
}
