namespace Fhe;

public sealed class CompactPublicKey : FheHandle
{
    private CompactPublicKey(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.CompactPublicKey.Destroy(handle);

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.CompactPublicKey.Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer));
        
        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe CompactPublicKey Deserialize(byte[] data, ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper2(SafeNativeMethods.CompactPublicKey.Deserialize, buffer_view, serialized_size_limit);
        }
    }

    private static CompactPublicKey Oper2<A, B>(OperFunc<A, B> func, A a, B b) =>
        new CompactPublicKey(Oper2n(func, a, b));
}
