using Cead;
using Cead.Interop;

DllManager.LoadCead();

Span<byte> data = File.ReadAllBytes(@"D:\Bin\AampLibrary\AirWall.bxml");
ParameterIO pio = ParameterIO.FromBinary(data);

using FileStream fs = File.Create(@"D:\Bin\AampLibrary\AirWall.yml");
fs.Write(pio.ToText().AsSpan());