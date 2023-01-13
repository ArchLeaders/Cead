using Microsoft.Win32.SafeHandles;

namespace Cead.Interop
{
    public class VoidSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public VoidSafeHandle() : base(true) { }
        protected override bool ReleaseHandle()
        {
            // TODO Create common FreeResource function
            return Yaz0.FreeResource(handle);
        }
    }
}
