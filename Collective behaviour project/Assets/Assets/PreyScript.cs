using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PreyScript : Agent
{

    public MatchManager matchManager;
    float maxVelocity = 4f*0.8f;
    Vector3 currentVelocity = Vector3.zero;
    float turnSpeed = 120f;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Animator anim;
    [SerializeField]
    bool isGOM = false;
    [SerializeField]
    bool isFear = false;

    float ESpredator = 0.0f;
    float R = 5.0f;
    float Dpredator = 0.0f;
    public PredatorScript predator = null;
    float EScontagion = 0.0f;
    public PreyScript[] allPrey = null;
    float mp = 0.6f;
    float mc = 0.4f;
    float ESfinal = 0.0f;
    float treshold = 0.5f;

    private void Start() {
        anim.SetFloat("velocity", 0.8f);
        predator = matchManager.predator;
        allPrey = matchManager.allPrey;
    }

    private void move(float turn) {
        if (isFear && ESfinal > treshold)
        {
            this.transform.LookAt(2 * this.transform.position - predator.transform.position);
            this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);
        }
        else
        {
            turn = Mathf.Clamp(turn, -1, 1);
            this.transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
        }
        currentVelocity = this.transform.forward * maxVelocity;
    }

    private void FixedUpdate() {
        CalculateESfinal();
        currentVelocity = currentVelocity.magnitude * this.transform.forward;
        currentVelocity = currentVelocity.normalized * Mathf.Clamp(currentVelocity.magnitude, 0f, maxVelocity);
        this.transform.localPosition += currentVelocity * Time.fixedDeltaTime;
    }

    private void CalculateESfinal()
    {
        Dpredator = Vector3.Distance(this.transform.position, predator.transform.position);
        ESpredator = 1 - (Dpredator / R);
        if (ESpredator < 0.0f)
        {
            ESpredator = 0.0f;
        }

        foreach (PreyScript prey in allPrey)
        {
            if (prey != this)
            {
                float distance = Vector3.Distance(this.transform.position, prey.transform.position);
                if (distance < R)
                {
                    EScontagion += prey.ESpredator;
                }
            }
        }

        EScontagion = EScontagion / allPrey.Length;

        ESfinal = mp * ESpredator + mc * EScontagion;
    }

    public override void OnEpisodeBegin()
    {
        matchManager.setRandomPositionInBounds(this.transform);
        currentVelocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(currentVelocity.magnitude);
        sensor.AddObservation(this.transform.rotation);
        if (isGOM) {
            sensor.AddObservation(predator.transform.position);
            sensor.AddObservation(predator.maxVelocity);
            sensor.AddObservation(Vector3.Distance(predator.transform.position, this.transform.position));
            sensor.AddObservation(predator.transform.rotation);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        move(actions.ContinuousActions[0]);
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
}
