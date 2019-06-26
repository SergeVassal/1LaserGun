using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject laserBeamPrefab;
    [SerializeField] private int laserBeamPoolSize;
    [SerializeField] private Transform laserBeamPoolParentTransform;

    private List<GameObject> laserBeamPool;


    private void Awake()
    {
        CreateLaserBeamPool();
    }

    private void CreateLaserBeamPool()
    {
        laserBeamPool = new List<GameObject>();

        for (int i = 0; i < laserBeamPoolSize; i++)
        {
            GameObject newGO = Instantiate(laserBeamPrefab, laserBeamPoolParentTransform);
            newGO.SetActive(false);
            laserBeamPool.Add(newGO);
        }
    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }
    }

    public void FireWeapon()
    {
        for (int i = 0; i < laserBeamPool.Count; i++)
        {
            if (!laserBeamPool[i].activeInHierarchy)
            {
                laserBeamPool[i].transform.position = transform.position;
                laserBeamPool[i].transform.rotation = transform.rotation;
                laserBeamPool[i].SetActive(true);
                break;
            }
        }
    }
}
