using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class OrganismMuscle : MonoBehaviour
{
    [Header("Parameters")]
    public float activeTime;
    public float interval;
    public float contractedDistance;
    public float relaxedDistance;
    public float frequency;
    public float startPhase;

    [Header("Setup")]
    public OrganismJoint jointA;
    public OrganismJoint jointB;

    [Header("Elements")]
    public SpriteRenderer sprite;

    private float t;
    private bool contracted = false;

    private SpringJoint2D joint;


    void Start()
    {
        ColorManager.GetMuscleColors(this, sprite);
        joint = jointA.gameObject.AddComponent<SpringJoint2D>();
        joint.connectedBody = jointB.JointRigidbody;
        joint.frequency = frequency;

        t = Mathf.Repeat(startPhase, activeTime + interval);
        if (t > activeTime)
        {
            joint.distance = relaxedDistance;
        }
        else
        {
			joint.distance = contractedDistance * relaxedDistance;
        }

        float minDistance = jointA.Radius + jointB.Radius;
		contractedDistance = Mathf.Max(contractedDistance * relaxedDistance, minDistance) / relaxedDistance;
    }

    public void Update()
    {
        t += Time.deltaTime;
        if (contracted && t > activeTime){
            t -= activeTime;
            contracted = false;
            joint.distance = relaxedDistance; // Expand
        }
        else if(!contracted && t > interval)
        {
            t -= interval;
            contracted = true;
			joint.distance = contractedDistance * relaxedDistance; // Contract
        }
    }

    public void LateUpdate()
    {
        transform.position = jointA.transform.position;
        Vector3 diff = jointB.transform.position - jointA.transform.position;
        transform.right = diff;

        float dist = Vector3.Distance(jointA.transform.position, jointB.transform.position);

        float stretch = relaxedDistance / dist;
        stretch = Mathf.Clamp(stretch, 0.1f, 2.3f);

        transform.localScale = new Vector3(dist, stretch);
    }
}
