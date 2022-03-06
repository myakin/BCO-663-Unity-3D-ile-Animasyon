using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour {
    public Transform door;
    public float duration = 1;
    public Transform closedDummy, openDummy;
    public AnimationCurve curve;
    private IEnumerator doorAnimationCoroutine;
    private float rotY;
    

    private void Start() {
        // door = GameObject.Find("Door").transform;
        // curve = AnimationCurve.EaseInOut;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            Debug.Log("Player has entered");
            // if (doorAnimationCoroutine==null) {
            //     doorAnimationCoroutine = OpenDoor();
            //     StartCoroutine(doorAnimationCoroutine);
            // }
            if (doorAnimationCoroutine!=null) {
                StopCoroutine(doorAnimationCoroutine);
                doorAnimationCoroutine = null;
            }
            if (doorAnimationCoroutine==null) {
                doorAnimationCoroutine = OpenDoor();
                StartCoroutine(doorAnimationCoroutine);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            Debug.Log("Player has exited");
            if (doorAnimationCoroutine!=null) {
                StopCoroutine(doorAnimationCoroutine);
                doorAnimationCoroutine = null;
            }
            if (doorAnimationCoroutine==null) {
                doorAnimationCoroutine = CloseDoor();
                StartCoroutine(doorAnimationCoroutine);
            }
        }
    }

    private IEnumerator OpenDoor() {
        Quaternion initRot = door.localRotation;
        Quaternion targetRot = openDummy.localRotation;

        float timer = 0;
        while (timer<duration) {
            door.localRotation = Quaternion.Lerp(initRot, targetRot, timer/duration);
            timer+=Time.deltaTime;
            yield return null;
        }
        door.localRotation = targetRot;

        doorAnimationCoroutine = null;
    }

    private IEnumerator CloseDoor() {
        Quaternion initRot = door.localRotation;
        Quaternion targetRot = closedDummy.localRotation;

        Vector3 initPos = door.localPosition;
        Vector3 targetPos = closedDummy.localPosition;

        Vector3 initScale = door.localScale;
        Vector3 targetScale = closedDummy.localScale;

        float timer = 0;
        while (timer<duration) {
            door.localRotation = Quaternion.Lerp(initRot, targetRot,  curve.Evaluate(timer/duration));
            door.localPosition = Vector3.Lerp(initPos, targetPos, curve.Evaluate(timer/duration));
            door.localScale = Vector3.Lerp(initScale, targetScale, curve.Evaluate(timer/duration));

            timer+=Time.deltaTime;
            yield return null;
        }
        door.localRotation = targetRot;
        door.localPosition = targetPos;
        door.localScale = targetScale;
        
        doorAnimationCoroutine = null;
    }

    // private IEnumerator CloseDoor() {
    //     while (rotY<=0) {
    //         door.rotation = Quaternion.Euler(0,rotY,0);
    //         rotY+=0.1f;
    //         // rotY++;
    //         yield return null;
    //     }
    //     doorAnimationCoroutine= null;
    // }



}
