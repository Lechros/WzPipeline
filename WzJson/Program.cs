
using System.Diagnostics;
using WzJson.Gear;
using WzJson.Wz;

Stopwatch sw = new();

Console.WriteLine("Loading wz...");
sw.Restart();
WzLoader wz = new();
wz.Load(@"C:\Nexon\Maple");
sw.Stop();
Console.WriteLine("Done!" + $" ({sw.ElapsedMilliseconds}ms)");

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
