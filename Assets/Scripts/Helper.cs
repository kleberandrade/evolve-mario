using System;

public class Helper
{ 
    private Helper() { }

    private static readonly int SEED = 11;
    
    private static Random m_Random = null;

    private static void Initialize()
    {
        if (m_Random == null) 
            m_Random = new Random(SEED);
    }

    public static int RandomInt(int max)
    {
        Initialize();
        return m_Random.Next(max);
    }

    public static int RandomInt(int min, int max)
    {
        Initialize();
        return m_Random.Next(min, max);
    }

    public static float RandomFloat()
    {
        Initialize();
        return (float)m_Random.NextDouble();
    }
}
