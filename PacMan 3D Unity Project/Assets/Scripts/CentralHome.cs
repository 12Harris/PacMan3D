using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CentralHome
{
    private Vector3[] homePositions = new Vector3[3];
    private int currentIndex = -1;

    public CentralHome()
    {
        homePositions[0] = new Vector3(-2,1,2);
        homePositions[1] = new Vector3(0,1,2);
        homePositions[2] = new Vector3(2,1,2);
    }

    public Vector3 NextAvailableHomePosition()
    {
        
        currentIndex++;
        if(currentIndex >= 3)
            currentIndex = 0;
        return homePositions[currentIndex];
    }

    public void FreePositions()
    {
        currentIndex = 0;
    }
}