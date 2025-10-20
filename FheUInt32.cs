using System;

namespace Fhe;

public sealed class FheUInt32 : FheHandle
{
    private static Lazy<FheUInt32> _Zero => new Lazy<FheUInt32>(() => Encrypt(0));
    public static FheUInt32 Zero => _Zero.Value;

    internal FheUInt32(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.UInt32_Destroy(handle);

    public static FheUInt32 Encrypt(uint value)
    {
        int error = SafeNativeMethods.UInt32_Encrypt(value, Fhe.Instance.ClientKey!.Handle, out nint out_value);
        if (error != 0)
            throw new FheException(error);
        return new FheUInt32(out_value);
    }

    public uint Decrypt()
    {
        int error = SafeNativeMethods.UInt32_Decrypt(Handle, Fhe.Instance.ClientKey!.Handle, out uint out_value);
        if (error != 0)
            throw new FheException(error);
        return out_value;
    }

    public byte[] Serialize()
    {
        int error = SafeNativeMethods.UInt32_Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer);
        if (error != 0)
            throw new FheException(error);
        return SafeNativeMethods.DynamicBuffer_ToArray(buffer);
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

            return Oper1(SafeNativeMethods.UInt32_Deserialize, buffer_view);
        }
    }

    private static FheUInt32 Oper1<A>(OperFunc<A> func, A a)
    {
        int error = func(a, out nint out_value);
        if (error != 0)
            throw new FheException(error);
        return new FheUInt32(out_value);
    }

    private static FheUInt32 Oper2<A, B>(OperFunc<A, B> func, A a, B b)
    {
        int error = func(a, b, out nint out_value);
        if (error != 0)
            throw new FheException(error);
        return new FheUInt32(out_value);
    }

    public static FheUInt32 operator +(FheUInt32 value1, FheUInt32 value2) =>
        Oper2(SafeNativeMethods.UInt32_Add, value1.Handle, value2.Handle);
}
