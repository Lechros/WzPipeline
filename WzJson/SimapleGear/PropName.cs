using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzJson.SimapleGear
{
    internal class PropName
    {
        public static string ToSimaple(string name)
        {
            switch(name)
            {
                case "incSTR": return "STR";
                case "incSTRr": return "STR_rate";
                case "incDEX": return "DEX";
                case "incDEXr": return "DEX_rate";
                case "incINT": return "INT";
                case "incINTr": return "INT_rate";
                case "incLUK": return "LUK";
                case "incLUKr": return "LUK_rate";
                case "incAllStat": return "allstat";
                case "incMHP": return "MHP";
                case "incMHPr": return "MHP_rate";
                case "incMMP": return "MMP";
                case "incMMPr": return "MMP_rate";
                case "incPAD": return "att";
                case "incPADr": return "att_rate";
                case "incMAD": return "matt";
                case "incMADr": return "matt_rate";
                case "damR": return "pdamage";
                case "bdR": return "boss_pdamage";
                case "imdR": return "armor_ignore";
                case "tuc": break;
                case "setItemID": return "set_item_id";
                case "bossReward": return "boss_reward";
                case "reqLevel": return "req_level";
                case "reqJob": return "req_job";
                case "superiorEqp": return "superior_eqp";
                case "jokerToSetItem": return "joker_to_set_item";
                case "blockGoldHammer": return "block_hammer";
            }
            return "";
        }
    }
}
