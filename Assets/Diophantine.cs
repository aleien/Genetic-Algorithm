using System;
using UnityEngine;

public struct cgene {
	
	public int[] alleles;		
	public int fitness;		
	public float likelihood;
	public float likelihoodAbs;		
	public int timesChosen;
	
	// Test for equality.
	
	public static bool operator ==(cgene gone, cgene gtwo) {			
		for (int i = 0; i < 4; i++) {				
			if (gone.alleles[i] != gtwo.alleles[i]) return false;				
		}			
		return true;			
	}
	
	public static bool operator !=(cgene gone, cgene gtwo) {			
		for (int i = 0; i < 4; i++) {				
			if (gone.alleles[i] != gtwo.alleles[i]) return true;				
		}			
		return true;			
	}
	
}

public class Diophantine : MonoBehaviour	{
	public const int MAXPOP = 5;
	
	int ca, cb, cc, cd;		
	int result;		
	cgene[] population;
	float generalFitness;
	System.Random rand;
	
	public Diophantine (int a, int b, int c, int d, int res)
	{
		ca = a;
		cb = b;
		cc = c;
		cd = d;
		result = res;	
		generalFitness = 0;
		population = new cgene[MAXPOP];
		for (int i = 0; i < MAXPOP; i++) {
			population[i].alleles = new int[4];
			population[i].fitness = 0;
			population[i].likelihood = 0;
			population[i].likelihoodAbs = 0;
			population[i].timesChosen = 0;
		}	
	}
	
	
	public cgene GetGene(int i) { 
		return population[i];
	}
	
	int Fitness(cgene gn) {			
		int total = ca * gn.alleles[0] + cb * gn.alleles[1] + cc * gn.alleles[2] + cd * gn.alleles[3];			
		return Mathf.Abs(total - result);
		
	}
	
	int CreateFitnesses() {			
		float avgfit = 0;			
		int fitness = 0;			
		for(int i = 0; i < MAXPOP; i++) {
			population[i].fitness = Fitness(population[i]);					
			fitness = population[i].fitness;	
			avgfit += fitness;				
			if (fitness == 0) {					
				return i;					
			}				
		}			
		return 0;			
	}
	
	float MultInv() {			
		float sum = 0;					
		for (int i = 0; i < MAXPOP; i++) {				
			sum += 1/((float)population[i].fitness);				
		}		
		
		return sum;
		
	}
	
	void GenerateLikelihoods() {			
		float multinv = MultInv();			
		float last = 0;
		
		for (int i = 0; i < MAXPOP; i++) {				
			last = last + ((1/((float)population[i].fitness) / multinv) * 100);	
			population[i].likelihood = last;	
			population[i].likelihoodAbs = (1/((float)population[i].fitness) / multinv) * 100;		
		}			
	}
	
	void CreateNewPopulation() {			
		cgene[] temppop= new cgene[MAXPOP];		
		
		for(int i=0;i<MAXPOP;i++) {				
			int parent1 = 0, parent2 = 0, iterations = 0;				
			while(parent1 == parent2 || population[parent1] == population[parent2]) {					
				parent1 = GetIndex((float)(rand.Next (101)));	
							
				parent2 = GetIndex((float)(rand.Next (101)));
								
				if (++iterations > (MAXPOP * MAXPOP)) break;					
			}				
			population[parent1].timesChosen++;	
			population[parent2].timesChosen++;	
			temppop[i] = Breed(parent1, parent2);  // Create a child.
			
		}			
		
		int max = 0;
		int maxValue = 0;
		for (int i = 0; i < MAXPOP; i++) {
			if (maxValue < population[i].timesChosen) {
				maxValue = population[i].timesChosen;
				max = i;
			}				
		}
		
		print ("Most chosen is: " + max + "; times: " + population[max].timesChosen + "; Fitness: " + population[max].fitness + "; likelihood: " + population[max].likelihoodAbs + "%");
		
		int attractiveIndex = GetMostAttractive();
		print ("Most attractive is: " + attractiveIndex + "; times: " + population[attractiveIndex].timesChosen + "; Fitness: " + population[attractiveIndex].fitness + "; likelihood: " + population[attractiveIndex].likelihoodAbs + "%");
				
		for(int i = 0; i < MAXPOP; i++) 
			population[i] = temppop[i];			
	}
	
	int GetIndex(float val) {			
		float last = 0;			
		
		for(int i = 0; i < MAXPOP; i++) {				
			if (last <= val && val <= population[i].likelihood) 
				return i;				
			else last = population[i].likelihood;
			
		}		
		return 4;
		
	}
	
	cgene Breed(int p1, int p2) {		
		int crossover = rand.Next(5);			
		int first = rand.Next(100);				
		cgene child = population[p1];			
		int initial = 0, final = 4;	
		
		for(int i = initial; i < final; i++) {	
			if (i < crossover)	{
				child.alleles[i] = population[p1].alleles[i];
			}		
			else {
				child.alleles[i] = population[p2].alleles[i];
			}
							
			if (rand.Next (101) < 5) 
				child.alleles[i] = rand.Next(result + 1);
			
		}		
		child.timesChosen = 0;
		return child;		
	}
	
	private float CalcGeneralFitness() {
		float result = 0;
		for (int i = 0; i < MAXPOP; i++) {
			result += population[i].fitness;
			
		}
		result = result/MAXPOP;
		return result;
	}
	
	private int GetMostAttractive() {
		int result = 0;
		float max = 0;
		for (int i = 0; i < MAXPOP; i++) {
			if (max < population[i].likelihoodAbs) {
				result = i;
				max = population[i].likelihoodAbs;
			}
				
		}
		return result;	
	}
	
	public int Solve() {
		rand = new System.Random();
		int fitness = -1;				
		
		for(int i = 0; i < MAXPOP; i++) {				
			for (int j = 0; j < 4; j++) {					
				population[i].alleles[j] = rand.Next(result + 1);					
			}				
		}			
		
		fitness = CreateFitnesses();
		if (fitness != 0) 
			return fitness;			
		int iterations = 0;
		generalFitness = CalcGeneralFitness();
		//print("P0 : " + generalFitness);
		
		while (fitness != 0 || iterations < 1000) {				
			GenerateLikelihoods();				
			CreateNewPopulation();	
			fitness = CreateFitnesses();
			generalFitness = CalcGeneralFitness();			
			//print("P" + iterations + " : " + generalFitness);
			if (fitness != 0) {					
				return fitness;					
			}				
				
				iterations++;			
			}			
			return -1;			
		}
		
	}

