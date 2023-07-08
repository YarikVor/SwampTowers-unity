using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Path : MonoBehaviour
{
    public Transform[] Points;
    
    void Start()
    {
        UpdatePoints();
    }
    
    
    private void UpdatePoints()
    {
        Points = GetComponentsInChildren<Transform>()
            .Skip(1)
            .ToArray();
    }
    
    private void OnDrawGizmos()
    {
        UpdatePoints();
        
        if(Points.Length <= 1)
            return;
        
        Vector3 current;
        Vector3 next = Points[0].position;

        foreach (var transform in Points.Skip(1))
        {
            current = next;
            next = transform.position;
            
            Gizmos.DrawLine(current, next);
        }
        
    }

}
