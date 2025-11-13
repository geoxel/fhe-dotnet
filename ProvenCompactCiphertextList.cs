namespace Fhe;

public sealed class ProvenCompactCiphertextList : FheHandle
{
    private ProvenCompactCiphertextList(nint handle) : base(handle)
    {
    }

    protected override void DestroyHandle(nint handle) =>
        SafeNativeMethods.ProvenCompactCiphertextList.Destroy(handle);

    public unsafe static ProvenCompactCiphertextList BuildWithProof(
        CompactCiphertextListBuilder builder,
        CompactPkeCrs crs,
        byte[] metadata,
        ZkComputeLoad computeLoad)
    {
        fixed (byte* metadata_ptr = metadata)
        {
            CheckError(SafeNativeMethods.ProvenCompactCiphertextList.BuildWithProof(
                builder.Handle,
                crs.Handle,
                new nint(metadata_ptr),
                metadata.Length,
                (byte)computeLoad,
                out nint out_value));

            return new ProvenCompactCiphertextList(out_value);
        }
    }

    public byte[] Serialize()
    {
        CheckError(SafeNativeMethods.ProvenCompactCiphertextList.Serialize(Handle, out SafeNativeMethods.DynamicBuffer buffer));

        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe ProvenCompactCiphertextList Deserialize(byte[] data)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            CheckError(SafeNativeMethods.ProvenCompactCiphertextList.Deserialize(buffer_view, out nint out_value));
            return new ProvenCompactCiphertextList(out_value);
        }
    }

    public byte[] SafeSerialize(ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        CheckError(SafeNativeMethods.ProvenCompactCiphertextList.SafeSerialize(Handle, out SafeNativeMethods.DynamicBuffer buffer, serialized_size_limit));

        using DynamicBuffer dynamicbuffer = new(buffer);
        return dynamicbuffer.ToArray();
    }

    public static unsafe ProvenCompactCiphertextList SafeDeserialize(byte[] data, ulong serialized_size_limit = SAFE_SER_SIZE_LIMIT)
    {
        fixed (byte* ptr = data)
        {
            var buffer_view = new SafeNativeMethods.DynamicBufferView
            {
                pointer = new nint(ptr),
                length = data.Length,
            };

            CheckError(SafeNativeMethods.ProvenCompactCiphertextList.SafeDeserialize(buffer_view, serialized_size_limit, out nint out_value));
            return new ProvenCompactCiphertextList(out_value);
        }
    }
}
