using Cead.Handles;

namespace Cead;

public unsafe partial class ParameterList : ClassHandle
{
    public ParameterListMap? Lists { get; set; }
    public ParameterObjectMap? Objects { get; set; }

    protected override bool ReleaseHandle()
    {
        throw new NotImplementedException();
    }
}
