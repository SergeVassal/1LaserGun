using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] SystemPrefabs;

    private int score = 0;

    private List<GameObject> instancedSystemPrefabs;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;

        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void IncreaseScore(int pointsToAdd)
    {
        score += pointsToAdd;
        Debug.Log(score);
        UIManager.Instance.UpdateScore(score);
    }

    public void DecreaseScore(int pointsToRemove)
    {
        score -= pointsToRemove;
        UIManager.Instance.UpdateScore(score);
    }



}
