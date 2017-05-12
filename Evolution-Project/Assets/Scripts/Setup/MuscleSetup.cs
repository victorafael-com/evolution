using UnityEngine;

[System.Serializable]
public class MuscleSetup : ISetup<OrganismMuscle>
{
	public int jointA;
	public int jointB;
	[Space]
	public float activeTime;
	public float interval;
	public float contractedDistance;
	public float relaxedDistance;
	public float frequency;
	public float startPhase;

	public void Apply(OrganismMuscle muscle)
	{
		muscle.activeTime = activeTime;
		muscle.interval = interval;
		muscle.contractedDistance = contractedDistance;
		muscle.relaxedDistance = relaxedDistance;
		muscle.frequency = frequency;
		muscle.startPhase = startPhase;
	}
}