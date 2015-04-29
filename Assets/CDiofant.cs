using UnityEngine;
using System.Collections;

public struct chromosome {
	public int[] genes;
	public int fitness;
	public float likelihood;
	
	// Test for equality.
	public static bool operator ==(chromosome gn1, chromosome gn2) {
		for (int i = 0; i < 4; i++) {
			if (gn1.genes[i] != gn2.genes[i]) return false;
		}		
		return true;
	}
	
	public static bool operator !=(chromosome gn1, chromosome gn2) {
		for (int i = 0; i < 4; i++) {
			if (gn1.genes[i] != gn2.genes[i]) return true;
		}		
		return true;
	}
};

public class CDiofant {
	public int MAXPOP = 35;
	public int generationsNumber = 0;

	// Returns a given gene.
	
	int ca, cb, cc, cd;							// The coefficients.
	int result;
	chromosome[] population;							// Population.	
	System.Random rand = new System.Random();

	public CDiofant(int a, int b, int c, int d, int res) {
		ca = a;
		cb = b;
		cc = c; 
		cd = d;
		result = res;
		population = new chromosome[MAXPOP];
		for (int i = 0; i < MAXPOP; i++) {
			population[i].genes = new int[4];
			population[i].fitness = 0;
			population[i].likelihood = 0;
		}	
		
	}
	
	public chromosome GetGene(int i) { return population[i];}
	
	public int Solve() {
		int fitness = -1;
		
		for(int i = 0; i < MAXPOP; i++) {						// Fill the population with numbers between
			for (int j=0;j<4;j++) {						// 0 and the result.
				population[i].genes[j] = rand.Next(result + 1);
			}
		}
		
		fitness = CreateFitnesses();
		if (fitness > 0) {
			return fitness;
		}
		
		int iterations = 0;								// Keep record of the iterations.
		while (fitness != 0 || iterations < 5000) {		// Repeat until solution found, or over 50 iterations.
			GenerateLikelihoods();						// Create the likelihoods.
			CreateNewPopulation();
			fitness = CreateFitnesses();
			if (fitness > 0) {
				generationsNumber = iterations;
				return fitness;
			}
			
			iterations++;
		}
		
		return -1;
	}
	
	int Fitness(chromosome gn) {
		int total = ca * gn.genes[0] + cb * gn.genes[1] + cc * gn.genes[2] + cd * gn.genes[3];
		
		return gn.fitness = Mathf.Abs(total - result);
	}
	
	int CreateFitnesses() {
		float avgfit = 0;
		int fitness = 0;
		for(int i=0;i<MAXPOP;i++) {
			fitness = Fitness(population[i]);
			avgfit += fitness;
			if (fitness == 0) {
				return i;
			}
		}
		
		return 0;
	}
	
	float MultInv() {
		float sum = 0;
		
		for(int i=0;i<MAXPOP;i++) {
			sum += 1/((float)population[i].fitness);
		}
		
		return sum;
	}
	
	void GenerateLikelihoods() {
		float multinv = MultInv();
		
		float last = 0;
		for(int i=0;i<MAXPOP;i++) {
			population[i].likelihood = last = last + ((1/((float)population[i].fitness) / multinv) * 100);
		}
	}
	
	int GetIndex(float val) {
		float last = 0;
		for(int i=0;i<MAXPOP;i++) {
			if (last <= val && val <= population[i].likelihood) return i;
			else last = population[i].likelihood;
		}
		
		return 4;
	}
	
	chromosome Breed(int p1, int p2) {
		int crossover = rand.Next(1, 4);					// Create the crossover point (not first).
		int first = rand.Next(100);						// Which parent comes first?
		
		chromosome child = Clone(population[p1]);
							// Child is all first parent initially.
		
		int initial = 0, final = 3;						// The crossover boundaries.
		if (first < 50) initial = crossover;			// If first parent first. start from crossover.
		else final = crossover+1;						// Else end at crossover.
		
		for(int i=initial;i<final;i++) {				// Crossover!
			child.genes[i] = population[p2].genes[i];
			if (rand.Next(101) < 5) child.genes[i] = rand.Next(result + 1);
		}
		
		return child;									// Return the kid...
	}
	
	chromosome Clone(chromosome parent) {
		chromosome copy = new chromosome();
		copy.genes = (int[])parent.genes.Clone();
		copy.fitness = parent.fitness;
		copy.likelihood = parent.likelihood;
		
		return copy;
	}
	
	void CreateNewPopulation() {
		chromosome[] temppop = new chromosome[MAXPOP];
		
		for(int i = 0; i < MAXPOP; i++) {
			int parent1 = 0, parent2 = 0, iterations = 0;
			while(parent1 == parent2 || population[parent1] == population[parent2]) {
				parent1 = GetIndex((float)(rand.Next(101)));
				parent2 = GetIndex((float)(rand.Next(101)));
				if (++iterations > 25) break;
			}
			
			temppop[i] = Breed(parent1, parent2);		// Create a child.
		}
		
		for(int i = 0; i < MAXPOP; i++) population[i] = temppop[i];
	}
}
