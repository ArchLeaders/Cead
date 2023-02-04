using Cead.Interop;
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

public unsafe partial class Byml : SafeHandle
{
    [LibraryImport("Cead.lib")] private static partial Byml FromBinary(byte* src, int src_len);
    [LibraryImport("Cead.lib")] private static partial PtrHandle ToBinary(IntPtr byml, out byte* dst, out int dst_len, [MarshalAs(UnmanagedType.Bool)] bool big_endian, int version);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial Byml FromText(string src);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial string ToText(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial BymlType GetType(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial Hash GetHash(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial Array GetArray(IntPtr byml);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial string GetString(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial PtrHandle GetBinary(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] [return: MarshalAs(UnmanagedType.Bool)] private static partial bool GetBool(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial int GetInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial uint GetUInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial float GetFloat(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial long GetInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial ulong GetUInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial double GetDouble(IntPtr byml);

    public Byml() : base(IntPtr.Zero, true) => DllManager.Load();
    internal Byml(IntPtr handle) : base(handle, true) { }

    public BymlType Type => GetType(handle);
    public override bool IsInvalid { get; }

    public Span<byte> ToBinary(out PtrHandle ptrHandle, bool bigEndian, int version = 2)
    {
        ptrHandle = ToBinary(handle, out byte* ptr, out int dstLen, bigEndian, version);
        return new(ptr, dstLen);
    }

    public string ToText()
    {
        return ToText(handle);
    }

    public Hash GetHash() => GetHash(handle);
    public Array GetArray() => GetArray(handle);
    public string GetString() => GetString(handle);
    public bool GetBool() => GetBool(handle);
    public int GetInt() => GetInt(handle);
    public uint GetUInt() => GetUInt(handle);
    public float GetFloat() => GetFloat(handle);
    public long GetInt64() => GetInt64(handle);
    public ulong GetUInt64() => GetUInt64(handle);
    public double GetDouble() => GetDouble(handle);
    public Span<byte> GetBinary(out PtrHandle ptrHandle)
    {
        ptrHandle = GetBinary(handle, out byte* dst, out int dstLen);
        return new(dst, dstLen);
    }

    protected override bool ReleaseHandle()
    {
        PtrHandle.FreePtr(handle);
        return true;
    }
}