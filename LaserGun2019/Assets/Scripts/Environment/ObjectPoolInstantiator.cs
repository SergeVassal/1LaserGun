using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInstantiator : MonoBehaviour
{
    [SerializeField] private PointCloudFactoryGrid pointCloudFactorySerializedObject;
    [SerializeField] private GameObject objectToSpawn;

    private PointCloudFactoryAbstract pointCloudFactory;
    private List<Vector3> pointCloud;
    private List<GameObject> objectPool;



    private void Start()
    {
        AssignPointCloudFactory(pointCloudFactorySerializedObject);
        pointCloud = GetPointCloud();
        objectPool = new List<GameObject>();
        InstantiateObjects();
    }

    private void AssignPointCloudFactory(PointCloudFactoryAbstract pointCloudFactoryImpl)
    {
        pointCloudFactory = pointCloudFactoryImpl;
    }

    private List<Vector3> GetPointCloud()
    {
        return pointCloudFactory.GetPointCloud(transform.position);
    }

    private void InstantiateObjects()
    {
        for(int i = 0; i < pointCloud.Count; i++)
        {
            GameObject newObj = Instantiate(objectToSpawn, pointCloud[i], Quaternion.identity, this.transform);
            objectPool.Add(newObj);
        }
    }
}
