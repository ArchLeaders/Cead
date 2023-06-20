using Cead.Handles;
using System.Runtime.InteropServices;

namespace Cead;

public enum ParameterType : byte
{
    Bool,
    F32,
    Int,
    Vec2,
    Vec3,
    Vec4,
    Color,
    String32,
    String64,
    Curve1,
    Curve2,
    Curve3,
    Curve4,
    BufferInt,
    BufferF32,
    String256,
    Quat,
    U32,
    BufferU32,
    BufferBinary,
    StringRef,
}

public unsafe partial class ParameterIO : ParameterList
{
    [LibraryImport(CeadLib)] private static partial ParameterIO AampFromBinary(byte* src, int src_len);
    [LibraryImport(CeadLib)] private static partial DataHandle AampToBinary(IntPtr pio);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial ParameterIO AampFromText(string src);
    [LibraryImport(CeadLib)] private static partial StringHandle AampToText(IntPtr pio);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeAamp(IntPtr aamp);

    public static ParameterIO FromBinary(Span<byte> data)
    {
        fixed (byte* ptr = data) {
            return AampFromBinary(ptr, data.Length);
        }
    }

    public DataHandle ToBinary()
    {
        return AampToBinary(handle);
    }

    public static ParameterIO FromText(string str)
    {
        return AampFromText(str);
    }

    public StringHandle ToText()
    {
        return AampToText(handle);
    }

    protected override bool ReleaseHandle()
    {
        return FreeAamp(handle);
    }
}
