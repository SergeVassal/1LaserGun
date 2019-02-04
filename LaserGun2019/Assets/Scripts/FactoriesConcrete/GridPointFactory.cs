using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPointFactory : AbstractGridPointSpawnerFactory
{
    public override List<Vector3> CreateGridPointCloud(Vector3 initPosition,int columnAmount, int rowAmount, 
        float columnInterval, float rowInterval, GridPointFactoryDirectionEnum direction)
    {
        Vector3 prevPosition = initPosition;
        List < Vector3 > pointCloudList= new List<Vector3>();

        for (int r = 0; r < rowAmount; ++r)
        {
            for (int c = 0; c < columnAmount; ++c)
            {
                Vector3 newPosition = PositionOfNewGO(initPosition,prevPosition, c, r, columnInterval, rowInterval,direction);
                pointCloudList.Add(newPosition);
                prevPosition = newPosition;
            }
        }

        return pointCloudList;
    }

    private Vector3 PositionOfNewGO(Vector3 initPosition,Vector3 prevPosition, int c, int r, float columnInterval, float rowInterval, GridPointFactoryDirectionEnum direction)
    {        
        if (c == 0 && r == 0)
        {
            return initPosition;
        }
        else if (c == 0 && r != 0)
        {
            return new Vector3(initPosition.x, prevPosition.y + rowInterval, initPosition.z);
        }
        else
        {
            switch (direction)
            {
                case GridPointFactoryDirectionEnum.plusX:
                    if (r == 0 && c != 0)
                    {
                        return new Vector3(prevPosition.x + columnInterval, initPosition.y, initPosition.z);
                    }
                    else
                    {
                        return new Vector3(prevPosition.x + columnInterval, prevPosition.y, initPosition.z);
                    }                    
                case GridPointFactoryDirectionEnum.minusX:
                    if (r == 0 && c != 0)
                    {
                        return new Vector3(prevPosition.x - columnInterval, initPosition.y, initPosition.z);
                    }
                    else
                    {
                        return new Vector3(prevPosition.x - columnInterval, prevPosition.y, initPosition.z);
                    }                    
                case GridPointFactoryDirectionEnum.plusZ:
                    if (r == 0 && c != 0)
                    {
                        return new Vector3(initPosition.x, initPosition.y, prevPosition.z + columnInterval);
                    }
                    else
                    {
                        return new Vector3(initPosition.x, prevPosition.y, prevPosition.z + columnInterval);
                    }                    
                case GridPointFactoryDirectionEnum.minusZ:
                    if (r == 0 && c != 0)
                    {
                        return new Vector3(initPosition.x, initPosition.y, prevPosition.z - columnInterval);
                    }
                    else
                    {
                        return new Vector3(initPosition.x, prevPosition.y, prevPosition.z - columnInterval);
                    }
                    
            }
        }
        return Vector3.zero;
    }
}
