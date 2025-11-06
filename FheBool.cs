namespace Fhe;

public sealed class FheBool : FheHandle, IEquatable<FheBool>
{
    private static Lazy<FheBool> _False => new Lazy<FheBool>(() => Encrypt(false));
    public static FheBool False => _False.Value;

    private static Lazy<FheBool> _True => new Lazy<FheBool>(() => Encrypt(true));
    public static FheBool True => _True.Value;

    internal FheBool(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.Bool.Destroy(handle);

    public static FheBool Encrypt(bool value)
    {
        CheckError(SafeNativeMethods.Bool.Encrypt(value ? (byte)1 : (byte)0, Fhe.Instance.ClientKey!.Handle, out nint out_value));
        return new FheBool(out_value);
    }

    public bool Decrypt()
    {
        CheckError(SafeNativeMethods.Bool.Decrypt(Handle, Fhe.Instance.ClientKey!.Handle, out byte out_value));
        return out_value != 0;
    }

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.Bool.Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer));

        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe FheBool Deserialize(byte[] data)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            return Oper1(SafeNativeMethods.Bool.Deserialize, buffer_view);
        }
    }

    private static FheBool Oper1<A>(OperFunc<A> func, A a)
    {
        CheckError(func(a, out nint out_value));
        return new FheBool(out_value);
    }

    private static FheBool Oper2<A, B>(OperFunc<A, B> func, A a, B b)
    {
        CheckError(func(a, b, out nint out_value));
        return new FheBool(out_value);
    }

    public static FheBool operator &(FheBool value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.And, value1.Handle, value2.Handle);
    public static FheBool operator &(FheBool value1, byte value2) =>
        Oper2(SafeNativeMethods.Bool.And, value1.Handle, value2);
    public static FheBool operator &(byte value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.And, value2.Handle, value1);
    public void And(FheBool value) =>
        CheckError(SafeNativeMethods.Bool.AndAssign(Handle, value.Handle));

    public static FheBool operator |(FheBool value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Or, value1.Handle, value2.Handle);
    public static FheBool operator |(FheBool value1, byte value2) =>
        Oper2(SafeNativeMethods.Bool.Or, value1.Handle, value2);
    public static FheBool operator |(byte value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Or, value2.Handle, value1);
    public void Or(FheBool value) =>
        CheckError(SafeNativeMethods.Bool.OrAssign(Handle, value.Handle));

    public static FheBool operator ^(FheBool value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Xor, value1.Handle, value2.Handle);
    public static FheBool operator ^(FheBool value1, byte value2) =>
        Oper2(SafeNativeMethods.Bool.Xor, value1.Handle, value2);
    public static FheBool operator ^(byte value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Xor, value2.Handle, value1);
    public void Xor(FheBool value) =>
        CheckError(SafeNativeMethods.Bool.XorAssign(Handle, value.Handle));

    public static FheBool operator !(FheBool value) =>
        Oper1(SafeNativeMethods.Bool.Not, value.Handle);

    public override bool Equals(object? obj) =>
        obj is FheBool other && Equals(other);
    public bool Equals(FheBool? other) =>
        ReferenceEquals(this, other) ||
        (other is not null && (this == other).Decrypt());

    public static FheBool operator ==(FheBool value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Eq, value1.Handle, value2.Handle);
    public static FheBool operator ==(FheBool value1, bool value2) =>
        Oper2(SafeNativeMethods.Bool.Eq, value1.Handle, (byte)(value2 ? 1 : 0));

    public static FheBool operator !=(FheBool value1, FheBool value2) =>
        Oper2(SafeNativeMethods.Bool.Ne, value1.Handle, value2.Handle);
    public static FheBool operator !=(FheBool value1, bool value2) =>
        Oper2(SafeNativeMethods.Bool.Ne, value1.Handle, (byte)(value2 ? 1 : 0));

    public override int GetHashCode() =>
        throw new InvalidOperationException();
}
