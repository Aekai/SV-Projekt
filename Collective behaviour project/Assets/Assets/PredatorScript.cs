using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorScript : MonoBehaviour
{

    public float visionRadius = 30f;
    public float maxVelocity = 10f; 
    public Transform[] allPreyRef = null;
    List<Transform> globalMemory = new List<Transform>();
    List<Transform> localMemory = new List<Transform>();
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Animator anim;

    private void moveTo(Transform prey) {

        this.transform.LookAt(prey.position);
        float acceleration = 150f;
        Vector3 direction = Vector3.forward;
        direction.y = 0;
        this.rb.AddRelativeForce(direction*acceleration * Time.deltaTime);
        if (this.rb.velocity.magnitude > maxVelocity)
            this.rb.velocity = this.rb.velocity.normalized * maxVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("velocity", this.rb.velocity.magnitude/maxVelocity);
        if (allPreyRef != null && allPreyRef.Length > 0 ){
            foreach (Transform prey in allPreyRef) {
                if ((prey.position - this.transform.position).magnitude <= visionRadius) {
                    localMemory.Add(prey);
                    if (!globalMemory.Contains(prey)) {
                        globalMemory.Add(prey);
                    }
                }
                else {
                    if (globalMemory.Contains(prey)) {
                        globalMemory.Remove(prey);
                    }
                }

                if (localMemory.Count < 2) {
                    globalMemory.Clear();
                    Transform tmpClosestPrey = allPreyRef[0];
                    foreach (Transform tmpPrey in allPreyRef) {
                        if ((tmpClosestPrey.position - this.transform.position).magnitude > (tmpPrey.position - this.transform.position).magnitude)
                            tmpClosestPrey = tmpPrey;
                    }
                    //move towards closest prey
                    moveTo(tmpClosestPrey);
                }
                else {
                    if (globalMemory.Count > 0) {
                        Transform tmpClosestPrey = localMemory[0];
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
