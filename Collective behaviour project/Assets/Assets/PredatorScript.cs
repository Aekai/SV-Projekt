using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorScript : MonoBehaviour
{

    public float visionRadius = 30f;
    public float maxVelocity = 10f; 
    public PreyScript[] allPreyRef = null;
    List<PreyScript> globalMemory = new List<PreyScript>();
    List<PreyScript> localMemory = new List<PreyScript>();
    [SerializeField]
    Animator anim;

    private void moveTo(Transform prey) {

        this.transform.LookAt(prey.position);
        Vector3 direction = (prey.position-this.transform.position).normalized;
        this.transform.position += direction * Time.deltaTime * maxVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("velocity", 1f);
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
