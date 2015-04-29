using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CDiofant dp = new CDiofant(1, 2, 3, 4, 30);
		
		int ans = 0;
		float time = Time.time;
		ans = dp.Solve();
		if (ans == -1) {
			print("No solution found.");
		} else {
			chromosome gn = dp.GetGene(ans);
			
			//print("The solution set to a + 2b + 3c + 4d = 30 is:");
			//print("a = " + gn.genes[0]);
			//print("b = " + gn.genes[1]);
			//print("c = " + gn.genes[2]);
			//print("d = " + gn.genes[3]);
			//print ("Population number: " + dp.MAXPOP);
			print ("Problem solved in " + dp.generationsNumber + " generations.");
			time = Time.time - time;
			print ("Time spent: " + Time.realtimeSinceStartup + ", s");
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
