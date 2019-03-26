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
    private Vector3 newPosition;


    public override List<Vector3> GetPointCloud(Vector3 position)
    {
        initialPos = position;
        CreatePointCloud();
        return pointCloud;
    }

    private void CreatePointCloud()
    {
        previousPos = initialPos;        
        pointCloud = new List<Vector3>();

        for(int rowIndex = 0; rowIndex < rowAmount; ++rowIndex)
        {
            for(int columnIndex = 0; columnIndex < columnAmount; ++columnIndex)
            {
                GetNextPosition(columnIndex,rowIndex);                
                previousPos = newPosition;
            }
            AddNewRow(rowIndex);
            
            
        }
        for (int i = 0; i < pointCloud.Count; i++)
        {
            //Debug.Log(pointCloud[i]);
        }
    }

    private void GetNextPosition(int currentColumnIndex,int currentRowIndex)
    {     
        
        AddNewColumn(currentColumnIndex, currentRowIndex);        
    }

    
    
    private void AddNewColumn(int currentColumnIndex, int currentRowIndex)
    {
        if (currentColumnIndex == 0 && currentRowIndex == 0)
        {
            newPosition = initialPos;
            pointCloud.Add(newPosition);
            return;
        }
        switch (pointCloudDirection)
        {
            case PointCloudDirection.plusX:
                newPosition.x=previousPos.x + columnInterval;
                break;

            case PointCloudDirection.minusX:
                newPosition.x = previousPos.x - columnInterval;
                break;

            case PointCloudDirection.plusZ:
                newPosition.z = previousPos.z + columnInterval;
                break;

            case PointCloudDirection.minusZ:
                newPosition.z = previousPos.z - columnInterval;
                break;
        }
        pointCloud.Add(newPosition);
    }

    private void AddNewRow(int rowIndex)
    {
        if (rowIndex == 0)
        {
            return;
        }
        newPosition= new Vector3(initialPos.x, previousPos.y + rowInterval, initialPos.z);
        pointCloud.Add(newPosition);
        previousPos = newPosition;
        Debug.Log(previousPos);
        Debug.Log(newPosition);
    }

}
