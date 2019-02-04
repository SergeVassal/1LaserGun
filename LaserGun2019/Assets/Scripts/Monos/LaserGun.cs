using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LaserFactory))]
public class LaserGun : AbstractGun
{
    private AbstractBulletFactory laserFactory;
    private List<GameObject> bulletPool;

    private void Awake()
    {
        laserFactory = GetComponent<AbstractBulletFactory>();
        bulletPool = laserFactory.CreateBulletPool();        
    }    

    public override void FireGun()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].transform.position = transform.position;
                bulletPool[i].transform.rotation = transform.rotation;
                bulletPool[i].SetActive(true);
                bulletPool[i].GetComponent<AbstractBullet>().FireBullet();
                break;
            }
        }
    }
    
}
