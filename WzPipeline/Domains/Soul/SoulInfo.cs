namespace WzPipeline.Domains.Soul;

public class SoulInfo
{
    public const int MagnificentIndex = 8;

    public int SoulId { get; init; }
    public int SkillId { get; init; }
    public int Index { get; init; }

    public bool IsMagnificent => IsMagnificentIndex(Index);

    public static bool IsMagnificentIndex(int index)
    {
        return index == MagnificentIndex;
    }
}