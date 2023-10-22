using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LineManager : IInitializable
{
    [Inject] private ObjectPool _objectPool;
    private Queue<Line> _lineQueue;

    public void Initialize()
    {
        _lineQueue = new Queue<Line>();
        for (int i = 0; i < 20; i++)
        {
            var line = _objectPool.SpawnFromPool(PoolTags.Line, Vector3.zero, Quaternion.identity).GetComponent<Line>();
            line.Initialize(this);
        }
    }

    public Line SetLine(Transform fromIsland, Transform toIsland)
    {
        var positions = new Vector3[6];
        positions[0] = fromIsland.position;
        positions[1] = fromIsland.position + fromIsland.forward * 0.5f;
        positions[2] = fromIsland.position + fromIsland.forward * 1.3f;
        positions[3] = toIsland.position + toIsland.forward * 1.3f;
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
}