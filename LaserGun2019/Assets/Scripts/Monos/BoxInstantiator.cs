using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int rowAmount;
    [SerializeField] private float rowInterval;
    [SerializeField] private int columnAmount;
    [SerializeField] private float columnInterval;
    [SerializeField] GridPointFactoryDirectionEnum direction;

    private List<GameObject> objectPool;
    private List<Vector3> pointCloud;


    private void Start()
    {
        objectPool = new List<GameObject>();        
       
        pointCloud = CreatePointCloud(new GridPointFactory());

        for (int i = 0; i < pointCloud.Count; i++)
        {            
            GameObject newBox = Instantiate(objectToSpawn, pointCloud[i], Quaternion.identity, this.transform);
            objectPool.Add(newBox);
        } 

    }

    private List<Vector3> CreatePointCloud(AbstractGridPointSpawnerFactory factory)
    {
        return factory.CreateGridPointCloud(transform.position, columnAmount, rowAmount,
            columnInterval, rowInterval, direction);
    }
}
