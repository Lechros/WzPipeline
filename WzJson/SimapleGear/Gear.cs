using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WzJson.SimapleGear
{
    public class Gear
    {
        [JsonPropertyOrder(-100)]
        public string name = "";

        public int req_level;
        public int req_job;
        public int tuc;
        public int etuc;

        public int STR;
        public int DEX;
        public int INT;
        public int LUK;

        public int STR_multiplier;
        public int LUK_multiplier;
        public int INT_multiplier;
        public int DEX_multiplier;

        public int attack_power;
        public int magic_attack;

        // public int attack_power_multiplier;
        // public int magic_attack_multiplier;

        public int critical_rate;
        public int critical_damage;

        public int boss_damage_multiplier;
        public int damage_multiplier;
        public int final_damage_multiplier;

        public int ignored_defence;

        public int MHP;
        public int MMP;

        public int MHP_multiplier;
        public int MMP_multiplier;

        public int set_item_id;
        public int boss_reward;
        public int superior_eqp;
        public int joker_to_set_item;
        public int block_hammer;
        public int block_star;

        public bool ShouldSerializereq_level() => req_level != 0;
        public bool ShouldSerializereq_job() => req_job != 0;
        public bool ShouldSerializetuc() => tuc != 0;
        public bool ShouldSerializeetuc() => etuc != 0;

        public bool ShouldSerializeSTR() => STR != 0;
        public bool ShouldSerializeDEX() => DEX != 0;
        public bool ShouldSerializeINT() => INT != 0;
        public bool ShouldSerializeLUK() => LUK != 0;

        public bool ShouldSerializeSTR_multiplier() => STR_multiplier != 0;
        public bool ShouldSerializeLUK_multiplier() => LUK_multiplier != 0;
        public bool ShouldSerializeINT_multiplier() => INT_multiplier != 0;
        public bool ShouldSerializeDEX_multiplier() => DEX_multiplier != 0;

        public bool ShouldSerializeattack_power() => attack_power != 0;
        public bool ShouldSerializemagic_attack() => magic_attack != 0;

        // public bool ShouldSerializeattack_power_multiplier() => attack_power_multiplier != 0;
        // public bool ShouldSerializemagic_attack_multiplier() => magic_attack_multiplier != 0;

        public bool ShouldSerializecritical_rate() => critical_rate != 0;
        public bool ShouldSerializecritical_damage() => critical_damage != 0;

        public bool ShouldSerializeboss_damage_multiplier() => boss_damage_multiplier != 0;
        public bool ShouldSerializedamage_multiplier() => damage_multiplier != 0;
        public bool ShouldSerializefinal_damage_multiplier() => final_damage_multiplier != 0;

        public bool ShouldSerializeignored_defence() => ignored_defence != 0;

        public bool ShouldSerializeMHP() => MHP != 0;
        public bool ShouldSerializeMMP() => MMP != 0;

        public bool ShouldSerializeMHP_multiplier() => MHP_multiplier != 0;
        public bool ShouldSerializeMMP_multiplier() => MMP_multiplier != 0;

        public bool ShouldSerializeset_item_id() => set_item_id != 0;
        public bool ShouldSerializeboss_reward() => boss_reward != 0;
        public bool ShouldSerializesuperior_eqp() => superior_eqp != 0;
        public bool ShouldSerializejoker_to_set_item() => joker_to_set_item != 0;
        public bool ShouldSerializeblock_hammer() => block_hammer != 0;
        public bool ShouldSerializeblock_star() => block_star != 0;
    }
}
