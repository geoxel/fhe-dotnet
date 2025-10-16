using System;

namespace Fhe;

public sealed class FheUInt8 : FheHandle, IEquatable<FheUInt8>
{
    private static Lazy<FheUInt8> _Zero => new Lazy<FheUInt8>(() => Encrypt(0));
    public static FheUInt8 Zero => _Zero.Value;

    internal FheUInt8(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.UInt8_Destroy(handle);

    public static FheUInt8 Encrypt(byte value)
    {
        int error = SafeNativeMethods.UInt8_Encrypt(value, Fhe.Instance.ClientKey!.Handle, out nint out_value);
        if (error != 0)
            throw new FheException(error);
        return new FheUInt8(out_value);
    }

    public byte Decrypt()
    {
        int error = SafeNativeMethods.UInt8_Decrypt(Handle, Fhe.Instance.ClientKey!.Handle, out byte out_value);
        if (error != 0)
            throw new FheException(error);
        return out_value;
    }

    public byte[] Serialize()
    {
        int error = SafeNativeMethods.UInt8_Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer);
        if (error != 0)
            throw new FheException(error);
        return SafeNativeMethods.DynamicBuffer_ToArray(buffer);
    }

    public static unsafe FheUInt8 Deserialize(byte[] data)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper1(SafeNativeMethods.UInt8_Deserialize, buffer_view);
        }
    }

    private static FheUInt8 Oper1<A>(Oper1Func<A> func, A a)
    {
        int error = func(a, out nint out_value);
        if (error != 0)
            throw new FheException(error);
        return new FheUInt8(out_value);
    }

    private static FheUInt8 Oper2<A, B>(Oper2Func<A, B> func, A a, B b) =>
        new FheUInt8(Oper2n(func, a, b));

    public static FheUInt8 operator +(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Add, value1.Handle, value2.Handle);
    public static FheUInt8 operator +(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Add, value1.Handle, value2);
    public static FheUInt8 operator +(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Add, value2.Handle, value1);
    public void Add(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_AddAssign, Handle, value.Handle);

    public static FheUInt8 operator -(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Sub, value1.Handle, value2.Handle);
    public static FheUInt8 operator -(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Sub, value1.Handle, value2);
    public static FheUInt8 operator -(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Sub, value2.Handle, value1);
    public void Sub(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_SubAssign, Handle, value.Handle);

    public static FheUInt8 operator *(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Mul, value1.Handle, value2.Handle);
    public static FheUInt8 operator *(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Mul, value1.Handle, value2);
    public static FheUInt8 operator *(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Mul, value2.Handle, value1);
    public void Mul(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_MulAssign, Handle, value.Handle);

    public static FheUInt8 operator /(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Div, value1.Handle, value2.Handle);
    public static FheUInt8 operator /(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Div, value1.Handle, value2);
    public static FheUInt8 operator /(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Div, value2.Handle, value1);
    public void Div(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_DivAssign, Handle, value.Handle);

    public static FheUInt8 operator &(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_And, value1.Handle, value2.Handle);
    public static FheUInt8 operator &(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_And, value1.Handle, value2);
    public static FheUInt8 operator &(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_And, value2.Handle, value1);
    public void And(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_AndAssign, Handle, value.Handle);

    public static FheUInt8 operator |(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Or, value1.Handle, value2.Handle);
    public static FheUInt8 operator |(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Or, value1.Handle, value2);
    public static FheUInt8 operator |(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Or, value2.Handle, value1);
    public void Or(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_OrAssign, Handle, value.Handle);

    public static FheUInt8 operator ^(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Xor, value1.Handle, value2.Handle);
    public static FheUInt8 operator ^(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8_Xor, value1.Handle, value2);
    public static FheUInt8 operator ^(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8_Xor, value2.Handle, value1);
    public void Xor(FheUInt8 value) =>
        Oper2nr(SafeNativeMethods.UInt8_XorAssign, Handle, value.Handle);

    public static FheUInt8 operator !(FheUInt8 value) =>
        Oper1(SafeNativeMethods.UInt8_Not, value.Handle);
    public static FheUInt8 operator -(FheUInt8 value) =>
        Oper1(SafeNativeMethods.UInt8_Neg, value.Handle);

    public FheUInt8 rotl(FheUInt8 count) =>
        Oper2(SafeNativeMethods.UInt8_RotateLeft, Handle, count.Handle);
    public static FheUInt8 operator <<(FheUInt8 value, byte count) =>
        count >= 8 ? Zero : Oper2(SafeNativeMethods.UInt8_ShiftLeft, value.Handle, count);

    public FheUInt8 rotr(FheUInt8 count) =>
        Oper2(SafeNativeMethods.UInt8_RotateRight, Handle, count.Handle);
    public static FheUInt8 operator >>(FheUInt8 value, byte count) =>
        count >= 8 ? Zero : Oper2(SafeNativeMethods.UInt8_ShiftRight, value.Handle, count);

    public override bool Equals(object? obj) =>
        obj is FheUInt8 other && Equals(other);
    public bool Equals(FheUInt8? other) =>
        ReferenceEquals(this, other) ||
        ((object?)other != null && (this == other).Decrypt());

    public static FheBool operator ==(FheUInt8 value1, FheUInt8 value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8_Eq, value1.Handle, value2.Handle));
    public static FheBool operator ==(FheUInt8 value1, byte value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8_Eq, value1.Handle, value2));

    public static FheBool operator !=(FheUInt8 value1, FheUInt8 value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8_Ne, value1.Handle, value2.Handle));
    public static FheBool operator !=(FheUInt8 value1, byte value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8_Ne, value1.Handle, value2));

    public override int GetHashCode() =>
        throw new InvalidOperationException();
}
