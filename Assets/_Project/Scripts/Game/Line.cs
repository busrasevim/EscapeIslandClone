using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    public void PrepareToGame(Vector3[] positions)
    {
        SetPositions(positions);
    }
    
    private void SetPositions(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            _lineRenderer.SetPosition(i,positions[i]);
        }
    }
}
