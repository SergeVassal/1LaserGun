using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInstantiator : MonoBehaviour 
{
    [SerializeField] private PointCloudFactoryGrid pointCloud;
    [SerializeField] private GameObject objectToSpawn;

    private List<Vector3> points;
    private List<GameObject> objectPool;
    

    private void Start()
    {
        points = pointCloud.GetPointCloud(transform.position);
        objectPool = new List<GameObject>();
        InstantiateObjects();
    }

    private void InstantiateObjects()
    {
        for(int i = 0; i < points.Count; i++)
        {
            GameObject newGO = Instantiate(objectToSpawn, points[i], Quaternion.identity,
                this.transform);
            objectPool.Add(newGO);
        }
    }
}
