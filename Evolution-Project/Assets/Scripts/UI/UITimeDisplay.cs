using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeDisplay : MonoBehaviour {
	private OrganismManager manager;
	public Image lowerSand;
	public Image upperSand;
	private float startTime = 0;
	public float bodyRotationTime = 0.25f;
	public AnimationCurve bodyRotationCurve;

	void Awake(){
		manager = FindObjectOfType<OrganismManager> ();
		manager.onRestartSimulation += Manager_onRestartSimulation;
	}

	void Manager_onRestartSimulation ()
	{
		lowerSand.fillAmount = 1;
		upperSand.fillAmount = 0;
		startTime = Time.time;
		transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - bodyRotationTime < startTime) {
			float lerp = bodyRotationCurve.Evaluate( Mathf.InverseLerp (startTime, startTime + bodyRotationTime, Time.time) );
			if (lerp < 0.5f) {
				lowerSand.fillAmount = 1;
				upperSand.fillAmount = 0;

				transform.eulerAngles = Vector3.forward * lerp * 180;

			} else {
				lowerSand.fillAmount = 0;
				upperSand.fillAmount = 1;

				transform.eulerAngles = Vector3.forward * (180 + (lerp * 180));
			}
		} else {
			transform.rotation = Quaternion.identity;
			float lerp = Mathf.InverseLerp (startTime, startTime + manager.simulationTime, Time.time);
			upperSand.fillAmount = 1 - lerp;
			lowerSand.fillAmount = lerp;
		}
	}
}
