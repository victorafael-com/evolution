using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrganismSetup
{
    public JointSetup[] joints;
    public MuscleSetup[] muscles;
}

[System.Serializable]
public class JointSetup
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

[System.Serializable]
public class MuscleSetup
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
