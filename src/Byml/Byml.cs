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
    [LibraryImport("Cead.lib")] private static partial PtrHandle ToText(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] private static partial BymlType GetType(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial IntPtr GetHash(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial Array GetArray(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial IntPtr GetString(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial PtrHandle GetBinary(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")][return: MarshalAs(UnmanagedType.Bool)] private static partial bool GetBool(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial int GetInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial uint GetUInt(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial float GetFloat(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial long GetInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial ulong GetUInt64(IntPtr byml);
    [LibraryImport("Cead.lib")] private static partial double GetDouble(IntPtr byml);

    [LibraryImport("Cead.lib", EntryPoint = "Hash")] private static partial IntPtr HashCOM(IntPtr value);
    [LibraryImport("Cead.lib", EntryPoint = "Array")] private static partial IntPtr ArrayCOM(Array value);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial IntPtr String(string value);
    [LibraryImport("Cead.lib")] private static partial IntPtr Binary(byte* value, int value_len);
    [LibraryImport("Cead.lib")] private static partial IntPtr Bool([MarshalAs(UnmanagedType.Bool)] bool value);
    [LibraryImport("Cead.lib")] private static partial IntPtr Int(int value);
    [LibraryImport("Cead.lib")] private static partial IntPtr UInt(uint value);
    [LibraryImport("Cead.lib")] private static partial IntPtr Float(float value);
    [LibraryImport("Cead.lib")] private static partial IntPtr Int64(long value);
    [LibraryImport("Cead.lib")] private static partial IntPtr UInt64(ulong value);
    [LibraryImport("Cead.lib")] private static partial IntPtr Double(double value);

    public static implicit operator IntPtr(Byml byml) => byml.handle;
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

    public static implicit operator Byml(Hash value) => new(value);
    public Byml(Hash value) : base(HashCOM(value.handle), true) { }

    public static implicit operator Byml(Array value) => new(value);
    public Byml(Array value) : base(ArrayCOM(value), true) { }

    public static implicit operator Byml(string value) => new(value);
    public Byml(string value) : base(String(value), true) { }

    public static implicit operator Byml(byte[] value) => new(value.AsSpan());
    public static implicit operator Byml(Span<byte> value) => new(value);
    public Byml(Span<byte> value) : base(Binary(value), true) { }

    public static implicit operator Byml(bool value) => new(value);
    public Byml(bool value) : base(Bool(value), true) { }

    public static implicit operator Byml(int value) => new(value);
    public Byml(int value) : base(Int(value), true) { }

    public static implicit operator Byml(uint value) => new(value);
    public Byml(uint value) : base(UInt(value), true) { }

    public static implicit operator Byml(float value) => new(value);
    public Byml(float value) : base(Float(value), true) { }

    public static implicit operator Byml(long value) => new(value);
    public Byml(long value) : base(Int64(value), true) { }

    public static implicit operator Byml(ulong value) => new(value);
    public Byml(ulong value) : base(UInt64(value), true) { }

    public static implicit operator Byml(double value) => new(value);
    public Byml(double value) : base(Double(value), true) { }

    private static IntPtr Binary(Span<byte> value)
    {
        fixed(byte* ptr = value) {
            return Binary(ptr, value.Length);
        }
    }

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