using BadgerClan_CodyClient.Library;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace BadgerClan_CodyClient.Maui;

public class CustomGrpcClient : IDisposable {
    private GrpcChannel channel;
    public IStrategyChanger Client { get; }

    public CustomGrpcClient() {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        channel = GrpcChannel.ForAddress("http://localhost:5000");
        Client = channel.CreateGrpcService<IStrategyChanger>();
    }
    public void Dispose() {
        channel.Dispose();
    }
}