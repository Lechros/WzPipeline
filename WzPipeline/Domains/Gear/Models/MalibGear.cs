namespace WzPipeline.Domains.Gear.Models;

public class MalibGear
{
    public int Version => 2;
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Desc { get; set; }
    public required string Icon { get; set; }
    public GearType Type { get; set; }
    public required GearReq Req { get; set; }
    public GearAttribute Attributes { get; set; } = new();

    public Dictionary<GearPropType, int>? RawAttributes { get; set; }

    public GearOption BaseOption { get; set; } = new();

    public int ScrollUpgradeableCount { get; set; }

    public int PotentialGrade { get; set; }
    public GearPotential[]? Potentials { get; set; }

    public int ExceptionalUpgradeableCount { get; set; }

    public bool ShouldSerializeRawAttributes() => RawAttributes != null && RawAttributes.Count > 0;
    public bool ShouldSerializeBaseOption() => !BaseOption.IsEmpty();
    public bool ShouldSerializePotentials() => Potentials != null && Potentials.Length > 0;

    public bool IsRawAttributes()
    {
        return RawAttributes != null;
    }
}

public class GearAttribute
{
    public bool Only { get; set; }
    public int Trade { get; set; }
    public bool OnlyEquip { get; set; }
    public int Share { get; set; }

    public bool Superior { get; set; }
    public int AttackSpeed { get; set; }

    public int CanScroll { get; set; }
    public int CanStarforce { get; set; }
    public int CanAddOption { get; set; }
    public int CanPotential { get; set; }
    public int CanAdditionalPotential { get; set; }
    public int? FixedMaxStar { get; set; }

    public bool SpecialGrade { get; set; }
    public int Cuttable { get; set; }
    public int? CuttableCount { get; set; }
    public int? TotalCuttableCount { get; set; }
    public bool AccountShareTag { get; set; }
    public int SetItemId { get; set; }
    public bool Lucky { get; set; }
    public bool BossReward { get; set; }
    public List<string> Skills { get; set; } = [];

    public int GrowthExp { get; set; }
    public int GrowthLevel { get; set; }
    public string? DateExpire { get; set; }

    public bool ShouldSerializeSkills() => Skills.Count > 0;
}