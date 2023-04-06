using Cead.Handles;

namespace Cead;

public unsafe partial class ParameterObject : ClassHandle
{
    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}
