using System;
using System.Dynamic;
using System.Threading;

namespace Fhe;

public sealed class Fhe : IDisposable
{
    public static Fhe Instance { get; private set; } = new Fhe();

    public Fhe()
    {
        GenerateKeys();
    }

    public void Dispose()
    {
        ClientKey?.Dispose();
        ClientKey = null;

        ServerKey?.Dispose();
        ServerKey = null;
    }

    public FheClientKey? ClientKey { get; private set; }
    public FheServerKey? ServerKey { get; private set; }

    private void GenerateKeys()
    {
        int error = SafeNativeMethods.ConfigBuilderDefault(out nint config_builder);
        if (error != 0)
            throw new FheException(error);

        error = SafeNativeMethods.ConfigBuilderBuild(config_builder, out nint config);
        if (error != 0)
            throw new FheException(error);

        error = SafeNativeMethods.GenerateKeys(config, out nint client_key, out nint server_key);
        if (error != 0)
            throw new FheException(error);

        error = SafeNativeMethods.SetServerKey(server_key);
        if (error != 0)
        {
            SafeNativeMethods.ServerKey_Destroy(server_key);
            SafeNativeMethods.ClientKey_Destroy(client_key);
            throw new FheException(error);
        }

        ClientKey = new FheClientKey(client_key);
        ServerKey = new FheServerKey(server_key);
    }
}
