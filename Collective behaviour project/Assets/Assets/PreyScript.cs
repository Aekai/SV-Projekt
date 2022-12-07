using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class PreyScript : Agent
{

    public MatchManager matchManager;
    public float maxVelocity = 4.86f*0.8f;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Animator anim;

    private void move(float forward, float turn) {
        this.rb.AddRelativeForce(Vector3.forward*(forward*maxVelocity));
        this.rb.AddRelativeTorque(Vector3.left*turn*30f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.ContinuousActions[0]);
        move(actions.ContinuousActions[0], actions.ContinuousActions[1]);
        //base.OnActionReceived(actions);
    }

    private void OnCollisionEnter(Collision other) {
        
        if (other.gameObject.CompareTag("Wall")) {
            // Give -0.5 points
        }
        else if (other.gameObject.CompareTag("Predator")) {
            // Give -1 point

            // maybe disable this game object or transfer this to new location?
            // this.gameObject.SetActive(false);
            matchManager.setRandomPositionInBounds(this.transform);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Food")) {
            // Give +1 point

            // Tell MatchManager to delete food source and make new one
            matchManager.setRandomPositionInBounds(other.transform);
        }
    }

    private void Update() {
        anim.SetFloat("velocity", this.rb.velocity.magnitude/maxVelocity);
    }

}
