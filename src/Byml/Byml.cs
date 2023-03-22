#pragma warning disable CA1419 // Provide a parameterless constructor that is as visible as the containing type for concrete types derived from 'System.Runtime.InteropServices.SafeHandle'

using Cead.Interop;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

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

public unsafe partial class Byml : SafeHandleMinusOneIsInvalid
{
    [LibraryImport(CeadLib)] private static partial Byml BymlFromBinary(byte* src, int src_len);
    [LibraryImport(CeadLib)] private static partial DataHandle BymlToBinary(IntPtr byml, [MarshalAs(UnmanagedType.Bool)] bool big_endian, int version);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8, EntryPoint = "FromText")] public static partial Byml FromTextCOM(string src);
    [LibraryImport(CeadLib)] private static partial StringHandle ToText(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial BymlType GetType(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial Hash GetHash(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial Array GetArray(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial IntPtr GetString(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial DataHandle GetBinary(IntPtr byml, out byte* dst, out int dst_len);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool GetBool(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial int GetInt(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial uint GetUInt(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial float GetFloat(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial long GetInt64(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial ulong GetUInt64(IntPtr byml);
    [LibraryImport(CeadLib)] private static partial double GetDouble(IntPtr byml);
    [LibraryImport(CeadLib, EntryPoint = "Hash")] private static partial IntPtr HashCOM(Hash value);
    [LibraryImport(CeadLib, EntryPoint = "Array")] private static partial IntPtr ArrayCOM(Array value);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial IntPtr String(string value);
    [LibraryImport(CeadLib)] private static partial IntPtr Binary(byte* value, int value_len);
    [LibraryImport(CeadLib)] private static partial IntPtr Bool([MarshalAs(UnmanagedType.Bool)] bool value);
    [LibraryImport(CeadLib)] private static partial IntPtr Int(int value);
    [LibraryImport(CeadLib)] private static partial IntPtr UInt(uint value);
    [LibraryImport(CeadLib)] private static partial IntPtr Float(float value);
    [LibraryImport(CeadLib)] private static partial IntPtr Int64(long value);
    [LibraryImport(CeadLib)] private static partial IntPtr UInt64(ulong value);
    [LibraryImport(CeadLib)] private static partial IntPtr Double(double value);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeByml(IntPtr byml);

    private Byml() : base(true) { }
    private Byml(IntPtr _handle) : base(true) => handle = _handle;

    public BymlType Type => GetType(handle);

    public static Byml FromBinary(ReadOnlySpan<byte> data)
    {
        fixed (byte* ptr = data) {
            return BymlFromBinary(ptr, data.Length);
        }
    }

    public static Byml FromText(string text)
    {
        return FromTextCOM(text);
    }

    public DataHandle ToBinary(bool bigEndian, int version = 2)
    {
        return BymlToBinary(handle, bigEndian, version);
    }

    public void ToBinary(string file, bool bigEndian, int version = 2)
    {
        using DataHandle dataHandle = BymlToBinary(handle, bigEndian, version);
        using FileStream fs = File.Create(file);
        fs.Write(dataHandle.AsSpan());
    }

    public StringHandle ToText()
    {
        return ToText(handle);
    }

    public void ToText(string file)
    {
        using StringHandle strHandle = ToText(handle);
        using FileStream fs = File.Create(file);
        fs.Write(strHandle.AsSpan());
    }

    public Hash GetHash() => GetHash(handle);
    public Array GetArray() => GetArray(handle);
    public string? GetString()
    {
        return Marshal.PtrToStringUTF8(GetString(handle));
    }
    public DataHandle GetBinary(out Span<byte> data)
    {
        DataHandle ptrHandle = GetBinary(handle, out byte* dst, out int dstLen);
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
    public Byml(Hash value) : this(HashCOM(value)) { }

    public static implicit operator Byml(Array value) => new(value);
    public Byml(Array value) : this(ArrayCOM(value)) { }

    public static implicit operator Byml(string value) => new(value);
    public Byml(string value) : this(String(value)) { }

    public static implicit operator Byml(byte[] value) => new(value.AsSpan());
    public static implicit operator Byml(Span<byte> value) => new(value);
    public Byml(Span<byte> value) : this(Binary(value)) { }

    public static implicit operator Byml(bool value) => new(value);
    public Byml(bool value) : this(Bool(value)) { }

    public static implicit operator Byml(int value) => new(value);
    public Byml(int value) : this(Int(value)) { }

    public static implicit operator Byml(uint value) => new(value);
    public Byml(uint value) : this(UInt(value)) { }

    public static implicit operator Byml(float value) => new(value);
    public Byml(float value) : this(Float(value)) { }

    public static implicit operator Byml(long value) => new(value);
    public Byml(long value) : this(Int64(value)) { }

    public static implicit operator Byml(ulong value) => new(value);
    public Byml(ulong value) : this(UInt64(value)) { }

    public static implicit operator Byml(double value) => new(value);
    public Byml(double value) : this(Double(value)) { }

    private static IntPtr Binary(Span<byte> value)
    {
        fixed (byte* ptr = value) {
            return Binary(ptr, value.Length);
        }
    }

    protected override bool ReleaseHandle()
    {
        return FreeByml(handle);
    }
}