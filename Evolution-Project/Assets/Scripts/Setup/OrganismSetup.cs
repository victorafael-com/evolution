﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrganismSetup
{
	public bool mutated = false;
    public List<JointSetup> joints;
    public List<MuscleSetup> muscles;
}