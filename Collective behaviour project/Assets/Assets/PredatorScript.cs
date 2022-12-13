using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorScript : MonoBehaviour
{

    float visionRadius = 5f;
    float maxVelocity = 4f;
    public PreyScript[] allPreyRef = null;
    List<PreyScript> globalMemory = new List<PreyScript>();
    List<PreyScript> localMemory = new List<PreyScript>();
    [SerializeField]
    Animator anim;
    Vector3 currentVelocity = Vector3.zero;
    Transform currentTarget;

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(this.transform.position, visionRadius);
    }

    private void Start() {
        anim.SetFloat("velocity", 1f);
    }

    private void moveTo(Transform prey) {

        currentTarget = prey;
        Vector3 targetPosition = prey.position;
        targetPosition.y = this.transform.position.y;
        this.transform.LookAt(prey.position);
        this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);
        // currentVelocity = (targetPosition-this.transform.position).normalized * maxVelocity;
    }

    private void FixedUpdate() {
        currentVelocity = this.transform.forward * maxVelocity; // currentVelocity.normalized * Mathf.Clamp(currentVelocity.magnitude, 0f, maxVelocity);
        this.transform.localPosition += currentVelocity * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate which prey to move to
        if (allPreyRef != null && allPreyRef.Length > 0 ){
            foreach (PreyScript prey in allPreyRef) {
                if ((prey.transform.position - this.transform.position).magnitude <= visionRadius) {
                    if (!localMemory.Contains(prey))
                        localMemory.Add(prey);
                    if (!globalMemory.Contains(prey))
                        globalMemory.Add(prey);
                }
                else {
                    globalMemory.Remove(prey);
                    localMemory.Remove(prey);
                }

                if (localMemory.Count < 2) {
                    globalMemory.Clear();
                    PreyScript tmpClosestPrey = allPreyRef[0];
                    foreach (PreyScript tmpPrey in allPreyRef) {
                        if ((tmpClosestPrey.transform.position - this.transform.position).magnitude > (tmpPrey.transform.position - this.transform.position).magnitude)
                            tmpClosestPrey = tmpPrey;
                    }
                    //move towards closest prey
                    moveTo(tmpClosestPrey.transform);
                }
                else {
                    if (globalMemory.Count > 0) {
                        Transform tmpClosestPrey = localMemory[0].transform;
                        //move to closest prey
                        moveTo(tmpClosestPrey);
                    }
                    else {
                        // move as before
                    }
                }
            }
        }
    }
}
