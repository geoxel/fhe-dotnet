namespace Fhe;

public sealed class FheClientKey : FheHandle
{
    public FheClientKey(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.ClientKey_Destroy(handle);
}
