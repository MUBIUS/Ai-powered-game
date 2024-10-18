using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class QLearningAgent
{
    private float learningRate;
    private float discountFactor;
    private int actionCount;
    private Dictionary<int, float[]> qTable;

    public QLearningAgent(float learningRate, float discountFactor, int actionCount)
    {
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        this.actionCount = actionCount;
        qTable = new Dictionary<int, float[]>();
    }

    public int ChooseAction(int state)
    {
        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new float[actionCount];
        }

        if (Random.value < 0.1f) // Exploration
        {
            return Random.Range(0, actionCount);
        }
        else // Exploitation
        {
            return GetBestAction(state);
        }
    }

    public void Learn(int state, int action, float reward, int newState)
    {
        if (!qTable.ContainsKey(newState))
        {
            qTable[newState] = new float[actionCount];
        }

        float maxFutureQ = qTable[newState].Max();
        float currentQ = qTable[state][action];

        float newQ = (1 - learningRate) * currentQ + learningRate * (reward + discountFactor * maxFutureQ);
        qTable[state][action] = newQ;
    }

    private int GetBestAction(int state)
    {
        float maxValue = float.MinValue;
        int bestAction = 0;

        for (int i = 0; i < actionCount; i++)
        {
            if (qTable[state][i] > maxValue)
            {
                maxValue = qTable[state][i];
                bestAction = i;
            }
        }

        return bestAction;
    }
}