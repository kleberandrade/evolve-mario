using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pattern
{
    public string expression;
    public float score;
}

public class LevelGenerator : MonoBehaviour
{
    [Header("Database")]
    public GameObject[] m_Slices;
    public List<Pattern> m_Patterns;

    [Header("Genetic Algorithm")]
    public string m_Filename = "exp";
    [Range(20, 1000)]
    public int m_PopulationSize = 50;
    [Range(100, 300)]
    public int m_ChromosomeLength = 200;
    [Range(2, 5)]
    public int m_TournamentSize = 3;
    [Range(0.0f, 1.0f)]
    public float m_ElitismRate = 0.1f;
    [Range(0.0f, 1.0f)]
    public float m_MutateRate = 0.01f;
    [Range(10, 10000)]
    public int m_MaxGeneration = 100;

    [Header("Debug")]
    public BuildLevelType m_BuildLevelType = BuildLevelType.All;
    public float m_BuildLevelWaitTime = 2.0f;
    public enum BuildLevelType { None, First, Best, All };
    public Text m_AreaText;

    private int m_Generation = 1;
    private int m_CurrentChromosome = 0;
    private float m_AvgFitness = 0.0f;
    private float m_MaxFitness = 0.0f;
    private List<Chromosome> m_Population = new List<Chromosome>();

    public void UpdateUI()
    {
        if (!m_AreaText) return;

        m_AreaText.text = "";
        m_AreaText.text += $"Generation: {m_Generation} / {m_MaxGeneration}\n";
        m_AreaText.text += $"Chromosome: {m_CurrentChromosome} / {m_PopulationSize}\n";
        m_AreaText.text += $"Average Fitness [{m_Generation}]: {m_AvgFitness:0.0}\n";
        m_AreaText.text += $"Best Fitness [{m_Generation}]: {m_MaxFitness:0.0}\n";
        m_AreaText.text += $"Elapsed Time: {Time.unscaledTime:0}";
    }

    private void Start()
    {
        Directory.CreateDirectory($"{Application.dataPath}/../Save");
        InitRandomPopulation();
        StartCoroutine(Loop());
    }

    public void InitRandomPopulation()
    {
        for (int i = 0; i < m_PopulationSize; i++)
        {
            var chromosome = new Chromosome(m_ChromosomeLength, m_Slices.Length);
            m_Population.Add(chromosome);
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    public IEnumerator Loop()
    {
        while (m_Generation < m_MaxGeneration)
        {
            m_CurrentChromosome = 0;
            while(m_CurrentChromosome < m_PopulationSize)
            {
                var chromosome = m_Population[m_CurrentChromosome];
                chromosome.Fitness = EvaluateLevel(chromosome);
                if (m_BuildLevelType == BuildLevelType.All ||  (m_BuildLevelType == BuildLevelType.First && m_CurrentChromosome == 0))
                {
                    DestroyLevel();
                    BuildLevel(chromosome);
                    yield return new WaitForSeconds(m_BuildLevelWaitTime);
                } 
                else if (m_BuildLevelType == BuildLevelType.Best && m_CurrentChromosome == m_PopulationSize - 1)
                {
                    DestroyLevel();
                    m_Population.Sort();
                    BuildLevel(m_Population[0]);
                    yield return new WaitForSeconds(m_BuildLevelWaitTime);
                }

                m_CurrentChromosome++;
            }
            NextGeneration();
        }
    }

    public void DestroyLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
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
        float fitness = 0.0f;
        string sentence = chromosome.ToString();
        foreach (Pattern pattern in m_Patterns)
             fitness += Regex.Matches(sentence, pattern.expression).Count * pattern.score;
        
        return fitness;
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
            chromosomes.Add(m_Population[i].Clone() as Chromosome);

        return chromosomes;
    }

    public void Save(bool append) 
    {
        if (string.IsNullOrEmpty(m_Filename)) return;

        Debug.Log($"{m_AvgFitness}\t{m_MaxFitness}");

        using (StreamWriter file = new StreamWriter($"{ Application.dataPath }/../Save/{m_Filename}.xls", append))
        {
            m_AvgFitness = m_Population.Average(x => x.Fitness);
            m_MaxFitness = m_Population.Max(x => x.Fitness);
            file.WriteLine($"{m_AvgFitness.ToString(System.Globalization.CultureInfo.CurrentCulture)}\t{m_MaxFitness.ToString(System.Globalization.CultureInfo.CurrentCulture)}");
        }
    }

    public void NextGeneration()
    {
        Save(m_Generation > 1);

        var newPopulation = Elitism();
        while (newPopulation.Count < m_PopulationSize)
        {
            Chromosome parent1 = Selection();
            Chromosome parent2 = Selection();

            int cutoff = Helper.RandomInt(1, m_ChromosomeLength - 1);

            Chromosome child1 = parent1.Crossover(parent2, cutoff);
            child1.Mutate(m_MutateRate);

            if (!newPopulation.Contains(child1)) newPopulation.Add(child1);

            if (newPopulation.Count < m_PopulationSize)
            {
                Chromosome child2 = parent2.Crossover(parent1, cutoff);
                child2.Mutate(m_MutateRate);

                if (!newPopulation.Contains(child2)) newPopulation.Add(child2);
            }
        }

        m_Population = newPopulation;
        m_Generation++;
    }
}
