using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKAnimationManager : MonoBehaviour {
    [Header("Set on Editor")]
    public Transform headIKDummy;
    public Transform rightFootIKDummy;
    public Transform rightHandIKDummy, grabPointDummy;
    public Transform lumbarIKDummy, lumbarNormalRotationDummy;
    public Rig rigScript;
    [Header("Set Runtime, DO NOT ALTER on Editor")]
    public Transform target;
    public Transform rightFootTarget;
    public Transform rightHandTarget;
    private IEnumerator graduallyLookCoroutine;
    private bool isGrabbed, isCarrying;
    private float releaseTimer;

    

    private void Update() {
        if (target)
            headIKDummy.position = target.position;
        if (rightFootTarget) {
            rightFootIKDummy.position = rightFootTarget.position;
            rigScript.weight = Mathf.Lerp(rigScript.weight, 1, 10 * Time.deltaTime);
        }
        if (rightHandTarget) {
            if (!isCarrying) {
                if (!isGrabbed) {
                    rightHandIKDummy.position = rightHandTarget.position;
                    rightHandIKDummy.rotation = rightHandTarget.rotation;
                    lumbarIKDummy.position = rightHandTarget.position;
                }   
                // rig2Script.weight = Mathf.Lerp(rigScript.weight, 1, 10 * Time.deltaTime);
                rigScript.weight = Mathf.Lerp(rigScript.weight, 1, 10 * Time.deltaTime);

                if (rigScript.weight>=0.9f && rightHandTarget.parent.parent!=grabPointDummy) {
                    rightHandTarget.parent.parent = grabPointDummy;
                    UIManager.instance.SetForRelease();
                    isGrabbed = true;
                }

                if (isGrabbed) {
                    lumbarIKDummy.position = Vector3.Lerp(lumbarIKDummy.position, lumbarNormalRotationDummy.position,  10 * Time.deltaTime);
                }
            } else {
                lumbarIKDummy.position = Vector3.Lerp(lumbarNormalRotationDummy.position, lumbarIKDummy.position,  releaseTimer);
                releaseTimer+=Time.deltaTime;

                if (releaseTimer>1) {
                    if (isGrabbed) {
                        rightHandTarget.parent.parent = null;
                        rightHandTarget.parent.gameObject.AddComponent<Rigidbody>();
                        isGrabbed = false;
                    }

                    rightHandIKDummy.position = rightHandTarget.position;
                    rightHandIKDummy.rotation = rightHandTarget.rotation;
                    lumbarIKDummy.position = rightHandTarget.position;

                    rigScript.weight = Mathf.Lerp(rigScript.weight, 0, 10 * Time.deltaTime);
                }

            }
        }
        
    }

    public void StartHeadFollow(Transform aTarget) {
        target = aTarget;  
        if (graduallyLookCoroutine==null) {
            graduallyLookCoroutine = GraduallyLookCoroutine(1);
            StartCoroutine(graduallyLookCoroutine);
        } 
    }

    public void StopHeadFollow(Transform aTarget) {
        if (target == aTarget) {
            // target = null; 
            if (graduallyLookCoroutine==null) {
                graduallyLookCoroutine = GraduallyLookCoroutine(0);
                StartCoroutine(graduallyLookCoroutine);
            }
        }
    }

     

    public void StartFootPositioning(Transform aTarget) {
        rightFootTarget = aTarget;
    }
    public void StopFootPositioning(Transform aTarget) {
        rightFootTarget = null;
        rigScript.weight = 0;
    }

    private IEnumerator GraduallyLookCoroutine(float targetValue) {
        float initValue = rigScript.weight;
        float timer = 0;
        float duration = 1;
        while (timer<duration) {
            rigScript.weight = Mathf.Lerp(initValue, targetValue, timer/duration);
            timer+=Time.deltaTime;
            yield return null;
        }
        rigScript.weight = targetValue;
        if (targetValue==0) {
            target = null;
        }
        graduallyLookCoroutine = null;
    }


    public void GrabItem(GameObject targetObject) {
        isCarrying = false;
        if (targetObject.transform.parent.GetComponent<Rigidbody>()) {
            Destroy(targetObject.transform.parent.GetComponent<Rigidbody>());
        }
        rightHandTarget = targetObject.transform;
    }
    public void ReleaseItem() {
        isCarrying = true;
    }
}
