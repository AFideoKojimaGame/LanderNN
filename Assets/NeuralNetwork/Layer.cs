using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer {

    Neuron[] neurons;

    public Layer(int neuronCount, int inputCount) {
        neurons = new Neuron[neuronCount];

        for (int i = 0; i < neurons.Length; i++) {
            neurons[i] = new Neuron(inputCount);
        }
    }

    public float[] ProcessInputs(float[] inputs) {

        float[] outputs = new float[neurons.Length];

        for (int i = 0; i < neurons.Length; i++) {
            outputs[i] = neurons[i].ProcessInputs(inputs);
        }

        return outputs;
    }

    public float[][] GetWeights() {
        float[][] weights = new float[neurons.Length][];

        for (int i = 0; i < neurons.Length; i++) {
            weights[i] = neurons[i].weights;
        }

        return weights;
    }

    public void SetWeights(float[][] weights) {

        for (int i = 0; i < neurons.Length; i++) {
            neurons[i].SetWeights(weights[i]);
        }
    }
}
