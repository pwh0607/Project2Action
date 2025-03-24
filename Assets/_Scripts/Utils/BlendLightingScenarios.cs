using UnityEngine;

public class BlendLightingScenarios : MonoBehaviour
{
    UnityEngine.Rendering.ProbeReferenceVolume probeRefVolume;
    public string scenario01 = "Scenario01Name";
    public string scenario02 = "Scenario02Name";
    [Range(0, 1)] public float blendingFactor = 0.5f;
    [Min(1)] public int numberOfCellsBlendedPerFrame = 10;

    void Start()
    {
        probeRefVolume = UnityEngine.Rendering.ProbeReferenceVolume.instance;
        probeRefVolume.lightingScenario = scenario01;
        probeRefVolume.numberOfCellsBlendedPerFrame = numberOfCellsBlendedPerFrame;
    }

    void Update()
    {
        probeRefVolume.BlendLightingScenario(scenario02, blendingFactor);
    }
}