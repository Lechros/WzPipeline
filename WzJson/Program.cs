
using System.Diagnostics;
using System.Reflection.Metadata;
using WzJson.Gear;
using WzJson.Item;
using WzJson.SetItem;
using WzJson.SimapleGear;
using WzJson.Soul;
using WzJson.Wz;

string outputRoot = Path.Join(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\output\");

Stopwatch sw = new();
bool exitFlag = false;

Console.WriteLine("Loading wz...");
sw.Restart();
WzLoader wz = new();
wz.Load(@"C:\Nexon\Maple");
sw.Stop();
Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

List<(string, Action)> options = new()
{
    ("export gear data", () =>
    {
        Console.WriteLine("Loading gear data...");
        sw.Restart();
        GearLoader gl = new(wz);
        gl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        gl.Save(Path.Join(outputRoot, @"gear.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export gear icons/origins (takes long)", () =>
    {
        Console.WriteLine("Loading gear data...");
        sw.Restart();
        GearLoader gl = new(wz);
        gl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        gl.SaveIcons(Path.Join(outputRoot, @"gearicon\"));
        gl.SaveOrigins(Path.Join(outputRoot, @"gearorigin.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export gear raw icons/origins (takes long)", () =>
    {
        Console.WriteLine("Loading gear data...");
        sw.Restart();
        GearLoader gl = new(wz);
        gl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        gl.SaveIcons(Path.Join(outputRoot, @"gearrawicon\"), true);
        gl.SaveOrigins(Path.Join(outputRoot, @"gearraworigin.json"), true);
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export item option data", () =>
    {
        Console.WriteLine("Loading item option data...");
        sw.Restart();
        ItemOptionLoader ol = new(wz);
        ol.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        ol.Save(Path.Join(outputRoot, @"itemoption.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export set item data", () =>
    {
        Console.WriteLine("Loading set item data...");
        sw.Restart();
        SetItemLoader sl = new(wz);
        sl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        sl.Save(Path.Join(outputRoot, @"setitem.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export soul data", () =>
    {
        Console.WriteLine("Loading soul data...");
        sw.Restart();
        SoulLoader sl = new(wz);
        sl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        sl.Save(Path.Join(outputRoot, @"soul.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("export items icons + origins (takes long)", () =>
    {
        Console.WriteLine("Loading item data...");
        sw.Restart();
        ItemLoader il = new(wz);
        il.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        il.Save(Path.Join(outputRoot, @"itemorigin.json"));
        il.SaveIcons(Path.Join(outputRoot, @"itemicon\"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
    ("[simaple] export gear data", () =>
    {
        Console.WriteLine("Loading gear data...");
        sw.Restart();
        SimapleGearLoader gl = new(wz);
        gl.Load();
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

        Console.WriteLine("Saving to file...");
        sw.Restart();
        gl.Save(Path.Join(outputRoot, @"simaple-gear.json"));
        sw.Stop();
        Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
    }
    ),
};

while(!exitFlag)
{
    Console.WriteLine("----------\nChoose option:");
    for(int i = 0; i < options.Count; i++)
    {
        Console.WriteLine((i + 1) + ". " + options[i].Item1);
    }
    if(int.TryParse(Console.ReadLine(), out int input) && (uint)(input - 1) < options.Count)
    {
        options[input - 1].Item2();
    }
    else
    {
        exitFlag = true;
    }
}