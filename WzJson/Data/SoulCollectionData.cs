using WzJson.Common;

namespace WzJson.Data;

public class SoulCollectionData : AbstractKeyValueData<SoulSkillNode>
{
    private const int MagnificentIndex = 8;

    private readonly Dictionary<int, int> soulSkill = new();
    private readonly Dictionary<int, bool> magnificent = new();

    public override SoulSkillNode this[string key]
    {
        set
        {
            base[key] = value;
            HandleAdd(key, value);
        }
    }

    public override void Add(string key, SoulSkillNode soulSkillNode)
    {
        base.Add(key, soulSkillNode);
        HandleAdd(key, soulSkillNode);
    }

    public bool ContainsSoul(int soulId)
    {
        return soulSkill.ContainsKey(soulId);
    }

    public int GetSoulSkillId(int soulId)
    {
        return soulSkill[soulId];
    }

    public bool IsMagnificentSoul(int soulId)
    {
        return magnificent[soulId];
    }

    private void HandleAdd(string key, SoulSkillNode soulSkillNode)
    {
        for (var i = 0; i < soulSkillNode.SoulList.Count; i++)
        {
            var isMagnificent = i == MagnificentIndex;
            var soulId = soulSkillNode.SoulList[i];
            var skillId = isMagnificent ? soulSkillNode.SoulSkillH!.Value : soulSkillNode.SoulSkill;
            soulSkill.Add(soulId, skillId);
            magnificent.Add(soulId, isMagnificent);
        }
    }
}

public class SoulSkillNode
{
    public int SoulSkill { get; set; }
    public int? SoulSkillH { get; set; }
    public List<int> SoulList { get; } = new();
}