namespace Fhe;

public sealed class FheUInt32 : FheHandle
{
    private static Lazy<FheUInt32> _Zero => new Lazy<FheUInt32>(() => Encrypt(0));
    public static FheUInt32 Zero => _Zero.Value;

    internal FheUInt32(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.UInt32.Destroy(handle);

    public static FheUInt32 Encrypt(uint value)
    {
        CheckError(SafeNativeMethods.UInt32.Encrypt(value, Fhe.Instance.ClientKey!.Handle, out nint out_value));
        return new FheUInt32(out_value);
    }

    public uint Decrypt()
    {
        CheckError(SafeNativeMethods.UInt32.Decrypt(Handle, Fhe.Instance.ClientKey!.Handle, out uint out_value));
        return out_value;
    }

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.UInt32.Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer));

        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe FheUInt32 Deserialize(byte[] data)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper1(SafeNativeMethods.UInt32.Deserialize, buffer_view);
        }
    }

    private static FheUInt32 Oper1<A>(OperFunc<A> func, A a)
    {
        CheckError(func(a, out nint out_value));
        return new FheUInt32(out_value);
    }

    private static FheUInt32 Oper2<A, B>(OperFunc<A, B> func, A a, B b)
    {
        CheckError(func(a, b, out nint out_value));
        return new FheUInt32(out_value);
    }

    public static FheUInt32 operator +(FheUInt32 value1, FheUInt32 value2) =>
        Oper2(SafeNativeMethods.UInt32.Add, value1.Handle, value2.Handle);
}
