using Cead.Interop;
using System.Runtime.InteropServices;

namespace Cead;

public enum Endianess
{
    Big, Little
}

public enum Mode
{
    Legacy, New
}

public unsafe partial class Sarc : SafeHandle
{
    [LibraryImport("Cead.lib")] internal static partial Sarc SarcFromBinary(byte* src, int src_len);
    [LibraryImport("Cead.lib")] internal static partial void SarcToBinary(IntPtr writer, out PtrHandle handle, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] internal static partial int GetNumFiles(IntPtr sarc);
    [LibraryImport("Cead.lib")] internal static partial int GetFileMapCount(IntPtr writer);
    [LibraryImport("Cead.lib")] internal static partial Endianess GetEndianess(IntPtr sarc);
    [LibraryImport("Cead.lib")] internal static partial Endianess SetEndianess(IntPtr writer, Endianess endianess);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] internal static partial bool GetFile(IntPtr sarc, string name, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] internal static partial IntPtr NewSarcWriter(Endianess endian, Mode mode);
    [LibraryImport("Cead.lib")] internal static partial IntPtr GetSarcWriter(IntPtr sarc);
    [LibraryImport("Cead.lib")] internal static partial void SetWriterMode(IntPtr writer, Mode mode);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] internal static partial bool SarcWriterGet(IntPtr writer, string name, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] internal static partial void AddSarcFile(IntPtr writer, string name, byte* src, int src_len);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] internal static partial void RemoveSarcFile(IntPtr writer, string name);
    [LibraryImport("Cead.lib")] internal static partial void ClearSarcFiles(IntPtr writer);

    public Sarc(IntPtr handle) : base(handle, true) { }
    public Sarc(Endianess endian = Endianess.Little, Mode mode = Mode.New) : base(-1, true)
    {
        _writer = NewSarcWriter(endian, mode);
    }

    public Span<byte> this[string key] {
        get {
            bool success = _writer != null ? SarcWriterGet((nint)_writer, key, out byte* ptr, out int len) : GetFile(handle, key, out ptr, out len);
            if (!success) {
                throw new KeyNotFoundException("Could not find a file with the name ''");
            }

            return new(ptr, len);
        }
    }

    public static Sarc FromBinary(Span<byte> data)
    {
        fixed (byte* ptr = data) {
            return SarcFromBinary(ptr, data.Length);
        }
    }

    public Span<byte> ToBinary(out PtrHandle handle)
    {
        SarcToBinary(Writer, out handle, out byte* dst, out int len);
        return new(dst, len);
    }

    public int Count => _writer != null ? GetFileMapCount((nint)_writer) : GetNumFiles(handle);
    public Endianess Endian {
        get => GetEndianess(handle);
        set => SetEndianess(Writer, value);
    }

    public void Add(string name, Span<byte> data)
    {
        fixed (byte* ptr = data) {
            AddSarcFile(Writer, name, ptr, data.Length);
        }
    }

    public void Remove(string name)
    {
        RemoveSarcFile(Writer, name);
    }

    public void Clear()
    {
        ClearSarcFiles(Writer);
    }

    private IntPtr? _writer;
    private IntPtr Writer {
        get {
            _writer ??= GetSarcWriter(handle);
            return (nint)_writer;
        }
    }

    public override bool IsInvalid { get; }

    protected override bool ReleaseHandle()
    {
        if (handle > -1) {
            PtrHandle.FreePtr(handle);
        }

        return true;
    }
}
