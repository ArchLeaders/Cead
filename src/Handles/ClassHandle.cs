using Microsoft.Win32.SafeHandles;

namespace Cead.Handles;

public abstract class ClassHandle : SafeHandleMinusOneIsInvalid
{
    protected bool _isChild = true;

    protected ClassHandle() : base(true) { }
}
