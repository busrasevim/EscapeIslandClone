using System.Collections.Generic;
using _Project.Scripts.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class LineManager : IInitializable
    {
        [Inject] private ObjectPool _objectPool;
        private Queue<Line> _lineQueue;
        private List<Line> _allLines;

        public void Initialize()
        {
            _lineQueue = new Queue<Line>();
            _allLines = new List<Line>();
            for (int i = 0; i < 50; i++)
            {
                var line = _objectPool.SpawnFromPool(PoolTags.Line, Vector3.zero, Quaternion.identity).GetComponent<Line>();
                line.Initialize(this);
                _allLines.Add(line);
            }
        }

        public Line SetLine(Transform fromIsland, Transform toIsland)
        {
            var positions = new Vector3[6];
            positions[0] = fromIsland.position;
            positions[1] = fromIsland.position + fromIsland.forward * 0.5f;
            positions[2] = fromIsland.position + fromIsland.forward +
                           (toIsland.position - fromIsland.position).normalized * 0.3f;
            positions[3] = toIsland.position + toIsland.forward +
                           (fromIsland.position - toIsland.position).normalized * 0.3f;
            positions[4] = toIsland.position + toIsland.forward * 0.5f;
            positions[5] = toIsland.position;

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].y = -0.1f;
            }

            var line = _lineQueue.Dequeue();
            line.PrepareToGame(positions);
            return line;
        }

        public void GetUsedLine(Line line)
        {
            if (_lineQueue.Contains(line)) return;

            _lineQueue.Enqueue(line);
        }

        public void ResetLines()
        {
            foreach (var line in _allLines)
            {
                line.Deactivate();
            }
        }
    }
}