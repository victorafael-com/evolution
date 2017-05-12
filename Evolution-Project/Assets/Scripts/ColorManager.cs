using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private static ColorManager instance;
    [Header("Joint")]
    public Gradient frictionRange;
    public float maxFriction;
    public Gradient weightRange;
    public float maxWeight;
    public Gradient bouncinessRange;
    public float maxBounciness;
    [Header("Muscle")]
    public Gradient activityRange;
    public float maxActivity;

    void Awake()
    {
        instance = this;
    }

    public static void GetJointColors(OrganismJoint joint, SpriteRenderer friction, SpriteRenderer weight, SpriteRenderer bounciness)
    {
        friction.color = instance.GetColor(instance.frictionRange, instance.maxFriction, joint.friction);
        weight.color = instance.GetColor(instance.weightRange, instance.maxWeight, joint.weight);
        bounciness.color = instance.GetColor(instance.bouncinessRange, instance.maxBounciness, joint.bounciness);
    }

    public static void GetMuscleColors(OrganismMuscle muscle, SpriteRenderer activity)
    {
        activity.color = instance.GetColor(instance.activityRange, instance.maxActivity, muscle.frequency);
    }

    private Color GetColor(Gradient gradient, float max, float value)
    {
        return gradient.Evaluate(Mathf.InverseLerp(0, max, value));
    }
}
