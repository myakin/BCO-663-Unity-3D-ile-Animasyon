using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour {
    public Animator animator;
    public float walkSpeed = 0.2f;
    public float turnSpeed = 1f;
    public float attackDistance = 3.5f;
    public float health = 100;
    public float despawnDuration = 10;
    private Transform player;  
    private Vector3 vectorToPlayer;
    private float angle;
    private bool die;
    

    // Update is called once per frame
    void Update() {
        if (!die) {
            if (player) {
                vectorToPlayer = player.position - transform.position; 
                angle = Vector3.SignedAngle(transform.forward, vectorToPlayer, transform.up);
                if (angle<-5 || angle>5) {
                    transform.rotation*=Quaternion.Euler(0, angle<0 ? -turnSpeed : turnSpeed, 0);
                }
                if (vectorToPlayer.sqrMagnitude < attackDistance * attackDistance) {
                    animator.SetFloat("movement", 0);
                    animator.SetTrigger("attack");
                } else {
                    animator.SetFloat("movement", 1);
                    transform.position+=transform.forward * walkSpeed;
                }
                if (health<=0) {
                    die=true;
                    animator.SetTrigger("die");
                    StartCoroutine(DespawnAfterDuration());
                }
            
            } else {
                animator.SetFloat("movement", 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player") {
            player = other.transform;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag=="Player") {
            player = null;
        }
    }

    public void Damage(float amount) {
        health-=amount;
    }

    private IEnumerator DespawnAfterDuration() {
        yield return new WaitForSeconds(despawnDuration);
        Destroy(gameObject);
    }
}
