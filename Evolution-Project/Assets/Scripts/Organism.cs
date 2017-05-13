using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Organism : MonoBehaviour
{
    public OrganismSetup setup;
    public List<OrganismJoint> joints;

    public float MaxDistance { get; private set; }
	public float MaxX{get; private set;}
	public float MinX{get; private set;}
	public float MaxY{get; private set;}

    void Awake()
    {
        joints = new List<OrganismJoint>();
		gameObject.AddComponent<SortingGroup> ();
        MaxDistance = 0;
    }

	public void Spawn(Vector3 pos, GameObject jointPrefab, GameObject musclePrefab){
		Rect boundaries = new Rect(0,0,0,0);
		GameObject g;
		for (int i = 0; i < setup.joints.Count; i++)
		{
			JointSetup s = setup.joints[i];
			if (s == null)
				continue;

			g = Instantiate<GameObject>(jointPrefab);
			g.transform.parent = transform;

			OrganismJoint joint = g.GetComponent<OrganismJoint>();
			s.Apply(joint);

			joints.Add(joint);

			Rect jointBounds = new Rect(s.position - Vector2.one * joint.Radius, new Vector2(joint.Radius * 2, joint.Radius * 2));
			if (i == 0 || jointBounds.xMin < boundaries.xMin) boundaries.xMin = jointBounds.xMin;
			if (i == 0 || jointBounds.xMax > boundaries.xMax) boundaries.xMax = jointBounds.xMax;
			if (i == 0 || jointBounds.yMin < boundaries.yMin)
			{
				boundaries.yMin = jointBounds.yMin;
				boundaries.yMax = jointBounds.yMax;
			}
		}

		for (int i = 0; i < setup.muscles.Count; i++)
		{
			MuscleSetup s = setup.muscles[i];

			if (setup.joints [s.jointA] == null || setup.joints [s.jointB] == null)
				continue;

			g = Instantiate<GameObject>(musclePrefab);
			g.transform.parent = transform;

			OrganismMuscle muscle = g.GetComponent<OrganismMuscle>();
			s.Apply(muscle);
			muscle.jointA = joints[s.jointA];
			muscle.jointB = joints[s.jointB];
		}

		transform.position = pos + new Vector3(boundaries.center.x, -boundaries.yMin);
	}

    void Update()
    {
		MaxX = MaxDistance - 20;
		MinX = MaxDistance + 20;
		MaxY = 0;
        for (int i = 0; i < joints.Count; i++)
        {
            float pos = joints[i].transform.position.x + joints[i].Radius;
			float minPos = joints [i].transform.position.x - joints [i].Radius;
			float posY = joints [i].transform.position.y + joints [i].Radius;
			if (pos > MaxX)
            {
				MaxX = pos;
            }
			if (minPos < MinX) {
				MinX = minPos;
			}
			if (posY > MaxY) {
				MaxY = posY;
			}
        }
		if (MaxX > MaxDistance) {
			MaxDistance = MaxX;
		}
    }

	void OnDrawGizmos(){
		if (Application.isPlaying && setup != null) {
			Vector3 pos = new Vector3 (MaxDistance, transform.position.z, 0);
			if (setup.mutated) {
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere (pos, 0.07f);
			} else {
				Gizmos.color = Color.white;
				Gizmos.DrawLine (pos, pos + Vector3.up * 0.05f);
			}
		}
	}
}
