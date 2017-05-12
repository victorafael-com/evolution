using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class Joint : MonoBehaviour
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
        JointRigidbody.mass = weight;

        PhysicsMaterial2D mat = new PhysicsMaterial2D();
        mat.friction = friction;
        mat.bounciness = bounciness;

        GetComponent<Collider2D>().sharedMaterial = mat;
    }

	void Start ()
	{
	    ColorManager.GetJointColors(this, frictionDisplay, weightDisplay, bouncinessDisplay);
	    bouncinessDisplay.transform.parent.localScale = Vector3.one * bounciness;
	}
}
