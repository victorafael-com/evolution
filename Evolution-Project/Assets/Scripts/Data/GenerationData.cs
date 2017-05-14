using System.Collections.Generic;

public class GenerationData {
	public OrganismSetup[] organisms;
	public int[] survivors;
	public float[] maxDistances;
	public float MaxDistance;

	public int SurvivorCount{
		get{
			return survivors.Length;
		}
	}

	public GenerationData (int count){
		organisms = new OrganismSetup[count];
	}

	public void SetDistances(List<Organism> o){
		maxDistances = new float[o.Count];
		for (int i = 0; i < o.Count; i++) {
			maxDistances [i] = o [i].MaxDistance;
		}
	}
	public void SetSurvivors(List<Organism> o){
		survivors = new int[o.Count];
		for (int i = 0; i < o.Count; i++) {
			survivors [i] = System.Array.IndexOf (organisms, o [i].setup);
		}
	}
	public OrganismSetup[] GetSurvivors(){
		OrganismSetup[] setups = new OrganismSetup[survivors.Length];
		for (int i = 0; i < survivors.Length; i++) {
			setups [i] = GetSurvivor (i);
		}
		return setups;
	}
	public OrganismSetup GetSurvivor(int n){
		if (n >= survivors.Length)
			return null;

		return organisms [survivors [n]];
	}
}
