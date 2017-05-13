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

	public JointSetup Lerp(JointSetup to, float lerp){
		return new JointSetup(){
			position = Vector2.Lerp(position, to.position, lerp),
			size = Mathf.Lerp(size, to.size, lerp),
			weight = Mathf.Lerp(weight, to.weight, lerp),
			friction = Mathf.Lerp(friction, to.friction, lerp),
			bounciness = Mathf.Lerp(bounciness, to.bounciness, lerp)
		};
	}
}