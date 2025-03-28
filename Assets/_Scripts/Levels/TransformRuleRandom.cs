using DungeonArchitect;
using UnityEngine;

public class TransformRuleRandom : TransformationRule
{
    public override void GetTransform(PropSocket socket, DungeonModel model, Matrix4x4 propTransform, System.Random random, out Vector3 outPosition, out Quaternion outRotation, out Vector3 outScale)
    {
        base.GetTransform(socket, model, propTransform, random, out outPosition, out outRotation, out outScale);

        outRotation = Quaternion.Euler(0f, Random.rotation.eulerAngles.y, 0f);
    }

}