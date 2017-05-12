[System.Serializable]
public class JointRandomizer : IRandomizer<JointSetup>{
	public Randomizable position;
	public Randomizable size;
	public Randomizable weight;
	public Randomizable friction;
	public Randomizable bounciness;

	public JointSetup Randomize(JointSetup setup)
	{
		if (setup == null)
			return null;
		
		JointSetup result = new JointSetup();

		result.position.x = position.Randomize(setup.position.x);
		result.position.y = position.Randomize(setup.position.y);
		result.size = size.Randomize(setup.size);
		result.weight = weight.Randomize(setup.weight);
		result.friction = friction.Randomize(setup.friction);
		result.bounciness = bounciness.Randomize(setup.bounciness);

		return result;
	}

	public JointSetup FullRandomize(){
		JointSetup result = new JointSetup ();

		result.position.x = position.RandomVal;
		result.position.y = position.RandomVal;
		result.size = size.RandomVal;
		result.weight = weight.RandomVal;
		result.friction = friction.RandomVal;
		result.bounciness = bounciness.RandomVal;

		return result;
	}
}

