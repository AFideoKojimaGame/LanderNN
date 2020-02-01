using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneticAlgorithm : MonoBehaviour {

    public List<Agent> population = new List<Agent>();
    List<Agent> nextGen = new List<Agent>();
    List<Agent> elites = new List<Agent>();

    int currentGeneration = 0;

    public int eliteCount = 6;
    public int populationCount = 20;

    int geneCount = 6;

    public float simulationTime = 60;
    float simulationCounter = 0;

    float mutationRate = 0.05f;
    public Text genText;

    //2 in 10 by default
    float mutationChance = 2;
    int mutationMax = 1;

    public bool done = false;

    int age = 1;

	// Use this for initialization
	void Start () {
        Random.InitState((int)System.DateTime.Now.Ticks);

		for(int i = 0; i < populationCount; i++) {
            population.Add(new Agent());
            population[i].Init();
        }
	}
	
	// Update is called once per frame
	void Update () {
        simulationCounter += Time.deltaTime;

        if(simulationCounter > simulationTime) {
            Epoch();
        }
	}

    void Crossover() {

        for (int i = 0; i < (populationCount - eliteCount) / 2; i++) {
            Agent parent1 = Roulette();
            Agent parent2 = Roulette();

            //int inheritRatio = Random.Range(0, geneCount);
            int inheritRatio = geneCount / 2;

            Agent child1 = GeneCombine(parent1, parent2, inheritRatio);
            Agent child2 = GeneCombine(parent2, parent1, inheritRatio);

            nextGen.Add(child1);
            nextGen.Add(child2);
        }

        population.Clear();

        for(int i = 0; i < nextGen.Count; i++) {
            population.Add(nextGen[i]);
        }
    }



    Agent GeneCombine(Agent p1, Agent p2, int ratio) {

        Agent a = new Agent();
        a.InitBreed();

        a.myC.GenesCrossover(p1.myC.genes, p2.myC.genes);
        a.myC.GeneMutate(mutationChance, mutationMax, mutationRate);

        return a;
    }

    bool ShouldMutate() {
        int ratio = Random.Range(0, mutationMax);

        if (ratio <= mutationChance) {
            return true;
        }

        return false;
    }

    Agent Roulette() {

        float totalFitness = 0;

        for (int i = 0; i < population.Count; i++)
        {
            totalFitness += population[i].GetFitness();
        }

        //float totalFitness = 0;
        //totalFitness = population[0].GetFitness();

        //for (int j = 0; j < population.Count; j++) {
        //    if (population[j].GetFitness() > totalFitness)
        //        totalFitness = population[j].GetFitness();
        //}

        float selectFitness = Random.Range(0, totalFitness);
        int selectedAgent = 0;
        float decideTotal = 0;

        for (int i = 0; i < population.Count; i++) {
            decideTotal += population[i].GetFitness();

            if (decideTotal > selectFitness) {
                selectedAgent = i;
                break;
            }
        }

        Agent chosen = population[selectedAgent];
        population.RemoveAt(selectedAgent);
        return chosen;
    }

    void Elitism() {

        elites.Clear();
        elites = null;
        elites = new List<Agent>();

        for (int k = 0; k < population.Count; k++) {
            population[k].elite = false;
        }

        Agent highest = population[0];
        int highestIndex = 0;

        for(int i = 0; i < eliteCount; i++) {

            for(int j = 0; j < population.Count; j++) {
                if (population[j].GetFitness() > highest.GetFitness()) {
                    highest = population[j];
                    highestIndex = j;
                }
            }

            Debug.Log((highestIndex + i) + ", " + highest.GetFitness());

            highest.elite = true;
            elites.Add(population[highestIndex]);
            population.RemoveAt(highestIndex);
            highest = population[0];
            highestIndex = 0;
        }

        for (int i = 0; i < elites.Count; i++) {
            population.Add(elites[i]);
        }
    }

    public void Epoch() {

        nextGen.Clear();

        age++;
        genText.text = "GEN " + age;

        Elitism();
        Crossover();

        for (int i = 0; i < elites.Count; i++) {
            population.Add(elites[i]);
        }

        done = true;
        simulationCounter = 0;
    }

    void DefineFitness() {

    }
}
