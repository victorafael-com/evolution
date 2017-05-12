[System.Serializable]
public class JointRandomizer : IRandomizer<JointSetup>{
	public Randomizable position;
	public Randomizable size;
	public Randomizable weight;
	public Randomizable friction;
	public Randomizable bounciness;

	public JointSetup Randomize(JointSetup setup)
	{
		JointSetup result = new JointSetup();

		result.position.x = position.Randomize(setup.position.x);
		result.position.y = position.Randomize(setup.position.y);
		result.size = size.Randomize(setup.size);
		result.weight = weight.Randomize(setup.weight);
		result.friction = friction.Randomize(setup.friction);
		result.bounciness = bounciness.Randomize(setup.bounciness);

		return result;
	}

	public JointSetup FullRandomize(JointSetup setup){
		JointSetup result = new JointSetup ();

		result.position = setup.position;
		result.size = size.RandomVal;
		result.weight = weight.RandomVal;
		result.friction = friction.RandomVal;
		result.bounciness = bounciness.RandomVal;

		return result;
	}
}

