namespace DockerEngine;

internal static class HttpMessageHandlerFactory
{
    public static HttpMessageHandler GetHttpMessageHandler(Uri dockerEndpoint)
    {
        switch (dockerEndpoint.Scheme)
        {
            case "http":
            case "https":
                return new HttpClientHandler();
            case "tcp":
                return new HttpClientHandler();
            case "npipe":
                return new SocketsHttpHandler { ConnectCallback = (_, _) => NamedPipeMessageHandler(dockerEndpoint) };
            case "unix":
                return new SocketsHttpHandler { ConnectCallback = (_, _) => UnixSocketMessageHandler(dockerEndpoint) };
            default:
                throw new InvalidOperationException($"The Docker scheme {dockerEndpoint.Scheme} is not supported.");
        }
    }

    private static ValueTask<Stream> NamedPipeMessageHandler(Uri dockerEndpoint)
    {
        var stream = new NamedPipeClientStream(".", dockerEndpoint.AbsolutePath, PipeDirection.InOut, PipeOptions.Asynchronous);
        stream.Connect();
        return new ValueTask<Stream>(stream);
    }

    private static ValueTask<Stream> UnixSocketMessageHandler(Uri dockerEndpoint)
    {
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
        socket.Connect(new UnixDomainSocketEndPoint(dockerEndpoint.AbsolutePath));
        return new ValueTask<Stream>(new NetworkStream(socket, true));
    }
}