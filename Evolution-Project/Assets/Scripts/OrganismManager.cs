using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class OrganismManager : MonoBehaviour
{
	public event UnityAction onRestartSimulation;

    public OrganismSetup originalSetup;
    public OrganismRandomizer randomizer;
    [Header("Prefabs")]
    [SerializeField] private GameObject jointPrefab;
    [SerializeField] private GameObject musclePrefab;
	[SerializeField] private GameObject organismPrefab;
	public static PrefabPool<OrganismJoint> JointPool;
	public static PrefabPool<OrganismMuscle> MusclePool;
	public int maxOrganismCreatedPerFrame = 42;

	[Header("Simulation")]
    public int ammount = 50;
    public float simulationTime = 30;
	public float zOffset = 0.003f;
    [Range(0, 1)]
    public float survivors = 0.1f;

	public List<Organism> Organisms{ get; private set; }

	private Dictionary<string, Transform> spawnGroups;

	private GenerationData currentGenerationData;

	void Start ()
	{
		JointPool = new PrefabPool<OrganismJoint> (transform, jointPrefab, 600, 50);
		MusclePool = new PrefabPool<OrganismMuscle> (transform, musclePrefab, 400, 50);

		spawnGroups = new Dictionary<string, Transform> ();
        StartSimulation();
	}
	void StartSimulation(){
		if (Organisms != null) {
			for (int i = 0; i < Organisms.Count; i++) {
				Destroy (Organisms [i].gameObject);
			}
		}
		Organisms = new List<Organism> ();
		for(int i = 0; i < ammount; i++){
			GameObject g = Instantiate<GameObject> (organismPrefab);
			g.transform.position = transform.position;
			g.GetComponent<UnityEngine.Rendering.SortingGroup> ().sortingOrder = i;
			Organisms.Add (g.GetComponent<Organism> ());
		}
		Simulate (null);
	}

    void EndGeneration()
    {
		List<Organism> remaining = Organisms.OrderByDescending(x => x.MaxDistance)
            .Take(Mathf.RoundToInt(survivors * ammount))
			.ToList();

		currentGenerationData.MaxDistance = remaining [0].MaxDistance;
		currentGenerationData.SetDistances (Organisms);
		currentGenerationData.SetSurvivors (remaining);

		for (int i = 0; i < Organisms.Count; i++) {
			Organisms [i].Kill ();
		}

		Simulate(currentGenerationData.GetSurvivors());
    }

	public void Simulate(OrganismSetup[] parents)
    {
		currentGenerationData = new GenerationData (ammount);

        for (int i = 0; i < ammount; i++)
        {
            OrganismSetup setup;
            if (parents == null)
            {
				setup = randomizer.Randomize(originalSetup);
            }
            else
            {
				setup = randomizer.Randomize(parents[Random.Range(0, parents.Length)]);
            }

			currentGenerationData.organisms [i] = setup;
            
			SpawnOrganism (transform.position, setup, i);
        }

		if (onRestartSimulation != null)
			onRestartSimulation ();
		Invoke("EndGeneration", simulationTime);
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

	public Organism SpawnOrganism(Vector3 pos, OrganismSetup setup, int index)
    {
		Organism organism = Organisms [index];
		organism.setup = setup;
		organism.Spawn (pos);
		organism.transform.parent = GetOrganismFamilyGroup (setup);
        return organism;
    }
}
