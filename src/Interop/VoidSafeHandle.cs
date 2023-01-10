using Microsoft.Win32.SafeHandles;

namespace Cead.Interop
{
    internal class VoidSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal VoidSafeHandle() : base(true) { }
        protected override bool ReleaseHandle()
        {
            // TODO Create common FreeResource function
            return Yaz0.FreeResource(handle);
        }
    }
}
