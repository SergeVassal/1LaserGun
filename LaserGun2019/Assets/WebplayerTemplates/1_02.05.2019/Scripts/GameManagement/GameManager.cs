using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int score = 0;



    private void Start()
    {
        DontDestroyOnLoad(gameObject);        
    }

    public void IncreaseScore(int pointsToAdd)
    {
        score += pointsToAdd;        
        UIManager.Instance.UpdateScore(score);
    }

    public void DecreaseScore(int pointsToRemove)
    {
        score -= pointsToRemove;
        UIManager.Instance.UpdateScore(score);
    }

}
