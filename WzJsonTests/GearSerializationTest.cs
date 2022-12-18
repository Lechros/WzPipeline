using Newtonsoft.Json;
using WzJson.Gear;

namespace WzJsonTests
{
    [TestClass]
    public class GearSerializationTest
    {
        [TestMethod]
        public void BasicTest()
        {
            Gear gear = new();
            gear.name = "테스트 장비 11";
            gear.desc = "";
            gear.icon = 1001234;
            gear.origin = new int[] { 10, -20 };
            gear.req = new GearReq(
                level: (GearReq.JobType)((int)GearReq.JobType.theif + (int)GearReq.JobType.pirates)
                );
            gear.props.Add("tradeok", 1);
            gear.props.Add("some_value", 10);
            gear.pots = new SpecialOption[]
            {
                new(60001, 20)
            };
            
            string output = JsonConvert.SerializeObject(gear);
            Assert.AreEqual(@"{""name"":""테스트 장비 11"",""icon"":1001234,""origin"":[10,-20],""req"":{""level"":24},""props"":{""tradeok"":1,""some_value"":10},""pots"":[{""option"":60001,""level"":20}]}", output);
        }
    }
}