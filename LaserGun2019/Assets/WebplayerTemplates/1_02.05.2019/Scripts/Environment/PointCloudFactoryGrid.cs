using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointCloudFactoryGrid : PointCloudFactoryAbstract
{
    [SerializeField] private int columnAmount;
    [SerializeField] private float columnInterval;

    [SerializeField] private int rowAmount;
    [SerializeField] private float rowInterval;

    [SerializeField] private enum PointCloudDirection
    {
        plusX,
        minusX,
        plusZ,
        minusZ
    }

    private List<Vector3> pointCloud;
    private PointCloudDirection pointCloudDirection = PointCloudDirection.plusX;
    private Vector3 initialPos;
    private Vector3 previousPos;
    private Vector3 newPointPosition;



    public override List<Vector3> GetPointCloud(Vector3 position)
    {
        pointCloud = new List<Vector3>();
        initialPos = position;
        previousPos = initialPos;
        CreatePointCloud();
        return pointCloud;
    }

    private void CreatePointCloud()
    {     
        float nextPointYCoordinate = initialPos.y;
        for (int rowIndex = 0; rowIndex < rowAmount; ++rowIndex)
        {
            for (int columnIndex = 0; columnIndex < columnAmount; ++columnIndex)
            {
                Vector3 nextPointXZCoordinates= GetNextPointXZCoordinates(columnIndex);                
                newPointPosition = new Vector3(nextPointXZCoordinates.x, nextPointYCoordinate, nextPointXZCoordinates.z);

                pointCloud.Add(newPointPosition);
                previousPos = newPointPosition;
            }
            nextPointYCoordinate = GetNextPointYCoordinate(rowIndex);
        }       
    }

    private Vector3 GetNextPointXZCoordinates(int currentColumnIndex)
    {
        Vector3 nextPointXZCoordinates=previousPos;

        if (currentColumnIndex == 0)
        {
            nextPointXZCoordinates.x = initialPos.x;
            nextPointXZCoordinates.z = initialPos.z;            
        }
        else
        {
            switch (pointCloudDirection)
            {
                case PointCloudDirection.plusX:
                    nextPointXZCoordinates.x = previousPos.x + columnInterval;
                    break;

                case PointCloudDirection.minusX:
                    nextPointXZCoordinates.x = previousPos.x - columnInterval;
                    break;

                case PointCloudDirection.plusZ:
                    nextPointXZCoordinates.z = previousPos.z + columnInterval;
                    break;

                case PointCloudDirection.minusZ:
                    nextPointXZCoordinates.z = previousPos.z - columnInterval;
                    break;
            }
        }        
        return nextPointXZCoordinates;
    } 

    private float GetNextPointYCoordinate(int currentRowIndex)
    {
        float nextPointYCoordinate=previousPos.y+rowInterval;   
        return nextPointYCoordinate;
    }
}
