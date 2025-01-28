using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BadgerClan_CodyClient.Maui;
public partial class HomeViewModel : ObservableObject {
    private HttpClient client;

    public HomeViewModel(HttpClient client) {
        client.BaseAddress = new Uri("http://localhost:1285");
        this.client = client;
    }

    [RelayCommand]
    public async Task ChangeStrategy(string value) {
        await client.GetAsync("/change/" + value);
    }
}
