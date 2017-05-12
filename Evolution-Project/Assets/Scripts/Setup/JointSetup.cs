using UnityEngine;

[System.Serializable]
public class JointSetup : ISetup<OrganismJoint>
{
	public Vector2 position;
	public float size;
	public float weight;
	public float friction;
	public float bounciness;

	public void Apply(OrganismJoint joint)
	{
		joint.bounciness = bounciness;
		joint.friction = friction;
		joint.size = size;
		joint.weight = weight;
		joint.transform.localPosition = position;
	}
}