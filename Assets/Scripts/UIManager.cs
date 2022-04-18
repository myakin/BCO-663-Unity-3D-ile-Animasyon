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
    private bool isListening;
    private GameObject targetPlayer, targetGrabPoint;
    private bool isCarrying;

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
