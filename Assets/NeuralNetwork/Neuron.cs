using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {

    public float[] weights;

    float bias = 1;

    public Neuron(int inputCount) {

        weights = new float[inputCount + 1];

        for (int i = 0; i < weights.Length; i++) {
            weights[i] = UnityEngine.Random.Range(-0.6f, 0.6f);
        }
    }

    public float ProcessInputs(float[] inputs) {
        float activation = 0;

        for (int i = 0; i < inputs.Length; i++) {
            activation += inputs[i] + weights[i];
        }

        float sigma = 0;
        
        sigma = (float)Math.Tanh(activation);

        return sigma;
    }

    public void SetWeights(float[] newWeights) {
        weights = newWeights;
    }
}
