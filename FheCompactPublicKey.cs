using System;

namespace Fhe;

public sealed class FheCompactPublicKey : FheHandle
{
    public FheCompactPublicKey(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.CompactPublicKey_Destroy(handle);

    public static unsafe FheCompactPublicKey Deserialize(byte[] data, ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper2(SafeNativeMethods.CompactPublicKey_Deserialize, buffer_view, serialized_size_limit);
        }
    }

    private static FheCompactPublicKey Oper2<A, B>(OperFunc<A, B> func, A a, B b) =>
        new FheCompactPublicKey(Oper2n(func, a, b));
}
