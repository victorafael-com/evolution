using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using UnityEngine;
using System.Linq;
using System.Text;

public class OrganismManager : MonoBehaviour
{
    public OrganismSetup originalSetup;
    public OrganismRandomizer randomizer;
    [Header("Prefabs")]
    [SerializeField] private GameObject jointPrefab;
    [SerializeField] private GameObject musclePrefab;

	[Header("Simulation")]
    public int ammount = 50;
    public float simulationTime = 30;
	public float zOffset = 0.003f;
    [Range(0, 1)]
    public float survivors = 0.1f;

	public List<Organism> Organisms{ get; private set; }

    private StringBuilder outputBuilder;

	void Start ()
	{
	    outputBuilder = new StringBuilder();
        Simulate(null);
	}

    void OnDestroy()
    {
        File.WriteAllText(Application.dataPath + "/generationData.txt", outputBuilder.ToString());
    }

    void EndSimulation()
    {
        List<Organism> remaining = Organisms.OrderByDescending(x => x.MaxDistance)
            .Take(Mathf.RoundToInt(survivors * ammount))
            .ToList();

        var builder = new StringBuilder();
        for (int i = 0; i < remaining.Count; i++)
        {
            builder.Append(remaining[i].MaxDistance+"\t");
        }
        outputBuilder.AppendLine(builder.ToString());
        Simulate(remaining);
    }

    public void Simulate(List<Organism> parents)
    {
        List<Organism> newOrganisms = new List<Organism>();
        for (int i = 0; i < ammount; i++)
        {
            OrganismSetup setup;
            if (parents == null)
            {
				setup = randomizer.Randomize(originalSetup);
            }
            else
            {
				setup = randomizer.Randomize(parents[Random.Range(0, parents.Count)].setup);
            }

            newOrganisms.Add(
				SpawnOrganism(transform.position, setup, i)
                );
        }

        if (Organisms != null)
        {
            for (int i = 0; i < Organisms.Count; i++)
            {
                Destroy(Organisms[i].gameObject);
            }
        }
        Organisms = newOrganisms;

        Invoke("EndSimulation", simulationTime);
    }

	public Organism SpawnOrganism(Vector3 pos, OrganismSetup setup, int zIndex = 0)
    {
		GameObject g = new GameObject("Organism "+setup.method);
        Organism organism = g.AddComponent<Organism>();

        Rect boundaries = new Rect(0,0,0,0);

		for (int i = 0; i < setup.joints.Count; i++)
        {
            JointSetup s = setup.joints[i];
			if (s == null)
				continue;

            g = Instantiate<GameObject>(jointPrefab);
            g.transform.parent = organism.transform;

            OrganismJoint joint = g.GetComponent<OrganismJoint>();
            s.Apply(joint);

            organism.joints.Add(joint);

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
            g.transform.parent = organism.transform;

            OrganismMuscle muscle = g.GetComponent<OrganismMuscle>();
            s.Apply(muscle);
            muscle.jointA = organism.joints[s.jointA];
            muscle.jointB = organism.joints[s.jointB];
        }

		organism.transform.position = pos + new Vector3(boundaries.center.x, -boundaries.yMin) + Vector3.forward * zIndex * zOffset;
        organism.setup = setup;
        return organism;
    }
}
