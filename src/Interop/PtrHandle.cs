using System.Runtime.InteropServices;

namespace Cead.Interop;

public partial class PtrHandle : SafeHandle
{
    [LibraryImport("Cead.lib")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static unsafe partial bool FreePtr(IntPtr ptr);

    public PtrHandle() : base(IntPtr.Zero, true) { }

    public override bool IsInvalid { get; }

    protected override bool ReleaseHandle()
    {
        return FreePtr(handle);
    }
}
