using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Text scoreText;

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

}
