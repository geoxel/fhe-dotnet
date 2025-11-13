namespace Fhe;

public enum FheValueType
{
    Bool = 0,
    UInt8 = 2,
    UInt16 = 3,
    UInt32 = 4,
    UInt64 = 5,
    UInt128 = 6,
    Address = 7, // a.k.a. UInt160
    UInt256 = 8,
    Bytes64 = 9,
    Bytes128 = 10,
    Bytes256 = 11,
}
