using BadgerClan_CodyClient.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BadgerClan_CodyClient.Maui;
public partial class HomeViewModel : ObservableObject {
    private HttpClient client;
    private CustomGrpcClient grpcClient;

    [ObservableProperty]
    private bool localhost;

    [ObservableProperty]
    private bool azure1;

    [ObservableProperty]
    private bool azure2;

    [ObservableProperty]
    private bool grpc;

    [ObservableProperty]
    private StringResponse response;

    public HomeViewModel(HttpClient client, CustomGrpcClient grpcClient) {
        this.client = client;
        this.grpcClient = grpcClient;
    }

    [RelayCommand]
    public async Task ChangeStrategy(string value) {
        if (Localhost) {
            client.BaseAddress = new Uri("http://localhost:1285");
            await client.GetAsync("/change/" + value);
        }
        if (Azure1) {
            client.BaseAddress = new Uri("https://badgerclanbots1-f8h2dgg8e9cdfcc6.westus-01.azurewebsites.net");
            await client.GetAsync("/change/" + value);
        }
        if (Azure2) {
            client.BaseAddress = new Uri("https://badgerclanbots2-bfefg3f7dbapgah8.westus-01.azurewebsites.net");
            await client.GetAsync("/change/" + value);
        }
        if (Grpc) {
            await GrpcCommunication(value);
        }
    }

    [RelayCommand]
    public async Task ChangeSpeed(string value) {
        if (Localhost) {
            client.BaseAddress = new Uri("http://localhost:1285");
            await client.GetAsync("/speed/" + value);
        }
        if (Azure1) {
            client.BaseAddress = new Uri("https://badgerclanbots1-f8h2dgg8e9cdfcc6.westus-01.azurewebsites.net");
            await client.GetAsync("/speed/" + value);
        }
        if (Azure2) {
            client.BaseAddress = new Uri("http://badgerclanbots2-bfefg3f7dbapgah8.westus-01.azurewebsites.net");
            await client.GetAsync("/speed/" + value);
        } 
    }

    private async Task GrpcCommunication(string value) {
        Response = await grpcClient.Client.StrategyChange(new StringRequest { NewStrat = value });
    }
}
