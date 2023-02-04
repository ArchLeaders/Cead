using System.Runtime.InteropServices;

namespace Cead.Interop;

public partial class PtrHandle : SafeHandle
{
    [LibraryImport("Cead.lib")]
    internal static unsafe partial void FreePtr(IntPtr ptr);

    public PtrHandle() : base(IntPtr.Zero, true) { }

    public override bool IsInvalid { get; }

    protected override bool ReleaseHandle()
    {
        FreePtr(handle);
        return true;
    }
}
