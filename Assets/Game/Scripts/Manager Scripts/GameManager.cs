using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int totalMoney;
    void Start()
    {
        if (LevelManager.Instance.GetGlobalLevelIndex() == 0) {//new game
            totalMoney = 0;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
        }
        if (PlayerPrefs.GetInt("TotalMoney") >= 0) {

            SetTotalMoney(0);
        }
        
    }
    void SetTotalMoney(int collectedAmount) {
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 0) + collectedAmount;
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        UIManager.Instance.SetTotalMoney();

        totalMoney = 0;
    }

}
