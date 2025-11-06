namespace Fhe;

public sealed class CompactPkeCrs : FheHandle
{
    private CompactPkeCrs(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.CompactPkeCrs.Destroy(handle);

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.CompactPkeCrs.Serialize(Handle, compress: false, out SafeNativeMethods.DynamicBuffer buffer));
        
        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe CompactPkeCrs Deserialize(byte[] data, ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper2(SafeNativeMethods.CompactPkeCrs.Deserialize, buffer_view, serialized_size_limit);
        }
    }

    private static CompactPkeCrs Oper2<A, B>(OperFunc<A, B> func, A a, B b) =>
        new CompactPkeCrs(Oper2n(func, a, b));
}
