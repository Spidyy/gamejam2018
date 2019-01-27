
public enum Stat
{
    HP,
    STA,
    HUN,
    GOLD
}

public static class StatConsts
{
    public static readonly int[] k_startingValues = new int[]
    {
        100,            // hp
        100,            // sta
        100,            // hun
        0              // gold
    };

    public readonly static int[] k_maxValues = new int[]
    {
        100,            // hp
        100,            // sta
        100,            // hun
        int.MaxValue    // gold
    };

    public const float k_startHomeDistance = 100.0f;
}