using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PreyScript : Agent
{

    public MatchManager matchManager;
    public float maxVelocity = 4.86f*0.8f;
    Vector3 currentVelocity = Vector3.zero;
    public float turnSpeed = 90f;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Animator anim;

    private void move(float forward, float turn) {
        turn = Mathf.Clamp(turn, -1, 1);
        this.transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);

        forward = Mathf.Clamp(forward, 0, 1)*maxVelocity;
        currentVelocity = this.transform.forward * forward;
    }

    public override void OnEpisodeBegin()
    {
        matchManager.setRandomPositionInBounds(this.transform);
        currentVelocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(currentVelocity);
        sensor.AddObservation(this.transform.rotation);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
        continuousActions[1] = Input.GetAxisRaw("Horizontal");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        move(actions.ContinuousActions[0], actions.ContinuousActions[1]);
    }

    private void OnCollisionEnter(Collision other) {
        
        if (other.gameObject.CompareTag("Wall")) {
            // Give -0.5 points
            AddReward(-0.5f);
        }
        else if (other.gameObject.CompareTag("Predator")) {
            // Give -1 point
            AddReward(-1f);
            // maybe disable this game object or transfer this to new location?
            // this.gameObject.SetActive(false);
            // matchManager.setRandomPositionInBounds(this.transform);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Food")) {
            // Give +0.5 points
            AddReward(0.5f);
            // Tell MatchManager to move food source
            matchManager.setRandomPositionInBounds(other.transform);
        }
    }

    private void Update() {
        anim.SetFloat("velocity", currentVelocity.magnitude/maxVelocity);
        currentVelocity = currentVelocity.magnitude * this.transform.forward;
        currentVelocity = currentVelocity.normalized * Mathf.Clamp(currentVelocity.magnitude, 0f, maxVelocity);
        this.transform.localPosition += currentVelocity * Time.deltaTime;
    }

}
