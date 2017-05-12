using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public OrganismSetup setup;
    public List<OrganismJoint> joints;

    public float MaxDistance { get; private set; }

    void Awake()
    {
        joints = new List<OrganismJoint>();
        MaxDistance = 0;
    }

    void Update()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            float pos = joints[i].transform.position.x + joints[i].Radius;
            if (pos > MaxDistance)
            {
                MaxDistance = pos;
            }
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
