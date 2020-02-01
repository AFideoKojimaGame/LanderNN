using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN {

    int hiddenCount;
    int hiddenNeurons;
    int inputCount;
    int outputCount;

    int lastInputs;

    Layer[] layers;

    public NN(int inC, int hidC, int hidN, int outC) {

        inputCount = inC;
        hiddenCount = hidC;
        hiddenNeurons = hidN;
        outputCount = outC;

        layers = new Layer[inputCount + hiddenCount + outputCount];

        layers[0] = new Layer(inputCount, inputCount);
        lastInputs = inputCount;

        for (int i = 1; i < layers.Length - 1; i++) {
            layers[i] = new Layer(hiddenNeurons, lastInputs);
            lastInputs = hiddenNeurons;
        }

        layers[layers.Length - 1] = new Layer(outputCount, lastInputs);

    }

    public float[] ProcessInputs(float[] inputs) {

        float[] outputs = new float[outputCount];
        float[] actualInput = inputs;

        for (int i = 0; i < layers.Length; i++) {

            actualInput = layers[i].ProcessInputs(actualInput);
        }

        outputs = actualInput;
        return outputs;
    }

    public float[][][] GetWeights() {

        float[][][] weights = new float[layers.Length][][];

        for (int i = 0; i < layers.Length; i++) {
            weights[i] = layers[i].GetWeights();
        }

        return weights;
    }

    public void SetWeights(float[][][] weights) {
        for (int i = 0; i < layers.Length; i++) {
            layers[i].SetWeights(weights[i]);
        }
    }
}
