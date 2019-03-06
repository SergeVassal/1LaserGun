using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGridPointSpawnerFactory
{
    public abstract List<Vector3> CreateGridPointCloud(Vector3 initPosition, int columnAmount, int rowAmount,
        float columnInterval, float rowInterval, GridPointFactoryDirectionEnum direction);
}
