using Microsoft.Win32.SafeHandles;

namespace Cead.Handles;

public abstract class BymlHandle : SafeHandleMinusOneIsInvalid
{
    protected bool _isChild = true;

    protected BymlHandle(bool ownsHandle) : base(ownsHandle) { }
}
