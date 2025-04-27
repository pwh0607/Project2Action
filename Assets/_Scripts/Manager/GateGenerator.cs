using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateGenerator : PropGenerator
{
    private DoorData doorData;
    private int lockCount;
    private Vector3 offset;

    public GateGenerator(DungeonContext context, DoorData doorData, Vector3 offset, int lockCount) : base(context)
    {
        this.doorData = doorData;
        this.lockCount = lockCount;
        this.offset = offset;
    }

    public override void Initialize()
    {
        CreateGates();
    }

    private void CreateGates()
    {
        List<int> validIndexes = new();
        for (int i = 0; i < context.links.Count; i++)
        {
            if (context.links[i].isNearWall)
                validIndexes.Add(i);
        }

        List<int> lockedIndexes = RandomGenerator.RandomIntGenerate(validIndexes.Count, lockCount).Select(idx => validIndexes[idx]).ToList();

        for (int i = 0; i < validIndexes.Count; i++)
        {
            Link link = context.links[validIndexes[i]];
            GameObject gatePrefab = lockedIndexes.Contains(i) ? doorData.lockedGate : doorData.openedGate;
            InterActiveGate gate = GameObject.Instantiate(gatePrefab, link.position, link.quaternion).GetComponent<InterActiveGate>();
            link.gate = gate;
        }
    }

    public bool HasLocksRemaining() => lockCount > 0;
    public void DecreaseLock() => lockCount--;
}
