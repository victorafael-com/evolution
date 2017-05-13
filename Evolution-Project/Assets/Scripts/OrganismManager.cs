using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using UnityEngine;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class OrganismManager : MonoBehaviour
{
	public event UnityAction onRestartSimulation;

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

	private Dictionary<string, Transform> spawnGroups;

	void Start ()
	{
		spawnGroups = new Dictionary<string, Transform> ();
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
		if (onRestartSimulation != null)
			onRestartSimulation ();
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

	private Transform GetOrganismFamilyGroup(OrganismSetup setup){
		string code = setup.Code;
		if (spawnGroups.ContainsKey (code)) {
			return spawnGroups [code];
		} else {
			GameObject g = new GameObject (code);
			g.transform.position = Vector3.zero;
			spawnGroups.Add (code, g.transform);
			return g.transform;
		}
	}

	public Organism SpawnOrganism(Vector3 pos, OrganismSetup setup, int zIndex = 0)
    {
		GameObject g = new GameObject("Organism "+setup.method);
        Organism organism = g.AddComponent<Organism>();
		organism.setup = setup;
		organism.Spawn (pos, jointPrefab, musclePrefab);
		organism.transform.parent = GetOrganismFamilyGroup (setup);
        return organism;
    }
}
