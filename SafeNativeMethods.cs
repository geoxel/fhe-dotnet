using System.Runtime.InteropServices;

namespace Fhe;

internal static partial class SafeNativeMethods
{
#if OS_WINDOWS
    private const string LibraryPrefix = "";
    private const string LibraryExtension = ".dll";
#elif OS_LINUX
    private const string LibraryPrefix = "lib";
    private const string LibraryExtension = ".so";
#elif OS_MACOS
    private const string LibraryPrefix = "lib";
    private const string LibraryExtension = ".dylib";
#else
#error Unsupported platform
#endif

    private const string LibraryPath = "../tfhe-rs/target/release/" + LibraryPrefix + "tfhe" + LibraryExtension;

    [LibraryImport(LibraryPath, EntryPoint = "config_builder_default")]
    public static partial int ConfigBuilderDefault(out nint config_builder);

    [LibraryImport(LibraryPath, EntryPoint = "config_builder_build")]
    public static partial int ConfigBuilderBuild(nint config_builder, out nint config);

    [LibraryImport(LibraryPath, EntryPoint = "generate_keys")]
    public static partial int GenerateKeys(nint config, out nint client_key, out nint server_key);

    [LibraryImport(LibraryPath, EntryPoint = "set_server_key")]
    public static partial int SetServerKey(nint server_key);

    [LibraryImport(LibraryPath, EntryPoint = "client_key_destroy")]
    public static partial int ClientKey_Destroy(nint client_key);

    [LibraryImport(LibraryPath, EntryPoint = "server_key_destroy")]
    public static partial int ServerKey_Destroy(nint server_key);

    public static partial class CompactPublicKey
    {
        [LibraryImport(LibraryPath, EntryPoint = "compact_public_key_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_public_key_serialize")]
        public static partial int Serialize(nint value, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "compact_public_key_safe_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, ulong serialized_size_limit, out nint out_value);
    }

