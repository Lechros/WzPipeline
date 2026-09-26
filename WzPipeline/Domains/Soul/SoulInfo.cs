namespace WzPipeline.Domains.Soul;

/// <summary>
/// SoulCollection.img/{}/soulList 내 하나의 항목의 정보
/// </summary>
public class SoulInfo
{
    public required int[] SoulIds { get; init; }

    public static int GetSkillOptionId(int soulId)
    {
        return soulId % 1000 + 1;
    }
}