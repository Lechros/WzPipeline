using Newtonsoft.Json;
using WzPipeline.Domains.Gear.Models;

namespace WzPipeline.Domains.Soul.Models;

public class MalibSoul
{
    [JsonIgnore] public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Skill { get; init; }
    public int ChargeFactor { get; init; }
    public bool Magnificent { get; init; }
    public GearOption? Option { get; set; }
    public RandomOptions? Options { get; set; }

    public class RandomOptions
    {
        public required GearOption AttackPower { get; init; }
        public required GearOption MagicPower { get; init; }
        public required GearOption AllStat { get; init; }
        public required GearOption MaxHp { get; init; }
        public required GearOption CriticalRate { get; init; }
        public required GearOption BossDamage { get; init; }
        public required GearOption IgnoreMonsterArmor { get; init; }
    }
}