using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    private LineManager _lineManager;
    private List<Line> _siblingLines;

    public void Initialize(LineManager lineManager)
    {
        _lineManager = lineManager;
        _siblingLines = new List<Line>();
        Deactivate();
    }
    
    public void PrepareToGame(Vector3[] positions)
    {
        SetPositions(positions);
        gameObject.SetActive(true);
    }
    
    private void SetPositions(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            _lineRenderer.SetPosition(i,positions[i]);
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _lineManager.GetUsedLine(this);

        foreach (var line in _siblingLines)
        {
            line.Deactivate();
        }
        
        _siblingLines.Clear();
    }

    public Vector3[] GetPositions()
    {
        Vector3[] linePoints = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(linePoints);
        return linePoints;
    }

    public void AddSiblingLine(Line line)
    {
        _siblingLines.Add(line);
    }
}
