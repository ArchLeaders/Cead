using Cead.Interop;
using Cead.Interop.Handles;
using System.Runtime.InteropServices;

namespace Cead;

public enum BymlType : int
{
    Null,
    String,
    Binary,
    Array,
    Hash,
    Bool,
    Int,
    Float,
    UInt,
    Int64,
    UInt64,
    Double
}

public unsafe partial class Byml
{
    [LibraryImport("Cead.lib")]
    internal static unsafe partial void FromBinary(byte* src, int src_len, out IntPtr byml);

    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)]
    internal static unsafe partial void FromText(string src, out IntPtr byml);

    [LibraryImport("Cead.lib")]
    internal static unsafe partial void ToBinary(IntPtr byml, out VectorSafeHandle handle, out byte* dst, out int dst_len, [MarshalAs(UnmanagedType.Bool)] bool big_endian, int version);

    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)]
    internal static unsafe partial void ToText(IntPtr byml, out StringSafeHandle handle, out IntPtr ptr);


    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)]
    internal static unsafe partial BymlType GetType(IntPtr byml);

    internal readonly IntPtr _byml;

    public BymlType Type => GetType(_byml);

    public Byml(IntPtr byml) => _byml = byml;
    public Byml(ReadOnlySpan<byte> data)
    {
        DllManager.Load();

        fixed (byte* ptr = data) {
            FromBinary(ptr, data.Length, out _byml);
        }
    }

    public Byml(string text)
    {
        DllManager.Load();
        FromText(text, out _byml);
    }

    public Span<byte> ToBinary(out VectorSafeHandle handle, bool bigEndian, int version = 2)
    {
        ToBinary(_byml, out handle, out byte* ptr, out int dstLen, bigEndian, version);
        return new(ptr, dstLen);
    }

    public string ToText(out StringSafeHandle handle)
    {
        ToText(_byml, out handle, out IntPtr ptr);
        return Marshal.PtrToStringUTF8(ptr)!;
    }
}