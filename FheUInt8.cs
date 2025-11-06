namespace Fhe;

public sealed class FheUInt8 : FheHandle, IEquatable<FheUInt8>
{
    private static Lazy<FheUInt8> _Zero => new Lazy<FheUInt8>(() => Encrypt(0));
    public static FheUInt8 Zero => _Zero.Value;

    internal FheUInt8(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.UInt8.Destroy(handle);

    public static FheUInt8 Encrypt(byte value)
    {
        CheckError(SafeNativeMethods.UInt8.Encrypt(value, Fhe.Instance.ClientKey!.Handle, out nint out_value));
        return new FheUInt8(out_value);
    }

    public byte Decrypt()
    {
        CheckError(SafeNativeMethods.UInt8.Decrypt(Handle, Fhe.Instance.ClientKey!.Handle, out byte out_value));
        return out_value;
    }

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.UInt8.Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer));
        
        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
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

            return Oper1(SafeNativeMethods.UInt8.Deserialize, buffer_view);
        }
    }

    private static FheUInt8 Oper1<A>(OperFunc<A> func, A a)
    {
        CheckError(func(a, out nint out_value));
        return new FheUInt8(out_value);
    }

    private static FheUInt8 Oper2<A, B>(OperFunc<A, B> func, A a, B b) =>
        new FheUInt8(Oper2n(func, a, b));

    public static FheUInt8 operator +(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Add, value1.Handle, value2.Handle);
    public static FheUInt8 operator +(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Add, value1.Handle, value2);
    public static FheUInt8 operator +(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Add, value2.Handle, value1);
    public void Add(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.AddAssign(Handle, value.Handle));

    public static FheUInt8 operator -(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Sub, value1.Handle, value2.Handle);
    public static FheUInt8 operator -(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Sub, value1.Handle, value2);
    public static FheUInt8 operator -(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Sub, value2.Handle, value1);
    public void Sub(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.SubAssign(Handle, value.Handle));

    public static FheUInt8 operator *(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Mul, value1.Handle, value2.Handle);
    public static FheUInt8 operator *(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Mul, value1.Handle, value2);
    public static FheUInt8 operator *(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Mul, value2.Handle, value1);
    public void Mul(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.MulAssign(Handle, value.Handle));

    public static FheUInt8 operator /(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Div, value1.Handle, value2.Handle);
    public static FheUInt8 operator /(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Div, value1.Handle, value2);
    public static FheUInt8 operator /(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Div, value2.Handle, value1);
    public void Div(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.DivAssign(Handle, value.Handle));

    public static FheUInt8 operator &(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.And, value1.Handle, value2.Handle);
    public static FheUInt8 operator &(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.And, value1.Handle, value2);
    public static FheUInt8 operator &(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.And, value2.Handle, value1);
    public void And(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.AndAssign(Handle, value.Handle));

    public static FheUInt8 operator |(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Or, value1.Handle, value2.Handle);
    public static FheUInt8 operator |(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Or, value1.Handle, value2);
    public static FheUInt8 operator |(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Or, value2.Handle, value1);
    public void Or(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.OrAssign(Handle, value.Handle));

    public static FheUInt8 operator ^(FheUInt8 value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Xor, value1.Handle, value2.Handle);
    public static FheUInt8 operator ^(FheUInt8 value1, byte value2) =>
        Oper2(SafeNativeMethods.UInt8.Xor, value1.Handle, value2);
    public static FheUInt8 operator ^(byte value1, FheUInt8 value2) =>
        Oper2(SafeNativeMethods.UInt8.Xor, value2.Handle, value1);
    public void Xor(FheUInt8 value) =>
        CheckError(SafeNativeMethods.UInt8.XorAssign(Handle, value.Handle));

    public static FheUInt8 operator !(FheUInt8 value) =>
        Oper1(SafeNativeMethods.UInt8.Not, value.Handle);
    public static FheUInt8 operator -(FheUInt8 value) =>
        Oper1(SafeNativeMethods.UInt8.Neg, value.Handle);

    public FheUInt8 rotl(FheUInt8 count) =>
        Oper2(SafeNativeMethods.UInt8.RotateLeft, Handle, count.Handle);
    public static FheUInt8 operator <<(FheUInt8 value, byte count) =>
        count >= 8 ? Zero : Oper2(SafeNativeMethods.UInt8.ShiftLeft, value.Handle, count);

    public FheUInt8 rotr(FheUInt8 count) =>
        Oper2(SafeNativeMethods.UInt8.RotateRight, Handle, count.Handle);
    public static FheUInt8 operator >>(FheUInt8 value, byte count) =>
        count >= 8 ? Zero : Oper2(SafeNativeMethods.UInt8.ShiftRight, value.Handle, count);

    public override bool Equals(object? obj) =>
        obj is FheUInt8 other && Equals(other);
    public bool Equals(FheUInt8? other) =>
        ReferenceEquals(this, other) ||
        ((object?)other != null && (this == other).Decrypt());

    public static FheBool operator ==(FheUInt8 value1, FheUInt8 value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8.Eq, value1.Handle, value2.Handle));
    public static FheBool operator ==(FheUInt8 value1, byte value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8.Eq, value1.Handle, value2));

    public static FheBool operator !=(FheUInt8 value1, FheUInt8 value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8.Ne, value1.Handle, value2.Handle));
    public static FheBool operator !=(FheUInt8 value1, byte value2) =>
        new FheBool(Oper2n(SafeNativeMethods.UInt8.Ne, value1.Handle, value2));

    public override int GetHashCode() =>
        throw new InvalidOperationException();
}
