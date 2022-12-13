using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    float floorRadius = 12.5f;
    public PredatorScript predator;
    public PreyScript[] allPrey;
    public GameObject foodPrefab;
    private List<GameObject> foodList = new List<GameObject>();
    int numFood = 6;
    public GameObject foodParentObject;

    public void setRandomPositionInBounds(Transform transform) {
        float range = floorRadius*0.90f; // make sure things don't spawn next to walls
        float newX = Random.Range(-range, range);
        float newZ = Random.Range(-range, range);
        transform.position = this.transform.position + new Vector3(newX, 0.5f, newZ);
    }

    void resetState() {
        // Initialize random positions for prey
        foreach (PreyScript prey in allPrey) {
            prey.gameObject.SetActive(true);
            setRandomPositionInBounds(prey.transform);
        }

        // initialize random positions for food
        for (int i = 0; i < numFood; i++) {
            GameObject tmpFood = Instantiate(foodPrefab, foodParentObject.transform);
            setRandomPositionInBounds(tmpFood.transform);
            foodList.Add(tmpFood);
        }

        // initialize random position for predator
        setRandomPositionInBounds(predator.transform);
        predator.allPreyRef = allPrey;
    }

    // Start is called before the first frame update
    void Start()
    {
        resetState();
    }
}
