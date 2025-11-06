// https://github.com/NethermindEth/int256/tree/main
using Nethermind.Int256;

namespace Fhe;

public sealed class CompactCiphertextListBuilder : FheHandle
{
    private CompactCiphertextListBuilder(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.CompactCiphertextListBuilder.Destroy(handle);

    public static CompactCiphertextListBuilder Create(CompactPublicKey publicKey)
    {
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.New(publicKey.Handle, out nint out_value));
        return new CompactCiphertextListBuilder(out_value);
    }

    public void PushBool(bool value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushBool(Handle, value));

    public void PushU8(byte value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU8(Handle, value));

    public void PushU16(ushort value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU16(Handle, value));

    public void PushU32(uint value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU32(Handle, value));

    public void PushU64(ulong value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU64(Handle, value));

    public void PushU128(UInt128 value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU128(Handle, (ulong)value, (ulong)(value >> 64)));

    public void PushU256(UInt256 value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU256(Handle, value.u0, value.u1, value.u2, value.u3));

    public void Push160(UInt256 value) =>
        CheckError(SafeNativeMethods.CompactCiphertextListBuilder.PushU160(Handle, value.u0, value.u1, value.u2, value.u3));
}
