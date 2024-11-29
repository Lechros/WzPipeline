using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WzJson.Model
{
    public class GearReq
    {
        [JsonProperty(Order = 1)]
        public int level;
        [JsonProperty(Order = 2, PropertyName = "str")]
        public int STR;
        [JsonProperty(Order = 3, PropertyName = "luk")]
        public int LUK;
        [JsonProperty(Order = 4, PropertyName = "dex")]
        public int DEX;
        [JsonProperty(Order = 5, PropertyName = "int")]
        public int INT;
        //[JsonPropertyOrder(1)]
        // public int POP;
        [JsonProperty(Order = 6)]
        public int job;
        [JsonProperty(Order = 7)]
        public int specJob;

        public GearReq(int level = 0, int STR = 0, int LUK = 0, int DEX = 0, int INT = 0, int job = 0, int specJob = 0)
        {
            this.level = level;
            this.STR = STR;
            this.LUK = LUK;
            this.DEX = DEX;
            this.INT = INT;
            this.job = job;
            this.specJob = specJob;
        }

        public bool AnyReq()
        {
            return level > 0 || STR > 0 || LUK > 0 || DEX > 0 || INT > 0;
        }

        public enum JobType
        {
            beginner = 0,
            warrior = 1,
            magician = 2,
            archer = 4,
            theif = 8,
            pirates = 16
        }

        public bool ShouldSerializelevel() => level > 0;
        public bool ShouldSerializeSTR() => STR > 0;
        public bool ShouldSerializeLUK() => LUK > 0;
        public bool ShouldSerializeDEX() => DEX > 0;
        public bool ShouldSerializeINT() => INT > 0;
        public bool ShouldSerializejob() => job > 0;
        public bool ShouldSerializespecJob() => specJob > 0;
    }

    public class SpecialOption
    {
        [JsonPropertyOrder(1)]
        public int option;
        [JsonPropertyOrder(2)]
        public int level;

        public SpecialOption(int option, int level)
        {
            this.option = option;
            this.level = level;
        }
    }

    public class Gear
    {
        [JsonPropertyOrder(1)]
        public string name = "";
        [JsonPropertyOrder(2)]
        public string? desc;
        [JsonPropertyOrder(3)]
        public int icon;
        [JsonPropertyOrder(5)]
        public GearReq? req;

        // public bool cash;

        [JsonPropertyOrder(6)]
        public Dictionary<string, int> props = new();
        [JsonPropertyOrder(7)]
        public Dictionary<string, int> options = new();

        [JsonPropertyOrder(8)]
        public int tuc;
        [JsonPropertyOrder(9)]
        public int etuc;


        [JsonPropertyOrder(10)]
        public SpecialOption[]? pots;

        public bool ShouldSerializedesc() => !string.IsNullOrEmpty(desc);

        public bool ShouldSerializereq() => req != null && req.AnyReq();

        // public bool ShouldSerializecash() => cash;

        public bool ShouldSerializeprops() => props.Count > 0;

        public bool ShouldSerializeoptions() => options.Count > 0;

        public bool ShouldSerializetuc() => tuc > 0;

        public bool ShouldSerializeetuc() => etuc > 0;

        public bool ShouldSerializepots() => pots != null && pots.Length > 0 && pots[0].option != 0 && pots[0].level != 0;
    }
}
