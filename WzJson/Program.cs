
using System.Diagnostics;
using WzJson.Gear;
using WzJson.SetItem;
using WzJson.SimapleGear;
using WzJson.Soul;
using WzJson.Wz;

string outputRoot = Path.Join(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");

Stopwatch sw = new();
bool exitFlag = false;

Console.WriteLine("Loading wz...");
sw.Restart();
WzLoader wz = new();
wz.Load(@"C:\Nexon\Maple");
sw.Stop();
Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

while(!exitFlag)
{
    Console.WriteLine("----------\nChoose option:");
    Console.WriteLine("1: export gear data");
    Console.WriteLine("2: export item option data");
    Console.WriteLine("3: export set item data");
    Console.WriteLine("4: export soul data");
    Console.WriteLine("5: [simaple] export gear data");
    Console.WriteLine("6: exit");
    switch(Console.ReadLine())
    {
        case "1":
            {
                Console.WriteLine("Loading gear data...");
                sw.Restart();
                GearLoader gl = new(wz);
                gl.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                gl.Save(Path.Join(outputRoot, @"output\gear.json"));
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "2":
            {
                Console.WriteLine("Loading item option data...");
                sw.Restart();
                ItemOptionLoader ol = new(wz);
                ol.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                ol.Save(Path.Join(outputRoot, @"output\itemoption.json"));
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "3":
            {
                Console.WriteLine("Loading set item data...");
                sw.Restart();
                SetItemLoader sl = new(wz);
                sl.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                sl.Save(Path.Join(outputRoot, @"output\setitem.json"));
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "4":
            {
                Console.WriteLine("Loading soul data...");
                sw.Restart();
                SoulLoader sl = new(wz);
                sl.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                sl.Save(Path.Join(outputRoot, @"output\soul.json"));
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "5":
            {
                Console.WriteLine("Loading gear data...");
                sw.Restart();
                SimapleGearLoader gl = new(wz);
                gl.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                gl.Save(Path.Join(outputRoot, @"output\simaple-gear.json"));
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "6":
            exitFlag = true;
            break;
    }
}