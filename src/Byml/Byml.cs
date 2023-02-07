#pragma warning disable CA1419 // Provide a parameterless constructor that is as visible as the containing type for concrete types derived from 'System.Runtime.InteropServices.SafeHandle'

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
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8, EntryPoint = "FromText")] public static partial Byml FromTextCOM(string src);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial PtrHandle ToText(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] private static partial BymlType GetType(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial IntPtr GetHash(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial IntPtr GetArray(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial IntPtr GetString(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial PtrHandle GetBinary(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")][return: MarshalAs(UnmanagedType.Bool)] private static partial bool GetBool(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial int GetInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial uint GetUInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial float GetFloat(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial long GetInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial ulong GetUInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial double GetDouble(IntPtr byml);

    internal Byml() : base(IntPtr.Zero, true) { }

    public BymlType Type => GetType(handle);
    public override bool IsInvalid { get; }
    public bool IsRoot { get; set; } = false;

    public static Byml FromBinary(ReadOnlySpan<byte> data)
    {
        fixed (byte* ptr = data) {
            Byml byml = FromBinary(ptr, data.Length);
            byml.IsRoot = true;
            return byml;
        }
    }

    public static Byml FromText(string text)
    {
        Byml byml = FromTextCOM(text);
        byml.IsRoot = true;
        return byml;
    }

    public PtrHandle ToBinary(out Span<byte> data, bool bigEndian, int version = 2)
    {
        PtrHandle ptrHandle = ToBinary(handle, out byte* ptr, out int dstLen, bigEndian, version);
        data = new(ptr, dstLen);
        return ptrHandle;
    }

    public void ToBinary(string file, bool bigEndian, int version = 2)
    {
        using PtrHandle ptrHandle = ToBinary(handle, out byte* ptr, out int dstLen, bigEndian, version);
        using FileStream fs = File.Create(file);
        Span<byte> data = new(ptr, dstLen);
        fs.Write(data);
    }

    public byte[] ToBinary(bool bigEndian, int version = 2)
    {
        using PtrHandle ptrHandle = ToBinary(handle, out byte* ptr, out int dstLen, bigEndian, version);
        return new Span<byte>(ptr, dstLen).ToArray();
    }

    public PtrHandle ToText(out Span<byte> data)
    {
        PtrHandle ptrHandle = ToText(handle, out byte* dst, out int dst_len);
        data = new(dst, dst_len);
        return ptrHandle;
    }

    public string ToText()
    {
        using PtrHandle ptrHandle = ToText(handle, out byte* dst, out int _);
        return Marshal.PtrToStringUTF8((IntPtr)dst)!;
    }

    public void ToText(string file)
    {
        using PtrHandle ptrHandle = ToText(handle, out byte* dst, out int dst_len);
        using FileStream fs = File.Create(file);
        Span<byte> data = new(dst, dst_len);
        fs.Write(data);
    }

    public Hash GetHash() => GetHash(handle);
    public Array GetArray() => GetArray(handle);
    public string? GetString()
    {
        return Marshal.PtrToStringUTF8(GetString(handle));
    }
    public PtrHandle GetBinary(out Span<byte> data)
    {
        PtrHandle ptrHandle = GetBinary(handle, out byte* dst, out int dstLen);
        data = new(dst, dstLen);
        return ptrHandle;
    }
    public bool GetBool() => GetBool(handle);
    public int GetInt() => GetInt(handle);
    public uint GetUInt() => GetUInt(handle);
    public float GetFloat() => GetFloat(handle);
    public long GetInt64() => GetInt64(handle);
    public ulong GetUInt64() => GetUInt64(handle);
    public double GetDouble() => GetDouble(handle);

    protected override bool ReleaseHandle()
    {
        // Only dispose the resource if
        // the byml object is the root
        // of a byml structure.
        // 
        // Releasing children of the root
        // will cause data corruption
        // on other operations (such as yaml serialization).
        if (IsRoot) {
            return PtrHandle.FreePtr(handle);
        }

        return true;
    }
}