using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchManager : MonoBehaviour
{

    float floorRadius = 12.5f;
    public PredatorScript predator;
    public PreyScript[] allPrey;
    public GameObject foodPrefab;
    private List<GameObject> foodList = new List<GameObject>();
    int numFood = 6;
    public GameObject foodParentObject;

    private int foodCounter = 0;
    private int preyCounter = 0;
    [SerializeField]
    private TextMeshProUGUI foodText;
    [SerializeField]
    private TextMeshProUGUI preyText;

    public enum TransformType
    {
        Food,
        Prey,
        Predator
    }

    public void setRandomPositionInBounds(Transform transform) {
        float range = floorRadius*0.90f; // make sure things don't spawn next to walls
        float newX = Random.Range(-range, range);
        float newZ = Random.Range(-range, range);
        transform.position = this.transform.position + new Vector3(newX, 0.5f, newZ);
    }

    public void setRandomPositionInBounds(Transform transform, TransformType transformType) {
        float range = floorRadius*0.90f; // make sure things don't spawn next to walls
        float newX = Random.Range(-range, range);
        float newZ = Random.Range(-range, range);
        transform.position = this.transform.position + new Vector3(newX, 0.5f, newZ);

        if (transformType == TransformType.Food) {
            foodCounter++;
            foodText.text = "Food: " + foodCounter.ToString();
        }
        else if (transformType == TransformType.Prey) {
            preyCounter++;
            preyText.text = "Prey: " + preyCounter.ToString();
        }
    }

    void resetState() {
        // Initialize random positions for prey
        foreach (PreyScript prey in allPrey) {
            prey.gameObject.SetActive(true);
            setRandomPositionInBounds(prey.transform);
            prey.allPrey = allPrey;
            prey.predator = predator;
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

        foodCounter = 0;
        foodText.text = "Food: " + foodCounter.ToString();
        preyCounter = 0;
        preyText.text = "Prey: " + preyCounter.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        resetState();
    }
}
