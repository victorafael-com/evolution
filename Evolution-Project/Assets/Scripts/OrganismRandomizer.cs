using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrganismRandomizer
{
    public JointRandomizer joint;
    public MuscleRandomizer muscle;

    public OrganismSetup Randomize(OrganismSetup setup)
    {
        OrganismSetup result = new OrganismSetup();
        result.joints = new JointSetup[setup.joints.Length];
        result.muscles = new MuscleSetup[setup.muscles.Length];

        for (int i = 0; i < setup.joints.Length; i++)
        {
            result.joints[i] = joint.Randomize(setup.joints[i]);
        }
        for (int i = 0; i < setup.muscles.Length; i++)
        {
            result.muscles[i] = muscle.Randomize(setup.muscles[i]);
        }

        return result;
    }
}

[System.Serializable]
public class JointRandomizer{
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
}
[System.Serializable]
public class MuscleRandomizer{
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
}

