namespace BadgerClan_CodyClient.Maui;

public partial class MainPage : ContentPage {

    public MainPage(HomeViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}

