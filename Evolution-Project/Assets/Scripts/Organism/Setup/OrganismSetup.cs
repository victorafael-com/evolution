﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrganismSetup
{
	public bool mutated = false;
	public string method = "";

	public string Code{
		get{
			return "J" + joints.Count + "M" + muscles.Count;
		}
	}

    public List<JointSetup> joints;
    public List<MuscleSetup> muscles;
}