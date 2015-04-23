using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CDiophantine dp = new CDiophantine(1, 2, 3, 4, 30);
		
		int ans = 0;
		ans = dp.Solve();
		if (ans == -1) {
			print("No solution found.");
		} else {
			gene gn = dp.GetGene(ans);
			
			print("The solution set to a + 2b + 3c + 4d = 30 is:");
			print("a = " + gn.alleles[0]);
			print("b = " + gn.alleles[1]);
			print("c = " + gn.alleles[2]);
			print("d = " + gn.alleles[3]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
