using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

public class Chromosome : ICloneable, IComparable<Chromosome>
{
    private List<int> m_Genes = new List<int>();

    public int this[int index]
    {
        get { return m_Genes[index]; }
        set { m_Genes[index] = value; }
    }

    public int Length => m_Genes.Count;

    public float Fitness { get; set; } = 0.0f;

    private int m_Max;

    public Chromosome(int length, int max)
    {
        m_Max = max;
        for (int i = 0; i < length; i++)
            m_Genes.Add(Helper.RandomInt(max));
    }

    public Chromosome(Chromosome chromosome)
    {
        m_Max = chromosome.m_Max;
        foreach (int gene in chromosome.m_Genes)
            m_Genes.Add(gene);
    }

    public object Clone()
    {
        return new Chromosome(this);
    }

    public Chromosome Crossover(Chromosome otherParent, int cutoff)
    {
        Chromosome child = Clone() as Chromosome;
        for (int i = cutoff; i < Length; i++)
            child[i] = otherParent[i];

        return child;
    }

    public Chromosome Mutate(float mutationRate)
    {
        for (int i = 0; i < Length; i++)
        {
            if (Helper.RandomFloat() < mutationRate)
                m_Genes[i] = Helper.RandomInt(m_Max);
        }

        return this;
    }

    public int CompareTo(Chromosome other)
    {
        if (Fitness > other.Fitness)
            return -1;
        else if (Fitness < other.Fitness)
            return 1;
        else
            return 0;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        m_Genes.ForEach(gene => builder.Append((char)(65 + gene)));
        return builder.ToString();
    }
}
