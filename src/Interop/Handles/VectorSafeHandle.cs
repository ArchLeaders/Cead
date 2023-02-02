using Microsoft.Win32.SafeHandles;

namespace Cead.Interop
{
    public class VectorSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public VectorSafeHandle() : base(true) { }
        protected override bool ReleaseHandle()
        {
            return Yaz0.FreeVector(handle);
        }
    }
}
