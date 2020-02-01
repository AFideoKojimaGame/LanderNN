using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent {

    public Chromosome myC;
    float fitness = 0;
    public bool elite = false;

	public void Init() {
        myC = new Chromosome();
    }

    public void InitBreed() {
        myC = new Chromosome();
    }

    public float GetFitness() {
        return fitness;
    }

    public void AddFitness(float f)
    {
        fitness += f;
    }

    public void SetFitness(float f) {
        fitness = f;
    }
}
