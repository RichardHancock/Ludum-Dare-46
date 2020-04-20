using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int HDDUserVal = 5;
    private int CoreUserVal = 40;
    private int ComputeUserVal = 100;
    private float UserValue = 0.5f;
    private int UserNum = 40;
    private GameData gameData;


    // tmp
    private int hdd_capacity = 12;
    private int core_capacity = 1;
    private int compute_capacity = 1;

    protected void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
    }

    protected void IncreaseUsers()
    {
        // User num increases by 10 every 10 seconds.
        UserNum += 10;
    }

    protected void UpdateScores()
    {
        // This runs every second.
        int HDDUserCapacity = hdd_capacity * HDDUserVal;
        int CoreUserCapacity = core_capacity * CoreUserVal;
        int ComputeUserCapacity = compute_capacity * ComputeUserVal;

        int CurrentUserCapacity = HDDUserCapacity + CoreUserCapacity + ComputeUserCapacity;

        float CapacityPercentage = ((float)UserNum / (float)CurrentUserCapacity) * 100.0f;

        float income = (float)UserNum * UserValue;
        // If the capacity_percentage is at 50% then the income is equal to the running cost.
        float CostScale = (income * 2.0f) / 100.0f;
        float RunningCost = CapacityPercentage * CostScale;
        int Profit = (int)(income - RunningCost);

        gameData.Money += Profit;
        Debug.Log("Capacity: " + CapacityPercentage + "%, Users: " + UserNum + ", Profit: " + Profit + "Pounds, Money: " + gameData.Money + "Pounds");
    }
}
