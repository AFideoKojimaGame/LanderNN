using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanderManager : MonoBehaviour {

    List<Lander> landers = new List<Lander>();
    public Vector3 initialPos = new Vector3(0, 3, 0);
    GeneticAlgorithm myGA;

    public GameObject landerPrefab;

	// Use this for initialization
	void Start () {

        for(int i = 0; i < 20; i++) {
            Vector3 newPos = new Vector3(Random.Range(-15, 15), Random.Range(-2.5f, 10), 0);
            GameObject newLander = Instantiate(landerPrefab, newPos, Quaternion.identity);
            landers.Add(newLander.GetComponent<Lander>());
        }

        myGA = GetComponent<GeneticAlgorithm>();

        for (int i = 0; i < landers.Count; i++) {
            landers[i].myAgent = myGA.population[i];
        }

        myGA.done = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (myGA.done) {
            ResetLanders();
        }
    }

    void ResetLanders() {

        for (int i = 0; i < 20; i++) {
            Destroy(landers[i].gameObject);
        }

        landers.Clear();
        landers = null;
        landers = new List<Lander>();

        for (int i = 0; i < 20; i++) {
            Vector3 newPos = new Vector3(Random.Range(-15, 15), Random.Range(-2.5f, 10), 0);
            GameObject newLander = Instantiate(landerPrefab, newPos, Quaternion.identity);
            landers.Add(newLander.GetComponent<Lander>());
        }

        //for (int i = 0; i < landers.Count; i++) {
        //    landers[i].transform.position = initialPos;
        //    landers[i].ReplaceAgent(myGA.population[i]);
        //}

        for (int i = 0; i < landers.Count; i++) {
            landers[i].myAgent = myGA.population[i];
            landers[i].UpdateWeights();
            landers[i].weightsSet = false;
        }

        myGA.done = false;
    }
}
