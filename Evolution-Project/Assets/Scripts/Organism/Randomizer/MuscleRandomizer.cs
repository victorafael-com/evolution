[System.Serializable]
public class MuscleRandomizer : IRandomizer<MuscleSetup>{
	public Randomizable activeTime;
	public Randomizable interval;
	public Randomizable contractedDistance;
	public Randomizable relaxedDistance;
	public Randomizable frequency;
	public Randomizable startPhase;

	public MuscleSetup Randomize(MuscleSetup setup)
	{
		MuscleSetup result = new MuscleSetup();

		result.activeTime = activeTime.Randomize(setup.activeTime);
		result.interval = interval.Randomize(setup.interval);
		result.contractedDistance = contractedDistance.Randomize(setup.contractedDistance);
		result.relaxedDistance = relaxedDistance.Randomize(setup.relaxedDistance);
		result.frequency = frequency.Randomize(setup.frequency);
		result.startPhase = startPhase.Randomize(setup.startPhase);

		result.jointA = setup.jointA;
		result.jointB = setup.jointB;

		return result;
	}

	public MuscleSetup FullRandomize(){
		MuscleSetup result = new MuscleSetup ();

		//result.jointA;
		//result.jointB;

		result.activeTime = activeTime.RandomVal;
		result.interval = interval.RandomVal;
		result.contractedDistance = contractedDistance.RandomVal;
		result.relaxedDistance = relaxedDistance.RandomVal;
		result.frequency = frequency.RandomVal;
		result.startPhase = startPhase.RandomVal;

		return result;
	}
}

