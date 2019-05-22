using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PointCloudFactoryAbstract 
{
    public abstract List<Vector3> GetPointCloud(Vector3 position);	
}
