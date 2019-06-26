using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointCloudFactory  
{
    List<Vector3> GetPointCloud(Vector3 position);
}
