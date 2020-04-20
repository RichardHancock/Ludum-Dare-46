using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private float PercentageUserVal = 0.1f;
    private int CoreHDDScale = 12;
    private int ComputeHDDScale = 6;
    private float UserValue = 1.0f;
    private int UserNum = 0;
    private GameData gameData;
    private float HDDCapacityPercentage = 0.0f;
    private float CoreCapacityPercentage = 0.0f;
    private float ComputeCapacityPercentage = 0.0f;

    public void Begin()
    {
        gameData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
        StartCoroutine(IncreaseUsers());
        StartCoroutine(UpdateScores());
    }

    protected IEnumerator IncreaseUsers()
    {
        while(true)
        {
            // User num increases by 12 every 30 seconds unless any capacity is 100% or above.
            if (HDDCapacityPercentage < 100.0f && CoreCapacityPercentage < 100.0f && ComputeCapacityPercentage < 100.0f)
            {
                Debug.Log("User num increase.");
                UserNum += 12;
            }
            yield return new WaitForSeconds(30.0f);
        }
    }

    protected IEnumerator UpdateScores()
    {
        while (true)
        {
            // This runs every second.
            int HDDUserCapacity = gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.HardDrive);
            int CoreUserCapacity = gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.Core) * CoreHDDScale;
            int ComputeUserCapacity = gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.Compute) * ComputeHDDScale;

            HDDCapacityPercentage = CalculatePercentage((float)(UserNum) * PercentageUserVal, (float)HDDUserCapacity);
            CoreCapacityPercentage = CalculatePercentage((float)(UserNum) * PercentageUserVal, (float)CoreUserCapacity);
            ComputeCapacityPercentage = CalculatePercentage((float)(UserNum) * PercentageUserVal, (float)ComputeUserCapacity);

            float income = (float)UserNum * UserValue;
            // If the total capacity percentage is at 150% then the income is equal to the running cost.
            float CostScale = (income * 2.0f) / 300.0f;
            float HDDRunningCost = HDDCapacityPercentage * CostScale;
            float CoreRunningCost = CoreCapacityPercentage * CostScale;
            float ComputeRunningCost = ComputeCapacityPercentage * CostScale;
            float RunningCost = HDDRunningCost + CoreRunningCost + ComputeRunningCost;
            int Profit = (int)(income - RunningCost);

            gameData.Money += Profit;
            Debug.Log("Total Cap: " + (int)(HDDCapacityPercentage + CoreCapacityPercentage + ComputeCapacityPercentage) + "%");
            Debug.Log("HDD Capacity: " + (int)HDDCapacityPercentage + "%, Core Capacity: " + (int)CoreCapacityPercentage + "%, Compute Capacity: " + (int)ComputeCapacityPercentage + "%, Users: " + UserNum + ", Profit: " + Profit + " Pounds, Money: " + gameData.Money + "Pounds");
            yield return new WaitForSeconds(1.0f);
        }
    }

    private float CalculatePercentage(float A, float B) 
    { 
        //A is ?% of B
        return (A / B) * 100.0f;
    }
}
