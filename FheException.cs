namespace Fhe;

public sealed class FheException : Exception
{
    public int Error { get; }

    public FheException()
    {
    }

    public FheException(int error)
    {
        Error = error;
    }
}
