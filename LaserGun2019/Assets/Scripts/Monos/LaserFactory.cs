using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFactory : AbstractBulletFactory
{    
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private int laserBeamPoolSize;

    private List<GameObject> laserPool;


    public override List<GameObject> CreateBulletPool()
    {
        laserPool = new List<GameObject>();

        for (int i = 0; i < laserBeamPoolSize; i++)
        {
            GameObject newBullet = Instantiate(laserBeam);
            newBullet.SetActive(false);
            laserPool.Add(newBullet);
        }
        return laserPool;
    }


}
