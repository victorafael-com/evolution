using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class OrganismJoint : MonoBehaviour, IPoolBehaviour
{
    public const float JointUnitSize = 32f / 100;

    [Header("Properties")]
    public float size;
    public float weight;
    public float friction;
    public float bounciness;

    [Header("Setup")]
    [SerializeField] private SpriteRenderer frictionDisplay;
    [SerializeField] private SpriteRenderer weightDisplay;
    [SerializeField] private SpriteRenderer bouncinessDisplay;

    public Rigidbody2D JointRigidbody { get; private set; }

    public float Radius
    {
        get { return size * JointUnitSize / 2; }
    }

    void Awake()
    {
		JointRigidbody = GetComponent<Rigidbody2D>();
    }

	public void Dettach(){
		JointRigidbody.velocity = Vector2.zero;
		JointRigidbody.angularVelocity = 0;
		JointRigidbody.isKinematic = true;
	}
	public void Process ()
	{
		JointRigidbody.mass = weight;

	    ColorManager.GetJointColors(this, frictionDisplay, weightDisplay, bouncinessDisplay);
		bouncinessDisplay.transform.parent.localScale = Vector3.one * bounciness;

		transform.localScale = Vector3.one * size;

		PhysicsMaterial2D mat = new PhysicsMaterial2D();
		mat.friction = friction;
		mat.bounciness = bounciness;

		GetComponent<Collider2D>().sharedMaterial = mat;

		JointRigidbody.isKinematic = false;

		gameObject.SetActive (true);
	}
}
