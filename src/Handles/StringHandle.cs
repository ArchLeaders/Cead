using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Text;

namespace Cead.Interop;

public unsafe partial class StringHandle : SafeHandleMinusOneIsInvalid
{
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FillString(IntPtr str, out byte* ptr, out int len);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeString(IntPtr handle);

    private byte* _ptr;
    private int _len = -1;
    private string? _cache;

    public StringHandle() : base(true) { }

    public static implicit operator Span<byte>(StringHandle data) => data.AsSpan();
    public Span<byte> AsSpan()
    {
        FillString(handle, out _ptr, out _len);
        return new(_ptr, _len);
    }

    /// <summary>
    /// Creates a managed copy of the data and disposes the reference
    /// </summary>
    /// <returns></returns>
    public static implicit operator string(StringHandle data) => data.ToString();
    public new string ToString() => ToString(true);
    public string ToString(bool dispose = true)
    {
        _cache ??= Encoding.UTF8.GetString(AsSpan());
        if (dispose) {
            Dispose();
        }

        return _cache;
    }

    protected override bool ReleaseHandle()
    {
        return FreeString(handle);
    }
}