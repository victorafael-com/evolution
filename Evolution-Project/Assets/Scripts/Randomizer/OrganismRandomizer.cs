using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class OrganismRandomizer
{
    public JointRandomizer joint;
    public MuscleRandomizer muscle;

	[Header("Mutations")]
	[Range(0,0.1f)]	public float mutationChance = 0.005f;
	[Space]
	[Range(0,1)] public float createJointWeight = 1;
	[Range(0,1)] public float removeJointWeight = 0.42f;
	[Range(0,1)] public float createMuscleWeight = 0.85f;
	[Tooltip("Creates a Joint if no muscle is available to be created at the Organism")]
	public bool fallbackFromMuscleToJoint = true;
	[Range(0,1)] public float removeMuscleWeight = 0.5f;

    public OrganismSetup Randomize(OrganismSetup setup)
    {
		OrganismSetup result = new OrganismSetup();
		result.joints = new List<JointSetup> ();
		result.muscles = new List<MuscleSetup> ();

		for (int i = 0; i < setup.joints.Count; i++)
        {
			result.joints.Add(joint.Randomize(setup.joints[i]));
        }
		for (int i = 0; i < setup.muscles.Count; i++)
        {
			result.muscles.Add(muscle.Randomize(setup.muscles[i]));
        }

		if (Random.value < mutationChance) {
			MutateOrganism (result);
		}

        return result;
    }
	public OrganismSetup FullRandomOrganism(OrganismSetup originalSetup){
		OrganismSetup result = new OrganismSetup();
		result.joints = new List<JointSetup> ();
		result.muscles = new List<MuscleSetup> ();

		for (int i = 0; i < originalSetup.joints.Count; i++) {
			result.joints.Add( joint.FullRandomize() );
		}

		for (int i = 0; i < originalSetup.muscles.Count; i++) {
			MuscleSetup original = originalSetup.muscles [i];
			MuscleSetup newMuscle = muscle.FullRandomize ();
			newMuscle.jointA = original.jointA;
			newMuscle.jointB = original.jointB;
			result.muscles.Add(newMuscle);
		}

		return result;
	}

	public void MutateOrganism(OrganismSetup organism){
		organism.mutated = true;

		float sum = createJointWeight + createMuscleWeight + removeMuscleWeight + removeJointWeight;
		float rand = Random.value * sum;

		if (rand < createJointWeight) {
			CreateJoint (organism);
			return;
		}
		rand -= createJointWeight;
		if (rand < createMuscleWeight) {
			CreateMuscle (organism);
			return;
		}
		rand -= createMuscleWeight;
		if (rand < removeJointWeight) {
			RemoveJoint (organism);
		} else {
			RemoveMuscle (organism);
		}
	}

	public void CreateJoint(OrganismSetup organism){
		int jointA = Random.Range (0, organism.joints.Count); //To which joint the new joint will be attached?
		int jointB = organism.joints.Count; //The new joint ID;

		JointSetup newJoint = joint.FullRandomize ();
		MuscleSetup newMuscle = muscle.FullRandomize ();
		newMuscle.jointA = jointA;
		newMuscle.jointB = jointB;

		organism.joints.Add (newJoint);
		organism.muscles.Add (newMuscle);
	}
	public void CreateMuscle(OrganismSetup organism){
		int joints = organism.joints.Count;
		int maxMuscles = 0;
		for (int i = 1; i < joints; i++) {
			maxMuscles += i;
		}

		if (maxMuscles >= organism.muscles.Count) {
			//no room for new muscles
			if (fallbackFromMuscleToJoint) {
				CreateJoint (organism);
				return;
			}
		}
		Dictionary<JointSetup, int> map = new Dictionary<JointSetup, int> ();
		for (int i = 0; i < joints; i++) {
			map.Add (organism.joints [i], i);
		}
		JointSetup[] jointsA = new List<JointSetup> (organism.joints).OrderBy(s => Random.value).ToArray();
		JointSetup[] jointsB = new List<JointSetup> (organism.joints).OrderBy(s => Random.value).ToArray();
		Debug.Log ("searching on jointss " + joints);
		for (int a = 0; a < joints; a++) {
			for (int b = 0; b < joints; b++) {
				if (jointsA[a] == jointsB[b])
					continue;

				bool found = false;
				int ja = map [jointsA [a]];
				int jb = map [jointsB [b]];

				for (int i = 0; i < organism.muscles.Count; i++) {
					if (organism.muscles [i].Match (ja, jb)) {
						found = true;
						Debug.Log ("Found muscle already using " + ja + " " + jb);
						break;
					}
				}

				if (!found) {

					MuscleSetup newMuscle = muscle.FullRandomize ();
					newMuscle.jointA = ja;
					newMuscle.jointB = jb;

					organism.muscles.Add(
						newMuscle
					);
					return;
				}
			}
		}
	}
	public void RemoveJoint(OrganismSetup organism){
		int lostJoint = Random.Range (0, organism.joints.Count);
		Debug.Log ("Removing random joint: "+lostJoint);

		RemoveSpecificJoint (organism, lostJoint);
	}
	private void RemoveSpecificJoint(OrganismSetup organism, int removed){
		Debug.Log ("Removed specific joint: "+removed);
		organism.joints.RemoveAt (removed);

		//Removes the empty reference inside the organism and muscles which references to it.
		List<MuscleSetup> fixedMuscles = new List<MuscleSetup> ();
		for (int i = 0; i < organism.muscles.Count; i++) {
			MuscleSetup ms = organism.muscles [i];
			if (ms.jointA == removed || ms.jointB == removed) {
				continue;
			}

			if (ms.jointA > removed)
				ms.jointA--;
			if (ms.jointB > removed)
				ms.jointB--;

			fixedMuscles.Add (ms);
		}
		organism.muscles = fixedMuscles;
	}
	public void RemoveMuscle(OrganismSetup organism){
		int removedMuscle = Random.Range (0, organism.muscles.Count);
		Debug.Log ("Removed Muscle");
		//Get the joints attached to the muscle
		int a = organism.muscles [removedMuscle].jointA;
		int b = organism.muscles [removedMuscle].jointB;
		//Removes the muscle
		organism.muscles.RemoveAt (removedMuscle);

		//Verifies if the joints attached to the muscle still have connections.
		bool foundA = false;
		bool foundB = false;

		for (int i = 0; i < organism.muscles.Count; i++) {
			MuscleSetup ms = organism.muscles [i];
			if (ms.jointA == a || ms.jointB == a) {
				foundA = true;
			}
			if (ms.jointB == b || ms.jointA == b) {
				foundB = true;
			}
			if (foundA && foundB)
				return;
		}

		if (!foundA) {
			RemoveSpecificJoint (organism, a);
		}
		if (!foundB) {
			if (!foundA && b > a) {
				b--;
			}
			RemoveSpecificJoint (organism, b);
		}
	}
}