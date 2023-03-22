using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Cead.Interop;

public unsafe partial class DataHandle : SafeHandleMinusOneIsInvalid
{
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FillData(IntPtr vector, out byte* ptr, out int len);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeVector(byte* ptr);

    private byte* _ptr;
    private int _len = -1;
    private byte[]? _cache;

    public DataHandle() : base(true) { }

    public static implicit operator Span<byte>(DataHandle data) => data.AsSpan();
    public Span<byte> AsSpan()
    {
        FillData(handle, out _ptr, out _len);
        return new(_ptr, _len);
    }

    /// <summary>
    /// Creates a managed copy of the data and disposes the reference
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray(bool dispose = true)
    {
        _cache ??= AsSpan().ToArray();
        if (dispose) {
            Dispose();
        }

        return _cache;
    }

    protected override bool ReleaseHandle()
    {
        return FreeVector(_ptr);
    }
}