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
            // User num increases by 20 every 10 seconds.
            //TODO stop increasing if capacity hits 100%
            UserNum += 20;
            yield return new WaitForSeconds(10.0f);
        }
    }

    protected IEnumerator UpdateScores()
    {
        while (true)
        {
            // This runs every second.
            int HDDUserCapacity = 12 * HDDUserVal;//gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.HardDrive) * HDDUserVal;
            int CoreUserCapacity = 1 * CoreUserVal;//gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.Core) * CoreUserVal;
            int ComputeUserCapacity = 1 * ComputeUserVal;//gameData.GetTotalModuleTypeRackCapacity(RackModule.ModuleType.Compute) * ComputeUserVal;

            int CurrentUserCapacity = HDDUserCapacity + CoreUserCapacity + ComputeUserCapacity;

            float CapacityPercentage = ((float)UserNum / (float)CurrentUserCapacity) * 100.0f;

            float income = (float)UserNum * UserValue;
            // If the capacity_percentage is at 50% then the income is equal to the running cost.
            float CostScale = (income * 2.0f) / 100.0f;
            float RunningCost = CapacityPercentage * CostScale;
            int Profit = (int)(income - RunningCost);

            gameData.Money += Profit;
            Debug.Log("Capacity: " + CapacityPercentage + "%, Users: " + UserNum + ", Profit: " + Profit + "Pounds, Money: " + gameData.Money + "Pounds");
            yield return new WaitForSeconds(1.0f);
        }
    }
}
