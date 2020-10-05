using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Database")]
    public GameObject[] m_Slices;

    [Header("Genetic Algorithm")]
    public string m_Filename = "experiment1";
    public int m_PopulationSize = 50;
    public int m_ChromosomeLength = 200;
    public int m_TournamentSize = 3;
    [Range(0.0f, 1.0f)]
    public float m_ElitismRate = 0.1f;
    public float m_MutateRate = 0.01f;
    public int m_MaxGeneration = 100;

    private int m_Generation = 0;
    private List<Chromosome> m_Population = new List<Chromosome>();

    private void Start()
    {
        InitRandomPopulation();
    }

    public void InitRandomPopulation()
    {
        for (int i = 0; i < m_PopulationSize; i++)
        {
            var chromosome = new Chromosome(m_ChromosomeLength, m_Slices.Length);
            m_Population.Add(chromosome);
        }
    }

    public void BuildLevel(Chromosome chromosome)
    {
        for (int i = 0; i < m_ChromosomeLength; i++)
        {
            var index = chromosome[i];
            var slice = m_Slices[index];
            var position = new Vector3(i, 0, 0);
            Instantiate(slice, position, Quaternion.identity, transform);
        }
    }

    public float EvaluateLevel(Chromosome chromosome)
    {
        return 0.0f;
    }

    public Chromosome Selection()
    {
        var candidates = new List<Chromosome>();
        for (int i = 0; i < m_TournamentSize; i++)
        {
            int index = Helper.RandomInt(m_PopulationSize);
            candidates.Add(m_Population[index]);
        }

        candidates.Sort();
        return candidates[0];
    }

    public List<Chromosome> Elitism()
    {
        var count = (int)(m_PopulationSize * m_ElitismRate);
        m_Population.Sort();

        var chromosomes = new List<Chromosome>();
        for (int i = 0; i < count; i++)
            chromosomes.Add((Chromosome)m_Population[i].Clone());

        return chromosomes;
    }

    public void Save(bool append) 
    {
        if (string.IsNullOrEmpty(m_Filename)) return;

        using (StreamWriter file = new StreamWriter($"{m_Filename}.xls", append))
        {
            float average = m_Population.Average(x => x.Fitness);
            float max = m_Population.Max(x => x.Fitness);
            file.WriteLine($"{average}\t{max}");
        }
    }

    public void NextGeneration()
    {
        var newPopulation = new List<Chromosome>();
        while (newPopulation.Count < m_PopulationSize)
        {
            Chromosome parent1 = Selection();
            Chromosome parent2 = Selection();

            int cutoff = Helper.RandomInt(1, m_ChromosomeLength - 1);

            Chromosome child1 = parent1.Crossover(parent2, cutoff).Mutate(m_MutateRate);
            newPopulation.Add(child1);

            if (newPopulation.Count < m_PopulationSize)
            {
                Chromosome child2 = parent2.Crossover(parent1, cutoff).Mutate(m_MutateRate);
                newPopulation.Add(child2);
            }
        }

        Save(m_Generation > 0);
        m_Population = new List<Chromosome>(newPopulation);
        m_Generation++;
    }
}
