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
	public OrganismSetup FullRandomOrganism(OrganismSetup originalSetup){
		OrganismSetup result = new OrganismSetup();
		result.joints = new JointSetup[originalSetup.joints.Length];
		result.muscles = new MuscleSetup[originalSetup.muscles.Length];

		for (int i = 0; i < originalSetup.joints.Length; i++) {
			result.joints [i] = joint.FullRandomize(originalSetup.joints[i]);
		}

		for (int i = 0; i < originalSetup.muscles.Length; i++) {
			result.muscles [i] = muscle.FullRandomize(originalSetup.muscles[i]);
		}

		return result;
	}
}