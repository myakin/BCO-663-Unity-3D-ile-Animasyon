using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
    public UnityEvent OnEnter, OnExit;


    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            if (OnEnter!=null) {
                OnEnter.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            if (OnExit!=null) {
                OnExit.Invoke();
            }
        }
    }
}
