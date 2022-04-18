using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForGrab : MonoBehaviour {
    public GameObject grabPointDummy;
    private GameObject targetPlayer;

    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            targetPlayer = other.gameObject;
            UIManager.instance.SetForGrab(other.gameObject, grabPointDummy);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            if (targetPlayer==other.gameObject) {
                UIManager.instance.ResetGrabWarning();
            }
        }
    }
}
