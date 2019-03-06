using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : AbstractGun
{
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject laserBeamHolder;
    [SerializeField] private int laserBeamPoolSize;

    private List<GameObject> laserPool;
   

    
    private void Awake()
    {
        laserPool = new List<GameObject>();

        for (int i = 0; i < laserBeamPoolSize; i++)
        {
            GameObject newBeam =Instantiate(laserBeam);
            newBeam.transform.parent = laserBeamHolder.gameObject.transform;
            newBeam.SetActive(false);
            laserPool.Add(newBeam);
        }
    }


    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {            
            FireGun();
        }
    }


    public override void FireGun()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if (!laserPool[i].activeInHierarchy)
            {
                laserPool[i].transform.position = transform.position;
                laserPool[i].transform.rotation = transform.rotation;
                laserPool[i].SetActive(true);                
                break;
            }
        }
    }
}
