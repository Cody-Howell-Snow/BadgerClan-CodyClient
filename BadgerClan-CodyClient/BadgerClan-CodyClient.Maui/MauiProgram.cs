using BadgerClan_CodyClient.Library;
using CommunityToolkit.Maui;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace BadgerClan_CodyClient.Maui;
public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<CustomGrpcClient>();

        return builder.Build();
    }
}

