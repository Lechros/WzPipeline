
using System.Diagnostics;
using WzJson.Gear;
using WzJson.Soul;
using WzJson.Wz;

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
    Console.WriteLine("3: export soul data");
    Console.WriteLine("4: exit");
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
                gl.Save(@"output\gear.json");
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
                ol.Save(@"output\itemoption.json");
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "3":
            {
                Console.WriteLine("Loading soul data...");
                sw.Restart();
                SoulLoader sl = new(wz);
                sl.Load();
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

                Console.WriteLine("Saving to file...");
                sw.Restart();
                sl.Save(@"output\soul.json");
                sw.Stop();
                Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");
            }
            break;
        case "4":
            exitFlag = true;
            break;
    }
}