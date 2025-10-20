using System;

namespace Fhe;

public sealed class FheCompactPkeCrs : FheHandle
{
    public FheCompactPkeCrs(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.CompactPkeCrs_Destroy(handle);

    public static unsafe FheCompactPkeCrs Deserialize(byte[] data, ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper2(SafeNativeMethods.CompactPkeCrs_Deserialize, buffer_view, serialized_size_limit);
        }
    }

    private static FheCompactPkeCrs Oper2<A, B>(OperFunc<A, B> func, A a, B b) =>
        new FheCompactPkeCrs(Oper2n(func, a, b));
}
