using System.Collections.Generic;
using UnityEngine;

public class QLearningAgent
{
    private float learningRate;
    private float discountFactor;

    private Dictionary<int, float[]> qTable;  // The Q-table

    public QLearningAgent(float learningRate, float discountFactor)
    {
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        qTable = new Dictionary<int, float[]>();
    }

    

    // Initialize Q-table for a given state dynamically if it doesn't exist
    private void EnsureStateExists(int state)
    {
        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new float[3];  // 3 actions: move, attack, retreat
            Debug.Log("New state added to Q-table: " + state);
        }
    }

    // Choose the best action for the given state
    public int ChooseAction(int currentState)
    {
        // Ensure the current state is in the Q-table
        EnsureStateExists(currentState);

        // Now we can safely access the current state's action values
        float[] actionValues = qTable[currentState];
        int bestAction = 0;

        // Find the best action based on the current Q-values
        for (int i = 1; i < actionValues.Length; i++)
        {
            if (actionValues[i] > actionValues[bestAction])
            {
                bestAction = i;
            }
        }

        return bestAction;
    }

    // Update the Q-table after an action is taken
    public void UpdateQTable(int currentState, int action, float reward, int nextState)
    {
        // Ensure both currentState and nextState exist in the Q-table
        EnsureStateExists(currentState);
        EnsureStateExists(nextState);

        // Q-learning update formula
        float[] currentActionValues = qTable[currentState];
        float[] nextActionValues = qTable[nextState];

        // Q(s, a) = Q(s, a) + α * [reward + γ * max(Q(s', a')) - Q(s, a)]
        currentActionValues[action] = currentActionValues[action] +
                                      learningRate * (reward + discountFactor * Mathf.Max(nextActionValues) - currentActionValues[action]);
    }
}
