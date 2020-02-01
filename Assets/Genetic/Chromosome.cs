using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome {

    public int geneCount = 4;

    public float[][][] genes;

    int max = 3;

    public void SetWeights(float[][][] w) {
        genes = w;
    }

    public void GenesCrossover(float[][][] g1, float[][][] g2) {
        genes = g1;

        int finalLength = 0;
        int index = 0;

        for (int i = 0; i < g1.Length; i++) {
            for (int j = 0; j < g1[i].Length; j++) {
                finalLength += g1[i][j].Length;
            }
        }

        for (int i = 0; i < g1.Length; i++) {
            for (int j = 0; j < g1[i].Length; j++) {
                for (int k = 0; k < g1[i][j].Length; k++) {
                    if (index <= finalLength / 2)
                        genes[i][j][k] = g1[i][j][k];
                    else
                        genes[i][j][k] = g2[i][j][k];

                    index++;
                }
            }
        }
    }

    public void GeneMutate(float mutationChance, int mutationMax, float mutationRate) {

        for (int i = 0; i < genes.Length; i++) {
            for (int j = 0; j < genes[i].Length; j++) {
                for (int k = 0; k < genes[i][j].Length; k++) {
                    if (ShouldMutate(mutationChance, mutationMax)) {
                        genes[i][j][k] += Random.Range(-mutationRate, mutationRate);
                    }
                }
            }
        }
    }

    bool Flip()
    {
        int shouldFlip = Random.Range(0, 2);

        if (shouldFlip == 1)
            return false;
        else
            return true;
    }

    bool ShouldMutate(float mutationChance, int mutationMax) {
        int ratio = Random.Range(0, mutationMax);

        if (ratio <= mutationChance) {
            return true;
        }

        return false;
    }
}
