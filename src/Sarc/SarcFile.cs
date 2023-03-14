using System.Runtime.InteropServices;

namespace Cead;

/// <summary>
/// Readonly view of an <c>oead::SarcFile</c> instance
/// </summary>
public readonly unsafe partial struct SarcFile
{
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] internal static partial string GetName(IntPtr sarc_file);
    [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] internal static partial void SetName(IntPtr sarc_file, string name);
    [LibraryImport("Cead.lib")] internal static partial void GetData(IntPtr sarc_file, out byte* dst, out int dst_len);
    [LibraryImport("Cead.lib")] internal static partial void SetData(IntPtr sarc_file, byte* src, int src_len);

    private readonly IntPtr _handle;

    public string Name {
        get => GetName(_handle);
        set => SetName(_handle, value);
    }

    public Span<byte> GetData()
    {
        GetData(_handle, out byte* dst, out int len);
        return new(dst, len);
    }

    public void SetData(Span<byte> data)
    {
        fixed(byte* ptr = data) {
            SetData(_handle, ptr, data.Length);
        }
    }

    public static implicit operator SarcFile(IntPtr handle) => new(handle);
    public SarcFile(IntPtr handle) => _handle = handle;
}
