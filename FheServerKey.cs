using System;

namespace Fhe;

public sealed class FheServerKey : FheHandle
{
    internal FheServerKey(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.ServerKey_Destroy(handle);
}
