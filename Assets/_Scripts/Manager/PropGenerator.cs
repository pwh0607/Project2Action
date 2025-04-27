using UnityEngine;

public abstract class PropGenerator
{
    protected DungeonContext context;
    protected StageManager stageManager;

    public PropGenerator(DungeonContext context)
    {
        this.context = context;
    }

    public abstract void Initialize();
}
