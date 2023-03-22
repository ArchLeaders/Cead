using Cead;
using Cead.Interop.Handles;
using System.Diagnostics;

Stopwatch watch = Stopwatch.StartNew();

byte[] data = File.ReadAllBytes("D:\\Bin\\BymlLibrary\\ActorInfo.product.byml");
Byml byml = new(data);
File.WriteAllText("D:\\Bin\\BymlLibrary\\ActorInfo.product.yml", byml.ToText(out StringSafeHandle _));

watch.Stop();
Console.WriteLine(watch.ElapsedMilliseconds);
watch.Restart();

string text = File.ReadAllText("D:\\Bin\\BymlLibrary\\ActorInfo.product.yml");
byml = new(text);
File.WriteAllBytes("D:\\Bin\\BymlLibrary\\WRITE-ActorInfo.product.byml", byml.ToBinary(out VectorSafeHandle _, true).ToArray());

watch.Stop();
Console.WriteLine(watch.ElapsedMilliseconds);
watch.Restart();
