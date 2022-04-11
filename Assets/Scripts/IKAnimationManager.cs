using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKAnimationManager : MonoBehaviour {
    [Header("Set on Editor")]
    public Transform headIKDummy;
    public Transform rightFootIKDummy;
    public Rig rigScript;
    [Header("Set Runtime, DO NOT ALTER on Editor")]
    public Transform target;
    public Transform rightFootTarget;
    private IEnumerator graduallyLookCoroutine;
    

    

    private void Update() {
        if (target)
            headIKDummy.transform.position = target.position;
        if (rightFootTarget) {
            rightFootIKDummy.transform.position = rightFootTarget.position;
            rigScript.weight = Mathf.Lerp(rigScript.weight, 1, 10 * Time.deltaTime);
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


}
