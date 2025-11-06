namespace Fhe;

public abstract class FheHandle : IDisposable
{
    private nint _handle;

    public nint Handle => _handle;

    protected FheHandle(nint handle)
    {
        _handle = handle;
    }

    ~FheHandle()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        nint handle = Interlocked.CompareExchange(ref _handle, value: IntPtr.Zero, comparand: _handle);
        if (handle != IntPtr.Zero)
            DestroyHandle(handle);
    }

    protected abstract void DestroyHandle(nint handle);

    internal const ulong SAFE_SER_SIZE_LIMIT = 1024UL * 1024 * 1024 * 2;

    protected static void CheckError(int error)
    {
        if (error != 0)
            throw new FheException(error);
    }

    internal delegate int OperFunc<in T1>(T1 arg1, out nint result);
    internal delegate int OperFunc<in T1, in T2>(T1 arg1, T2 arg2, out nint result);

    internal static nint Oper1n<A>(OperFunc<A> func, A a)
    {
        CheckError(func(a, out nint out_value));
        return out_value;
    }

    internal static nint Oper2n<A, B>(OperFunc<A, B> func, A a, B b)
    {
        CheckError(func(a, b, out nint out_value));
        return out_value;
    }
}
