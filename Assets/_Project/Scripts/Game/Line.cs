using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Game
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
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
            var linePoints = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(linePoints);
            return linePoints;
        }

        public void AddSiblingLine(Line line)
        {
            _siblingLines.Add(line);
        }

        private void SetPositions(IReadOnlyList<Vector3> positions)
        {
            lineRenderer.positionCount += positions.Count - lineRenderer.positionCount;
            
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i, positions[i]);
            }
        }
    }
}