using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    private void Awake() {
        instance = this;
    }

    public Text warningText;
    public KeyCode grabKey = KeyCode.E;
    public KeyCode selectTargetKey = KeyCode.Tab;
    private bool isListening;
    private GameObject targetPlayer, targetGrabPoint;
    private bool isCarrying;
    private GameObject currentTarget;
    private int currentTargetIndex;
    private List<GameObject> targets = new List<GameObject>();

    private void Update() {
        if (isListening) {
            if (Input.GetKeyDown(grabKey)) {
                if (!isCarrying) {
                    targetPlayer.GetComponent<IKAnimationManager>().GrabItem(targetGrabPoint);
                    isCarrying = true;
                } else { 
                    targetPlayer.GetComponent<IKAnimationManager>().ReleaseItem();
                    isCarrying = false;
                }
            }
        }
        if (Input.GetKeyDown(selectTargetKey)) {
            // search for targets around 100m
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            RaycastHit[] hits = Physics.SphereCastAll(player.transform.position, 100, player.transform.forward, 0.1f, 1<<8, QueryTriggerInteraction.Ignore);
            
            // update targets list with new elements
            targets.Clear();
            for (int i=0; i<hits.Length; i++) {
                if (!targets.Contains(hits[i].collider.gameObject)) {
                    targets.Add(hits[i].collider.gameObject);
                }
            }

            // target selection logic
            if (targets!=null) {
                if (!currentTarget) {
                    currentTarget = targets[0];
                    currentTargetIndex = 0;
                } else {
                    if (currentTargetIndex+1 < targets.Count) {
                        currentTargetIndex++;
                    } else {
                        currentTargetIndex=0;
                    }
                    currentTarget = targets[currentTargetIndex];
                }
            }

            // pin current target
            player.GetComponent<IKAnimationManager>().StartAiming(currentTarget.transform);

            Debug.Log(currentTarget.name);
        }
    }

    public void SetForGrab(GameObject aPlayer, GameObject aGrabPoint) {
        warningText.text = "Hit "+grabKey.ToString()+" to grab";
        targetPlayer = aPlayer;
        targetGrabPoint = aGrabPoint;
        isListening = true;
    }

    public void ResetGrabWarning() {
        warningText.text = "";
        isListening = false;
    }

    public void SetForRelease() {
        warningText.text = "Hit "+grabKey.ToString()+" to release";
    }
}
