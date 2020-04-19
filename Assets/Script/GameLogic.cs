using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameData.Money = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        int test = Random.Range(-10, 10);
        Debug.Log(test);
    }
}