    public static partial class CompactPkeCrs
    {
        [LibraryImport(LibraryPath, EntryPoint = "compact_pke_crs_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_pke_crs_serialize")]
        public static partial int Serialize(nint value, [MarshalAs(UnmanagedType.U1)] bool compress, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "compact_pke_crs_safe_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, ulong serialized_size_limit, out nint out_value);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DynamicBuffer
    {
        public nint pointer;
        public nint length;
        public nint destructor;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DynamicBufferView
    {
        public nint pointer;
        public nint length;
    };

    public static byte[] DynamicBuffer_ToArray(DynamicBuffer buffer)
    {
        const int MaxArraySize = 0x7FFFFFC7;
        if (buffer.length > MaxArraySize)
            throw new FheException(1); // TODO: use a better error code

        var result = new byte[(int)buffer.length];
        Marshal.Copy(buffer.pointer, result, 0, result.Length);
        return result;
    }

    [LibraryImport(LibraryPath, EntryPoint = "destroy_dynamic_buffer")]
    public static partial int DynamicBuffer_Destroy(ref DynamicBuffer buffer);

    // public static partial class CompactCiphertextList
    // {
    //     [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_destroy")]
    //     public static partial int Destroy(nint value);
    // }

    public static partial class ProvenCompactCiphertextList
    {
        [LibraryImport(LibraryPath, EntryPoint = "proven_compact_ciphertext_list_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_build_with_proof_packed")]
        public static partial int BuildWithProof(
            nint builder,
            nint crs,
            nint metadata,
            nint metadata_len,
            int compute_load,
            out nint ProvenCompactCiphertextList);

        [LibraryImport(LibraryPath, EntryPoint = "proven_compact_ciphertext_list_serialize")]
        public static partial int Serialize(nint value, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "proven_compact_ciphertext_list_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, out nint out_value);
    }

    public static partial class CompactCiphertextListBuilder
    {
        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_new")]
        public static partial int New(nint publicKey, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_bool")]
        public static partial int PushBool(nint builder, [MarshalAs(UnmanagedType.U1)] bool value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u8")]
        public static partial int PushU8(nint builder, byte value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u16")]
        public static partial int PushU16(nint builder, ushort value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u32")]
        public static partial int PushU32(nint builder, uint value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u64")]
        public static partial int PushU64(nint builder, ulong value);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u128")]
        public static partial int PushU128(nint builder, ulong lower, ulong upper);

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u256")]
        public static partial int PushU256(nint builder, ulong u0, ulong u1, ulong u2, ulong u3); // little endian

        [LibraryImport(LibraryPath, EntryPoint = "compact_ciphertext_list_builder_push_u160")]
        public static partial int PushU160(nint builder, ulong u0, ulong u1, ulong u2, ulong u3); // little endian
    }

    public static partial class Bool
    {
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_try_encrypt_with_client_key_bool")]
        public static partial int Encrypt(byte value, nint client_key, out nint fhe);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_decrypt")]
        public static partial int Decrypt(nint fhe, nint client_key, out byte out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_serialize")]
        public static partial int Serialize(nint value, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitand")]
        public static partial int And(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_scalar_bitand")]
        public static partial int And(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitand_assign")]
        public static partial int AndAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitor")]
        public static partial int Or(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_scalar_bitor")]
        public static partial int Or(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitor_assign")]
        public static partial int OrAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitxor")]
        public static partial int Xor(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_scalar_bitxor")]
        public static partial int Xor(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_bitxor_assign")]
        public static partial int XorAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_not")]
        public static partial int Not(nint value, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_eq")]
        public static partial int Eq(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_scalar_eq")]
        public static partial int Eq(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_ne")]
        public static partial int Ne(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_bool_scalar_ne")]
        public static partial int Ne(nint value1, byte value2, out nint out_value);
    }

    public static partial class UInt8
    {
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_try_encrypt_with_client_key_u8")]
        public static partial int Encrypt(byte value, nint client_key, out nint fhe);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_decrypt")]
        public static partial int Decrypt(nint fhe, nint client_key, out byte out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_serialize")]
        public static partial int Serialize(nint value, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_add")]
        public static partial int Add(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_add")]
        public static partial int Add(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_add_assign")]
        public static partial int AddAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_sub")]
        public static partial int Sub(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_sub")]
        public static partial int Sub(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_sub_assign")]
        public static partial int SubAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_mul")]
        public static partial int Mul(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_mul")]
        public static partial int Mul(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_mul_assign")]
        public static partial int MulAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_div")]
        public static partial int Div(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_div")]
        public static partial int Div(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_div_assign")]
        public static partial int DivAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_rotate_left")]
        public static partial int RotateLeft(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_shl")]
        public static partial int ShiftLeft(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_rotate_right")]
        public static partial int RotateRight(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_shr")]
        public static partial int ShiftRight(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_neg")]
        public static partial int Neg(nint value, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitand")]
        public static partial int And(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_bitand")]
        public static partial int And(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitand_assign")]
        public static partial int AndAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitor")]
        public static partial int Or(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_bitor")]
        public static partial int Or(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitor_assign")]
        public static partial int OrAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitxor")]
        public static partial int Xor(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_bitxor")]
        public static partial int Xor(nint value1, byte value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_bitxor_assign")]
        public static partial int XorAssign(nint dst_value, nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_not")]
        public static partial int Not(nint value, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_eq")]
        public static partial int Eq(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_eq")]
        public static partial int Eq(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_ne")]
        public static partial int Ne(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_ne")]
        public static partial int Ne(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_ge")]
        public static partial int Ge(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_ge")]
        public static partial int Ge(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_gt")]
        public static partial int Gt(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_gt")]
        public static partial int Gt(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_le")]
        public static partial int Le(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_le")]
        public static partial int Le(nint value1, byte value2, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_lt")]
        public static partial int Lt(nint value1, nint value2, out nint out_value);
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint8_scalar_lt")]
        public static partial int Lt(nint value1, byte value2, out nint out_value);
    }

    public static partial class UInt32
    {
        // UInt32
        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_try_encrypt_with_client_key_u32")]
        public static partial int Encrypt(uint value, nint client_key, out nint fhe);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_decrypt")]
        public static partial int Decrypt(nint fhe, nint client_key, out uint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_destroy")]
        public static partial int Destroy(nint value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_serialize")]
        public static partial int Serialize(nint value, out DynamicBuffer out_buffer);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_deserialize")]
        public static partial int Deserialize(DynamicBufferView buffer_view, out nint out_value);

        [LibraryImport(LibraryPath, EntryPoint = "fhe_uint32_add")]
        public static partial int Add(nint value1, nint value2, out nint out_value);
    }
}
