using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Randomizable
{
    public float minValue;
    public float maxValue;
    public float variation;

	public float RandomVal{
		get{
			return Random.Range (minValue, maxValue);
		}
	}

    public float Randomize(float val)
    {
        float ret = val + Random.Range(-variation * .5f, variation * .5f);
        return Mathf.Clamp(ret, minValue, maxValue);
    }
}