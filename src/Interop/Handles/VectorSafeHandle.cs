using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Cead.Interop.Handles;

public partial class VectorSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [LibraryImport("Cead.lib")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeVector(IntPtr vector_ptr);

    public VectorSafeHandle() : base(true) { }
    protected override bool ReleaseHandle()
    {
        return FreeVector(handle);
    }
}
