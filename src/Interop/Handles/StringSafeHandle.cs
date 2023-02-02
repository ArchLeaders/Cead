using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Cead.Interop.Handles;

public partial class StringSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [LibraryImport("Cead.lib")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeString(IntPtr string_ptr);

    public StringSafeHandle() : base(true) { }
    protected override bool ReleaseHandle()
    {
        return FreeString(handle);
    }
}
